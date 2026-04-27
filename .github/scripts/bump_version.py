#!/usr/bin/env python3
from __future__ import annotations

import re
import sys
import xml.etree.ElementTree as Et
from pathlib import Path
from typing import Final, Literal, Never
import json

# Resolve paths from the repository root: .github/scripts -> repo root is three levels up.
REPO_ROOT: Final[Path] = Path(__file__).parent.parent.parent
FILE: Final[Path] = REPO_ROOT / "src" / "Directory.Build.props"
VERSION_PATTERN: Final[re.Pattern[str]] = re.compile(r"^\d+\.\d+\.\d+(-preview\.\d+)?$")
BumpPart = Literal["major", "minor", "patch", "preview"]

def fail(message: str) -> Never:
    print(message)
    raise SystemExit(1)


def validate_version(version: str) -> bool:
    """
    Validate version format: major.minor.patch or major.minor.patch-preview.number.
    """
    return VERSION_PATTERN.match(version) is not None


def bump(version: str, part: BumpPart) -> str:
    """
    Bump version according to 'major', 'minor', 'patch', or 'preview'.
    Expects a format like: 0.1.0-preview.88
    """
    core: str
    preview: str | None
    core, preview = version, None
    if "-preview." in version:
        core, preview = version.split("-preview.")
    had_preview = preview is not None

    major, minor, patch = map(int, core.split("."))

    if part == "major":
        major += 1
        minor = 0
        patch = 0
        preview = "0" if had_preview else None
    elif part == "minor":
        minor += 1
        patch = 0
        preview = "0" if had_preview else None
    elif part == "patch":
        patch += 1
        preview = "0" if had_preview else None
    elif part == "preview":
        if preview is None:
            preview = "1"
        else:
            preview = str(int(preview) + 1)
    else:
        raise ValueError(f"Unknown bump part: {part}")

    new_version = f"{major}.{minor}.{patch}"
    if preview is not None:
        new_version += f"-preview.{preview}"
    return new_version

def main() -> int:
    if len(sys.argv) < 2:
        fail("Usage: bump_version.py [major|minor|patch|preview|custom] [custom_version]")

    part = sys.argv[1].lower()

    if not FILE.exists():
        fail(f"Error: File not found: {FILE}")

    tree = Et.parse(FILE)
    root = tree.getroot()

    version_elem = root.find(".//Version")
    if version_elem is None or not version_elem.text:
        fail("Error: <Version> not found in XML.")

    old_version = version_elem.text.strip()

    if part == "custom":
        if len(sys.argv) < 3:
            fail("Error: custom version must be provided")

        new_version = sys.argv[2]
        if not validate_version(new_version):
            fail(
                f"Error: Invalid version format '{new_version}'. "
                "Expected format: X.Y.Z or X.Y.Z-preview.N"
            )
    else:
        if part not in ("major", "minor", "patch", "preview"):
            fail(f"Error: Unknown bump part '{part}'")
        new_version = bump(old_version, part)

    version_elem.text = new_version
    tree.write(FILE, encoding="utf-8", xml_declaration=True)

    print(f"Bumped version: {old_version} -> {new_version}")
    print(new_version)  # Output for GitHub Actions to capture
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
