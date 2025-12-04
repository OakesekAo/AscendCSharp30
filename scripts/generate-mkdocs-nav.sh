#!/bin/bash
# Generate mkdocs nav from days folder

echo "  - Days:" > mkdocs_nav_days.yml

for d in days/Day*/; do
  day=$(basename "$d")
  # Extract day number and title
  # Day01-Setup-And-Tooling -> 01 and Setup-And-Tooling
  num=$(echo "$day" | cut -d- -f1 | sed 's/Day//')
  title=$(echo "$day" | cut -d- -f2-)
  # Convert hyphens to spaces and title case each word
  title_readable=$(echo "$title" | sed 's/-/ \& /g' | sed 's/\&/\&/g')
  
  # Use the actual markdown file name
  echo "    - Day $num — $title_readable: days/${day}.md" >> mkdocs_nav_days.yml
done

echo "Generated mkdocs_nav_days.yml"
cat mkdocs_nav_days.yml
