# fetchpkg GUI

A lightweight, multi-lingual graphical user interface (GUI) for the official `fetchpkg` command-line tool, built natively for Windows using VB.NET (WinForms). 

This interface simplifies the download process of Sony PlayStation 4 and PlayStation 5 game updates from official servers by wrapping the core tool features into a clean, standalone, and single-file application.

## Features

- **Standalone Single File:** Compiles into a single independent `.exe` binary with zero messy DLL folder clutter.
- **Smart Link Conversion:** Automatically detects and converts PlayStation update links (`*_sc.pkg` or `*.crc`) from ProsperoPatches or PSN straight into a valid `*.json` manifest URL when starting a download.
- **Dynamic Theme Switcher:** Easily toggle between Dark Mode and Light Mode instantly using the built-in interface toggle switch (`☀️` / `🌙`).
- **Dual-Language Support:** On-the-fly interface translation switching between French and English.
- **Real-Time Feed:** Extracts the exact decimal download percentage and provides an accurate Time Remaining (ETA) countdown ticker.
- **Isolated Error Logs:** Keeps the main screen compact and minimal. A clean, independent terminal log window triggers and pops up automatically *only* if `fetchpkg.exe` encounters an error or fails.

## Prerequisites

To run this graphical interface, you must ensure that:
1. Download fetchpkg by ps5-payload-dev from : https://github.com/ps5-payload-dev/fetchpkg (Win-x86_64.zip)
2. The official **`fetchpkg.exe`** binary is placed inside the exact same folder as this GUI.
3. The mandatory **`libcurl-x64.dll`** dependency is present alongside it.

## How to Compile

You can compile this program natively on any Windows machine using the built-in .NET Framework compiler via PowerShell. Open a terminal in your source directory and run:

```powershell
& "\$env:SystemRoot\Microsoft.NET\Framework64\v4.0.30319\vbc.exe" /target:winexe /win32icon:fetch.ico /out:fetchpkg_GUI.exe gui.vb
```

## 📝 License

This interface is designed to support the development community and is shared under the same spirit as the core open-source projects it interacts with.
