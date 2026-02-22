#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Packet Updater - GUI-Frontend
Einfaches Tkinter-GUI zum Starten des do_auto_packets.py (oder eines ähnlichen Tools),
um das Root-Verzeichnis, die Mapping-Datei und Optionen (dry-run/backup/no-apply) per GUI zu setzen.

Benutzung:
- Platziere dieses Script irgendwo (z. B. scripts/tools/packet_updater_gui.py).
- Stelle sicher, dass do_auto_packets.py (oder dein updater script) erreichbar ist.
- Starte: python packet_updater_gui.py

Hinweis:
Dieses GUI ruft das angegebene Tool per subprocess auf (sys.executable).
Standardmäßig schlägt es als Tool-Pfad "do_auto_packets.py" im selben Verzeichnis vor.
"""
from __future__ import annotations
import os
import sys
import threading
import subprocess
import queue
import shlex
import tkinter as tk
from tkinter import ttk, filedialog, messagebox

# ---------------------------
# Hilfsfunktionen
# ---------------------------
def resource_path(fname: str) -> str:
    """Hilfsfunktion: falls Script gebündelt wird, passt Pfad an; sonst fname."""
    return os.path.abspath(fname)

def run_subprocess(cmd: list[str], out_queue: "queue.Queue[str]"):
    """
    Führt subprocess aus, schreibt stdout/stderr Zeilen in out_queue.
    Läuft in einem Hintergrund-Thread.
    """
    try:
        # merged stdout/stderr for simpler handling
        proc = subprocess.Popen(
            cmd,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            universal_newlines=True,
            bufsize=1
        )
    except Exception as e:
        out_queue.put(f"FEHLER: Prozess konnte nicht gestartet werden: {e}\n")
        return

    if proc.stdout is None:
        out_queue.put("FEHLER: Keine Ausgabe vom Prozess verfügbar.\n")
        return

    try:
        for line in proc.stdout:
            out_queue.put(line)
    except Exception as e:
        out_queue.put(f"FEHLER beim Lesen der Prozessausgabe: {e}\n")
    finally:
        proc.wait()
        out_queue.put(f"\n--- Prozess beendet mit Exit-Code {proc.returncode} ---\n")

# ---------------------------
# GUI-Klasse
# ---------------------------
class PacketUpdaterGUI(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Packet Updater - GUI")
        self.minsize(800, 520)
        self.out_queue: "queue.Queue[str]" = queue.Queue()
        self.process_thread: threading.Thread | None = None
        self.poll_job = None

        # Default paths
        default_cwd = os.getcwd()
        default_tool = os.path.join(default_cwd, "do_auto_packets.py")
        default_mapping = os.path.join(default_cwd, "packets_mapping.json")

        # UI Layout
        frm = ttk.Frame(self, padding=12)
        frm.pack(fill=tk.BOTH, expand=True)

        # Tool script path
        lbl_tool = ttk.Label(frm, text="Updater-Script (do_auto_packets.py):")
        lbl_tool.grid(row=0, column=0, sticky=tk.W)
        self.var_tool = tk.StringVar(value=default_tool)
        ent_tool = ttk.Entry(frm, textvariable=self.var_tool, width=80)
        ent_tool.grid(row=1, column=0, columnspan=3, sticky=tk.W+tk.E, pady=(0,6))
        btn_tool = ttk.Button(frm, text="Browse...", command=self.browse_tool)
        btn_tool.grid(row=1, column=3, sticky=tk.E, padx=(6,0))

        # Root folder
        lbl_root = ttk.Label(frm, text="Projekt-Root (z. B. Darkside-Public):")
        lbl_root.grid(row=2, column=0, sticky=tk.W)
        self.var_root = tk.StringVar(value=default_cwd)
        ent_root = ttk.Entry(frm, textvariable=self.var_root, width=80)
        ent_root.grid(row=3, column=0, columnspan=3, sticky=tk.W+tk.E, pady=(0,6))
        btn_root = ttk.Button(frm, text="Browse...", command=self.browse_root)
        btn_root.grid(row=3, column=3, sticky=tk.E, padx=(6,0))

        # Mapping file
        lbl_map = ttk.Label(frm, text="Mapping-Datei (packets_mapping.json):")
        lbl_map.grid(row=4, column=0, sticky=tk.W)
        self.var_map = tk.StringVar(value=default_mapping)
        ent_map = ttk.Entry(frm, textvariable=self.var_map, width=80)
        ent_map.grid(row=5, column=0, columnspan=3, sticky=tk.W+tk.E, pady=(0,6))
        btn_map = ttk.Button(frm, text="Browse/Save...", command=self.browse_map)
        btn_map.grid(row=5, column=3, sticky=tk.E, padx=(6,0))

        # Options: dry-run, backup, no-apply
        self.var_dry = tk.BooleanVar(value=True)
        self.var_backup = tk.BooleanVar(value=True)
        self.var_no_apply = tk.BooleanVar(value=False)
        chk_dry = ttk.Checkbutton(frm, text="Dry-run (zeige Änderungen, schreibe nichts)", variable=self.var_dry)
        chk_backup = ttk.Checkbutton(frm, text="Backup (.bak) erstellen", variable=self.var_backup)
        chk_noapply = ttk.Checkbutton(frm, text="Nur Scan (no-apply)", variable=self.var_no_apply)
        chk_dry.grid(row=6, column=0, columnspan=2, sticky=tk.W, pady=(6,0))
        chk_backup.grid(row=6, column=2, sticky=tk.W, pady=(6,0))
        chk_noapply.grid(row=6, column=3, sticky=tk.W, pady=(6,0))

        # Buttons
        btn_frame = ttk.Frame(frm)
        btn_frame.grid(row=7, column=0, columnspan=4, sticky=tk.W+tk.E, pady=(8,8))
        btn_scan = ttk.Button(btn_frame, text="Scan & Mapping erzeugen", command=self.on_scan)
        btn_apply = ttk.Button(btn_frame, text="Scan + Apply (Backup & Write)", command=self.on_apply)
        btn_stop = ttk.Button(btn_frame, text="Stop", command=self.on_stop)
        btn_scan.pack(side=tk.LEFT, padx=(0,6))
        btn_apply.pack(side=tk.LEFT, padx=(0,6))
        btn_stop.pack(side=tk.LEFT, padx=(0,6))

        # Output area
        out_lbl = ttk.Label(frm, text="Output")
        out_lbl.grid(row=8, column=0, sticky=tk.W)
        self.txt_out = tk.Text(frm, wrap=tk.NONE, height=18)
        self.txt_out.grid(row=9, column=0, columnspan=4, sticky=tk.N+tk.S+tk.E+tk.W)
        vsb = ttk.Scrollbar(frm, orient=tk.VERTICAL, command=self.txt_out.yview)
        vsb.grid(row=9, column=4, sticky=tk.N+tk.S)
        self.txt_out['yscrollcommand'] = vsb.set

        # configure grid resizing
        frm.rowconfigure(9, weight=1)
        frm.columnconfigure(0, weight=1)
        frm.columnconfigure(1, weight=0)
        frm.columnconfigure(2, weight=0)
        frm.columnconfigure(3, weight=0)

        # Bind closing event
        self.protocol("WM_DELETE_WINDOW", self.on_close)

    # ---------------------------
    # Browse callbacks
    # ---------------------------
    def browse_tool(self):
        initial = self.var_tool.get()
        p = filedialog.askopenfilename(
            title="Updater-Script auswählen (do_auto_packets.py)",
            initialdir=os.path.dirname(initial) if os.path.exists(initial) else os.getcwd(),
            filetypes=[("Python files","*.py"),("All files","*.*")]
        )
        if p:
            self.var_tool.set(p)

    def browse_root(self):
        initial = self.var_root.get()
        d = filedialog.askdirectory(title="Projekt Root auswählen", initialdir=initial if os.path.isdir(initial) else os.getcwd())
        if d:
            self.var_root.set(d)

    def browse_map(self):
        initial = self.var_map.get()
        p = filedialog.asksaveasfilename(
            title="Mapping Datei wählen / Speichern",
            initialfile=os.path.basename(initial),
            initialdir=os.path.dirname(initial) if os.path.isdir(os.path.dirname(initial)) else os.getcwd(),
            defaultextension=".json",
            filetypes=[("JSON files","*.json"),("All files","*.*")]
        )
        if p:
            self.var_map.set(p)

    # ---------------------------
    # Button handlers
    # ---------------------------
    def on_scan(self):
        """Nur scan (Mapping erzeugen, no-apply)"""
        if self.process_thread and self.process_thread.is_alive():
            messagebox.showinfo("Info", "Ein Prozess läuft bereits. Stoppe ihn zuerst.")
            return
        tool = self.var_tool.get().strip()
        root = self.var_root.get().strip()
        mapping = self.var_map.get().strip()
        if not os.path.isfile(tool):
            messagebox.showerror("Fehler", f"Updater-Script nicht gefunden:\n{tool}")
            return
        cmd = [sys.executable, tool, "--root", root, "--mapping", mapping, "--no-apply"]
        if self.var_dry.get():
            cmd.append("--dry-run")
        if not self.var_backup.get():
            # for scan-only we don't need backup flag; but keep option
            pass
        self.start_process(cmd)

    def on_apply(self):
        """Scan + Apply (Standard: Backup, optional dry-run)"""
        if self.process_thread and self.process_thread.is_alive():
            messagebox.showinfo("Info", "Ein Prozess läuft bereits. Stoppe ihn zuerst.")
            return
        tool = self.var_tool.get().strip()
        root = self.var_root.get().strip()
        mapping = self.var_map.get().strip()
        if not os.path.isfile(tool):
            messagebox.showerror("Fehler", f"Updater-Script nicht gefunden:\n{tool}")
            return
        cmd = [sys.executable, tool, "--root", root, "--mapping", mapping]
        if self.var_dry.get():
            cmd.append("--dry-run")
        if self.var_no_apply.get():
            cmd.append("--no-apply")
        if self.var_backup.get():
            cmd.append("--backup")
        self.start_process(cmd)

    def on_stop(self):
        """Versuche laufenden Prozess abzubrechen (nur best-effort)."""
        # we don't keep Popen object here; the background thread created it.
        # Simpler: notify user to close GUI or rely on script to handle interrupts.
        messagebox.showinfo("Stop", "Stop-Pause ist nicht implementiert. Schließe das Programm, um Prozesse zu beenden, oder beende das aufgerufene Script manuell.")
        # Advanced: we could store Popen and terminate it. Not implemented to keep code simple.

    # ---------------------------
    # Prozess / Thread management
    # ---------------------------
    def start_process(self, cmd: list[str]):
        # show the command in the UI
        self.txt_out.insert(tk.END, "Starte: " + " ".join(shlex.quote(c) for c in cmd) + "\n\n")
        self.txt_out.see(tk.END)
        # create and start thread
        self.out_queue = queue.Queue()
        self.process_thread = threading.Thread(target=run_subprocess, args=(cmd, self.out_queue), daemon=True)
        self.process_thread.start()
        # start polling queue
        self.poll_queue()

    def poll_queue(self):
        """Lese aus out_queue und schreibe in Text-Widget."""
        try:
            while not self.out_queue.empty():
                line = self.out_queue.get_nowait()
                self.txt_out.insert(tk.END, line)
                self.txt_out.see(tk.END)
        except Exception:
            pass
        if self.process_thread and self.process_thread.is_alive():
            self.poll_job = self.after(100, self.poll_queue)
        else:
            # ensure any remaining output is read
            try:
                while not self.out_queue.empty():
                    line = self.out_queue.get_nowait()
                    self.txt_out.insert(tk.END, line)
                    self.txt_out.see(tk.END)
            except Exception:
                pass
            self.process_thread = None
            if self.poll_job:
                self.poll_job = None

    # ---------------------------
    # Window events
    # ---------------------------
    def on_close(self):
        if self.process_thread and self.process_thread.is_alive():
            if not messagebox.askyesno("Beenden", "Ein Prozess läuft noch. Wirklich beenden?"):
                return
        self.destroy()

# ---------------------------
# Main
# ---------------------------
def main():
    app = PacketUpdaterGUI()
    app.mainloop()

if __name__ == "__main__":
    main()