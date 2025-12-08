#!/usr/bin/env python3
"""
Copy all Day starter READMEs into docs/days/ for MkDocs processing.
Also copy CHANGELOG.md to docs/ for inclusion in the site.
"""
import os
import shutil
from pathlib import Path

# Create docs/days directory
docs_days = Path("docs/days")
docs_days.mkdir(parents=True, exist_ok=True)

# Copy CHANGELOG.md to docs/
changelog_src = Path("CHANGELOG.md")
changelog_dest = Path("docs/CHANGELOG.md")
if changelog_src.exists():
    shutil.copy2(changelog_src, changelog_dest)
    print(f"✓ Copied: {changelog_src} → {changelog_dest}")
else:
    print(f"✗ Missing: {changelog_src}")

# Copy all Day starter READMEs
days_dir = Path("days")
count = 0

for day_folder in sorted(days_dir.glob("Day*")):
    if not day_folder.is_dir():
        continue

    day_name = day_folder.name
    # Extract just the day number (e.g., "Day01" from "Day01-Setup-And-Tooling")
    day_num = day_name.split("-")[0]
    starter_readme = day_folder / f"{day_num}-Starter" / "README.md"

    if starter_readme.exists():
        dest = docs_days / f"{day_name}.md"
        shutil.copy2(starter_readme, dest)
        print(f"✓ Copied: {starter_readme} → {dest}")
        count += 1
    else:
        print(f"✗ Missing: {starter_readme}")

print(f"\n✓ Copied {count} day starter READMEs to docs/days/")

# List files in docs/days
print("\nFiles in docs/days/:")
for f in sorted(docs_days.glob("*.md")):
    print(f"  - {f.name}")
