#!/usr/bin/env python3
"""
Copy all Day starter READMEs into docs/days/ for MkDocs processing.
"""
import os
import shutil
from pathlib import Path

docs_days = Path("docs/days")
docs_days.mkdir(parents=True, exist_ok=True)

days_dir = Path("days")
count = 0

for day_folder in sorted(days_dir.glob("Day*")):
    if not day_folder.is_dir():
        continue
    
    day_name = day_folder.name
    starter_readme = day_folder / f"{day_name}-Starter" / "README.md"
    
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
