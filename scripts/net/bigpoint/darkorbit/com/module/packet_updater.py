# packet_updater.py
# Python 3 script — heuristische Generator für Write()-Methoden aus Read()-Methoden in C# Command-Klassen.
# Benutzung: python3 packet_updater.py <pfad-zum-Commands-ordner>
# Das Skript erstellt .bak-Backups der geänderten Dateien.

import re
import os
import sys
from pathlib import Path

# Regex helpers
RE_READ_METHOD = re.compile(r'public\s+override\s+void\s+Read\s*\(\s*ByteArray\s+reader\s*\)\s*\{', re.MULTILINE)
RE_WRITE_METHOD = re.compile(r'public\s+override\s+byte\[\]\s+Write\s*\(\s*\)\s*\{', re.MULTILINE)
RE_METHOD_HEADER = re.compile(r'(public\s+override\s+(?:void|byte\[\])\s+\w+\s*\([^\)]*\)\s*\{)', re.MULTILINE)

# Basic read->write mapping
SIMPLE_MAP = {
    r'reader\.ReadInt\(\)': 'WriteInt',
    r'reader\.ReadShort\(\)': 'WriteShort',
    r'reader\.ReadUTF\(\)': 'WriteUTF',
    r'reader\.ReadBool\(\)': 'WriteBool',
    r'reader\.ReadDouble\(\)': 'WriteDouble',
    r'reader\.ReadFloat\(\)': 'WriteFloat',
    r'reader\.ReadByte\(\)': 'WriteByte',
}

ROT_LEFT_PATTERN = re.compile(r'this\.(?P<f>\w+)\s*=\s*this\.(?P=f)\s*<<\s*(?P<n>\d+)\s*\|\|\s*this\.(?P=f)\s*>>>\s*(?P<m>\d+)\s*;')
ROT_RIGHT_PATTERN = re.compile(r'this\.(?P<f>\w+)\s*=\s*this\.(?P=f)\s*>>>\s*(?P<n>\d+)\s*\|\|\s*this\.(?P=f)\s*<<\s*(?P<m>\d+)\s*;')

# also accept patterns with single '|' instead of '||' (in C# it's | for bitwise)
ROT_LEFT_PATTERN2 = re.compile(r'this\.(?P<f>\w+)\s*=\s*this\.(?P=f)\s*<<\s*(?P<n>\d+)\s*\|\s*this\.(?P=f)\s*>>>\s*(?P<m>\d+)\s*;')
ROT_RIGHT_PATTERN2 = re.compile(r'this\.(?P<f>\w+)\s*=\s*this\.(?P=f)\s*>>>\s*(?P<n>\d+)\s*\|\s*this\.(?P=f)\s*<<\s*(?P<m>\d+)\s*;')

# helper to inverse rotation expression (32-bit)
def inverse_rotation_expr(field, direction, n):
    n = int(n) % 32
    if direction == 'left':
        # read did field = (field << n) | (field >>> 32-n)
        # inverse (what to write) is: field >>> n | field << (32 - n)
        return f"this.@byte.WriteInt(this.{field} >>> {n} | this.{field} << {32 - n});"
    else:
        # read did field = (field >>> n) | (field << 32-n)
        # inverse write: field << n | field >>> (32 - n)
        return f"this.@byte.WriteInt(this.{field} << {n} | this.{field} >>> {32 - n});"

def extract_block(lines, start_index):
    """Given list of lines and index to '{' line, return block lines and end index (brace count)."""
    block = []
    i = start_index
    brace = 0
    started = False
    while i < len(lines):
        line = lines[i]
        if '{' in line:
            brace += line.count('{')
            started = True
        if '}' in line:
            brace -= line.count('}')
        if started:
            block.append(line)
        i += 1
        if started and brace <= 0:
            break
    return block, i

def generate_write_from_read(read_block):
    """
    Heuristically convert read-block (list of lines) into write-lines.
    Returns list of strings suitable to place in Write() before the final ToArray.
    """
    write_lines = []
    i = 0
    while i < len(read_block):
        line = read_block[i].strip()
        # skip empty or braces
        if not line or line in ['{', '}']:
            i += 1
            continue

        # rotation right pattern
        m = ROT_RIGHT_PATTERN.search(line) or ROT_RIGHT_PATTERN2.search(line)
        if m:
            field = m.group('f')
            n = m.group('n')
            write_lines.append(inverse_rotation_expr(field, 'right', n))
            i += 1
            continue

        # rotation left pattern
        m = ROT_LEFT_PATTERN.search(line) or ROT_LEFT_PATTERN2.search(line)
        if m:
            field = m.group('f')
            n = m.group('n')
            write_lines.append(inverse_rotation_expr(field, 'left', n))
            i += 1
            continue

        # simple reader calls assigned to fields (no rotation)
        assign_read = re.match(r'this\.(?P<f>\w+)\s*=\s*reader\.(?P<call>\w+)\s*\(\s*\)\s*;', line)
        if assign_read:
            field = assign_read.group('f')
            call = assign_read.group('call')
            wcall = 'Write' + call[0].upper() + call[1:]
            write_lines.append(f"this.@byte.{wcall}(this.{field});")
            i += 1
            continue

        # direct reader calls without assignment (e.g., reader.ReadShort(); used for control)
        rc = re.match(r'reader\.(?P<call>\w+)\s*\(\s*\)\s*;', line)
        if rc:
            call = rc.group('call')
            if call.lower() == 'readshort':
                # ambiguous — often used as marker for presence of module or count. Add TODO.
                write_lines.append("// TODO: write marker short (was reader.ReadShort()) — ensure correct value (0/1 or length).")
                write_lines.append("this.@byte.WriteShort(0);")
            else:
                write_lines.append(f"// TODO: reader.{call}() seen; manual check required.")
            i += 1
            continue

        # UTF/Bool/Double/Float/Byte/Short already covered above by assigned case,
        # handle simple list pattern: var i=0; var max = reader.ReadByte(); while (i<max) { ... }
        if 'ReadByte()' in line and ('max' in line or 'ReadByte' in line):
            write_lines.append("// TODO: array/list detected — write count and iterate items.")
            write_lines.append("// Example: this.@byte.WriteByte((byte)this.myList.Count); foreach(var e in this.myList) { /* write element */ }")
            i += 1
            continue

        # conditional module pattern: if (reader.ReadShort() != 0) { this.toolTip.Read(reader); }
        m_cond = re.match(r'if\s*\(\s*reader\.ReadShort\(\)\s*!=\s*0\s*\)\s*\{', line)
        if m_cond:
            # try to find the inner call line like this.toolTip.Read(reader);
            # add counterpart: if (this.toolTip != null) { this.@byte.WriteShort(1); this.toolTip.Write(this.@byte); } else this.@byte.WriteShort(0);
            write_lines.append("// conditional module presence detected")
            write_lines.append("if (this.toolTip != null) {")
            write_lines.append("    this.@byte.WriteShort(1);")
            write_lines.append("    this.toolTip.Write(this.@byte);")
            write_lines.append("} else {")
            write_lines.append("    this.@byte.WriteShort(0);")
            write_lines.append("}")
            # advance until matching closing brace of if-block
            # crude skip
            j = i + 1
            brace = 1
            while j < len(read_block) and brace > 0:
                if '{' in read_block[j]:
                    brace += read_block[j].count('{')
                if '}' in read_block[j]:
                    brace -= read_block[j].count('}')
                j += 1
            i = j
            continue

        # fallback: insert comment and keep line for manual checking
        write_lines.append(f"// TODO (manual): could not auto-convert: {line}")
        i += 1

    return write_lines

def process_file(path: Path):
    text = path.read_text(encoding='utf-8')
    if 'public override void Read(ByteArray reader)' not in text:
        return False, "no Read method"

    # find read method start
    read_start = text.find('public override void Read(ByteArray reader)')
    if read_start < 0:
        return False, "no Read signature"

    # extract read block by counting braces from first '{' after signature
    idx = text.find('{', read_start)
    if idx < 0:
        return False, "malformed Read"

    # crude splitting to lines for brace matching
    lines = text[idx:].splitlines()
    read_block, read_end_offset = extract_block(lines, 0)
    # read_block includes starting '{' line; convert to stripped lines
    # Adjust to original file offsets to find where to insert write.
    # We'll use regex to find Write method and replace it; otherwise append before class end.

    # generate write lines
    write_body_lines = generate_write_from_read(read_block)

    # build write method text
    write_method = []
    write_method.append("        public override byte[] Write()\n        {")
    # many Commands start by writing the Id short
    # try to find "public new const short Id = <num>;" in file
    m_id = re.search(r'public\s+new\s+const\s+short\s+Id\s*=\s*([-\d]+)\s*;', text)
    if m_id:
        idnum = m_id.group(1)
        write_method.append(f"            this.@byte.WriteShort(Id);")
    else:
        # fallback: don't write Id
        write_method.append("            // TODO: add this.@byte.WriteShort(Id); — Id not found automatically")

    # append generated lines with indentation
    for l in write_body_lines:
        # indent nested lines correctly
        for sub in l.splitlines():
            write_method.append("            " + sub)

    write_method.append("            return this.@byte.ToArray();")
    write_method.append("        }")
    write_method_text = "\n".join(write_method) + "\n"

    # Replace existing Write() if present and contains NotImplementedException, else insert before class end
    if RE_WRITE_METHOD.search(text):
        # find the Write method start and its block and replace it
        # crude: find 'public override byte[] Write()' and then find matching braces and replace
        wstart = text.find('public override byte[] Write()')
        wbrace_idx = text.find('{', wstart)
        if wbrace_idx < 0:
            return False, "malformed Write"
        # count braces from wbrace_idx
        rest = text[wbrace_idx:]
        block, endoff = extract_block(rest.splitlines(), 0)
        # compute end index in characters: sum len of lines
        char_count = sum(len(l) + 1 for l in block)  # +1 for newline
        new_text = text[:wstart] + write_method_text + text[wstart + (wbrace_idx - wstart) + char_count:]
        # easier: use regex to replace method body between Write() { ... }
        new_text = re.sub(r'public\s+override\s+byte\[\]\s+Write\s*\(\s*\)\s*\{[\s\S]*?\}', write_method_text, text, count=1)
    else:
        # insert before final closing brace of class file
        # find last occurrence of '}\n}' ? just put before final '}\n' file end
        # safer: insert before last occurrence of '\n}\n' (class close)
        last_brace = text.rfind('\n}')
        if last_brace == -1:
            return False, "could not find class end to insert Write()"
        new_text = text[:last_brace] + "\n" + write_method_text + text[last_brace:]

    # create backup
    bak = path.with_suffix(path.suffix + '.bak')
    path.rename(bak)
    # write new file
    path.write_text(new_text, encoding='utf-8')
    return True, f"updated (backup: {bak.name})"

def walk_and_process(dirpath: Path):
    results = []
    for p in dirpath.rglob("*.cs"):
        ok, msg = process_file(p)
        if ok:
            results.append((str(p), "updated"))
        else:
            results.append((str(p), "skipped: " + msg))
    return results

def main():
    if len(sys.argv) < 2:
        print("Usage: python3 packet_updater.py <path-to-Commands-folder>")
        sys.exit(1)
    target = Path(sys.argv[1])
    if not target.exists() or not target.is_dir():
        print("Pfad existiert nicht oder ist kein Verzeichnis:", target)
        sys.exit(1)
    print("Scanning:", target)
    res = walk_and_process(target)
    for f, status in res:
        print(f, "=>", status)
    print("Fertig. Prüfe die .bak-Dateien falls etwas schief geht.")
    print("Hinweis: Das Tool ist heuristisch. Bitte die generierten Write()-Methoden manuell prüfen.")

if __name__ == '__main__':
    main()