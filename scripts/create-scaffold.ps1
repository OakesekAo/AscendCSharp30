<#
Creates Day01..Day30 folders with DayNN-Starter and DayNN-Complete using titles from README.
Run:
  powershell -ExecutionPolicy Bypass -File .\scripts\create-scaffold.ps1
Options:
  -CreateWeeklySolutions:$true  create Week01..Week04.sln grouping DayNN-Complete projects
#>

param(
    [switch]$CreateWeeklySolutions = $true,
    [string]$RootPath = "."
)

$netTarget = "net10.0"

# Titles derived from README.md
$titles = @(
    "Setup-And-Tooling",
    "Types-And-Variables",
    "Control-Flow",
    "Collections",
    "Methods-And-Parameters",
    "LINQ-Fundamentals",
    "Mini-Console-Project",
    "Classes-And-Objects",
    "Interfaces-And-Abstraction",
    "Inheritance-And-Polymorphism",
    "Error-Handling",
    "Async-And-Await",
    "IO-And-JSON",
    "Service-Simulation-Project",
    "Dependency-Injection",
    "Options-Pattern-And-Config",
    "Logging",
    "HttpClient",
    "Clean-Architecture",
    "Advanced-LINQ",
    "Mini-API-Project",
    "EF-Core-Basics",
    "EF-Core-Advanced",
    "Unit-Testing",
    "Blazor-Components",
    "API-Integration",
    "Authentication",
    "Deployment",
    "Refactoring-And-Performance",
    "Capstone"
)

function Pad($n){ return $n.ToString("D2") }

$starterReadmeTemplate = @'
# Day{DAY}-Starter — {TITLE}

Objectives
- Follow the daily learning objectives and complete the starter tasks.

Starter steps
1. Read the objectives.
2. Open this folder in Visual Studio (__Open Folder__ recommended) or open the provided project.
3. Implement the exercises in this folder following the TODOs.

Boilerplate
- This folder contains minimal code and instructions to get started.
- Do not commit build artifacts; keep only source and instructions.
'@

$completeReadmeTemplate = @'
# Day{DAY}-Complete — {TITLE}

This is the runnable, completed state for Day{DAY}.

To run:
1. cd Day{DAY}-Complete
2. dotnet restore
3. dotnet run

Notes
- This project targets .NET 10.
- Replace or expand the project with further exercises as desired.
'@

$csprojTemplate = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>' + $netTarget + '</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
  </PropertyGroup>
</Project>
'@

$programCsTemplate = @'
using System;

Console.WriteLine("Day{DAY} — Complete project template. Replace with day-specific code.");
'@

for ($i = 0; $i -lt $titles.Length; $i++) {
    $dayNum = Pad ($i + 1)
    $title = $titles[$i]
    $folderName = "Day$dayNum-$title"
    $base = Join-Path $RootPath $folderName
    $starter = Join-Path $base "Day$dayNum-Starter"
    $complete = Join-Path $base "Day$dayNum-Complete"

    New-Item -ItemType Directory -Path $starter -Force | Out-Null
    New-Item -ItemType Directory -Path $complete -Force | Out-Null

    ($starterReadmeTemplate -replace "{DAY}", $dayNum -replace "{TITLE}", $title) | Out-File -FilePath (Join-Path $starter "README.md") -Encoding utf8
    ($completeReadmeTemplate -replace "{DAY}", $dayNum -replace "{TITLE}", $title) | Out-File -FilePath (Join-Path $complete "README.md") -Encoding utf8

    $projPath = Join-Path $complete "Day$dayNum.Complete.csproj"
    $programPath = Join-Path $complete "Program.cs"

    $csprojTemplate | Out-File -FilePath $projPath -Encoding utf8
    ($programCsTemplate -replace "{DAY}", $dayNum) | Out-File -FilePath $programPath -Encoding utf8
}

if ($CreateWeeklySolutions) {
    for ($week = 1; $week -le 4; $week++) {
        dotnet new sln -n ("Week" + $week.ToString("D2")) | Out-Null

        $start = (($week - 1) * 7)
        $end = [Math]::Min($start + 6, $titles.Length - 1)
        for ($d = $start; $d -le $end; $d++) {
            $dayNum = Pad ($d + 1)
            $title = $titles[$d]
            $proj = Join-Path (Join-Path (Get-Location) ("Day$dayNum-$title")) ("Day$dayNum-Complete/Day$dayNum.Complete.csproj")
            if (Test-Path $proj) {
                dotnet sln ("Week" + $week.ToString("D2" ) + ".sln") add $proj | Out-Null
            }
        }
    }
}

Write-Host "Scaffold created. Run dotnet restore in created DayNN-Complete folders to restore."
