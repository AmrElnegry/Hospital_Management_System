# Hospital Management System

This repository contains our Hospital Management System project files, source code, pseudocode, documentation, and presentation materials.

## Repository Structure

- `00 - About Project`: project specifications and instructions
- `01 - Pseudocode`: pseudocode and class diagram files
- `02 - Coding/01 - Full Code`: console-based project logic
- `03 - GUI/01 - Full GUI`: Windows Forms GUI project
- `04 - Presentation`: presentation and proposal files
- `05 - Final Program`: final packaged program files

## Main Source Projects

### Console Version

- Project file: `02 - Coding/01 - Full Code/HospitalManagementOldLogic.csproj`
- Framework: `.NET 8.0`

### GUI Version

- Project file: `03 - GUI/01 - Full GUI/Project_Hospital.csproj`
- Framework: `.NET 8.0 Windows Forms`

## Requirements

- Visual Studio 2022 or later
- .NET 8 SDK

## Build Commands

### Build console project

```powershell
dotnet build ".\02 - Coding\01 - Full Code\HospitalManagementOldLogic.csproj"
```

### Build GUI project

```powershell
dotnet build ".\03 - GUI\01 - Full GUI\Project_Hospital.csproj"
```

## Run Commands

### Run console project

```powershell
dotnet run --project ".\02 - Coding\01 - Full Code\HospitalManagementOldLogic.csproj"
```

### Run GUI project

```powershell
dotnet run --project ".\03 - GUI\01 - Full GUI\Project_Hospital.csproj"
```

## GitHub Notes

- Existing project files were kept as-is.
- Generated folders and machine-specific files are excluded through `.gitignore`.
- Packaged outputs and runtime data files remain in the folder, but Git will ignore them by default.
