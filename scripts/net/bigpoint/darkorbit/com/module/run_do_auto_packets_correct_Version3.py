#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Wrapper: erkennt, wenn --root ein Commands- oder module-Pfad ist und setzt das richtige Repo-Root,
dann ruft er dein do_auto_packets Script mit korrigierten Parametern auf.

Usage:
  python run_do_auto_packets_correct_Version3.py --tool "C:/path/to/do_auto_packets_Version3.py" --root "C:/.../Commands" [--mapping "..."] [--dry-run] [--no-apply] [--backup]
"""

import argparse
import os
import subprocess
import sys

def find_repo_root_from_path(p: str) -> str:
    p = os.path.abspath(p)
    if os.path.isdir(p):
        parts = p.replace("/", os.sep).split(os.sep)
        if "DarkOrbitProtoLib" in parts and "Commands" in parts:
            idx = parts.index("DarkOrbitProtoLib")
            return os.sep.join(parts[:idx])
        if "scripts" in parts and "net" in parts and "bigpoint" in parts:
            idx = parts.index("scripts")
            return os.sep.join(parts[:idx])
    cur = p
    for _ in range(8):
        if not os.path.isdir(cur):
            cur = os.path.dirname(cur)
            continue
        if os.path.isdir(os.path.join(cur, "DarkOrbitProtoLib")) and os.path.isdir(os.path.join(cur, "scripts")):
            return cur
        cur = os.path.dirname(cur)
    return os.path.abspath(p)

def build_cmd(tool, root, mapping, extra_args):
    cmd = [sys.executable, tool, "--root", root]
    if mapping:
        cmd += ["--mapping", mapping]
    cmd += extra_args
    return cmd

def main():
    p = argparse.ArgumentParser(description="Wrapper für do_auto_packets: korrigiert --root falls Commands/Module übergeben wurde")
    p.add_argument("--tool", required=True, help="Pfad zu deinem do_auto_packets script")
    p.add_argument("--root", required=True, help="Repo-Root oder Commands/module Pfad")
    p.add_argument("--mapping", help="Mapping Pfad (optional)")
    p.add_argument("--dry-run", action="store_true")
    p.add_argument("--no-apply", action="store_true")
    p.add_argument("--backup", action="store_true")
    args, unknown = p.parse_known_args()

    tool = os.path.abspath(args.tool)
    if not os.path.isfile(tool):
        print(f"ERROR: Tool nicht gefunden