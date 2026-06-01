# WheelPicker

A lightweight Windows desktop application built with **C#**, **WPF**, and **.NET**, designed to help users make random selections using an animated Wheel of Fortune style interface.

WheelPicker was originally created to solve a common problem: deciding what to listen to, play, watch, or do when presented with too many choices.

The application supports multiple libraries loaded from JSON files, allowing the wheel to be used for:

* Music genres and subgenres
* Video games
* Movies
* TV shows
* Anime
* Activities
* Tasks
* Custom user-defined libraries

---

# Features

## Animated Wheel

* Smooth spinning animation
* Randomized spin duration and stopping position
* Visual pointer indicating the selected result
* Protection against multiple simultaneous spins

## Library System

Libraries are stored as JSON files and loaded dynamically.

Examples:

* Music
* Games
* Activities
* Movies
* Anime

New libraries can be added without modifying application code.

---

## Hierarchical Genre → Subgenre Selection

Music libraries support two-stage selection:

### First Spin

Selects a genre.

Example:

Metal

### Second Spin

Automatically switches the wheel to that genre's subgenres.

Example:

Power Metal

Final result:

Metal > Power Metal

The wheel then resets back to the genre selection screen.

---

## Sound Effects

* Wheel spin sound effect support
* Easy replacement with custom WAV files
* Future support planned for tick-based wheel sounds

---

## Simple Deployment

The application can be published as a standalone Windows executable.

No separate .NET installation is required when using self-contained deployment.

---

# Screenshots

Current layout:

* Wheel centered on screen
* Fixed top pointer
* Library selector
* Spin button
* Result display
* Optional Back button during subgenre selection

---

# Project Structure

```text
WheelPicker/
│
├── Assets/
│   └── spin.wav
│
├── Libraries/
│   ├── music.json
│   ├── games.json
│   └── ...
│
├── MainWindow.xaml
├── MainWindow.xaml.cs
│
├── WheelDrawer.cs
├── LibraryLoader.cs
├── MusicLibraryLoader.cs
│
├── ItemLibrary.cs
├── MusicLibrary.cs
│
└── WheelPicker.csproj
```

# Requirements

* Windows 10 or later
* .NET 9 SDK (development)
* Visual Studio Code or Visual Studio

---

# Building

Clone the repository:

```bash
git clone <repository-url>
cd WheelPicker
```

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run
```

---

# Publishing

Create a self-contained Windows executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

Output:

```text
bin/
└── Release/
    └── net9.0-windows/
        └── win-x64/
            └── publish/
```

The executable can be launched directly:

```text
WheelPicker.exe
```

---

# Single File Deployment

To generate a single executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Result:

```text
WheelPicker.exe
```

---

# Creating Libraries

## Simple Library

```json
{
  "name": "Games",
  "items": [
    "Ghost of Tsushima",
    "Cyberpunk 2077",
    "Dota 2",
    "Rainbow Six Siege",
    "Assassin's Creed Origins",
    "Grand Theft Auto V",
    "Dark Souls"
  ]
}
```

---

## Hierarchical Music Library

```json
{
  "name": "Music",
  "genres": [
    {
      "name": "Metal",
      "subgenres": [
        "Heavy Metal",
        "Power Metal",
        "Thrash Metal"
      ]
    },
    {
      "name": "Electronic",
      "subgenres": [
        "Synthwave",
        "Ambient",
        "Techno"
      ]
    }
  ]
}
```

---

# Roadmap

Planned features:

* Tick-based wheel sound effects
* Dark mode
* Custom themes
* Persistent settings
* Remember last selected library
* Weighted probabilities
* Editable libraries from within the application
* Search functionality
* Result history
* Statistics tracking
* Export and import libraries
* Multi-level category trees beyond Genre → Subgenre

---

# Motivation

WheelPicker was created as a fast, lightweight decision-making tool for situations where too many choices lead to decision fatigue.

Whether choosing a music genre, a game to play, a movie to watch, or simply deciding what to do next, WheelPicker provides a simple and enjoyable way to let randomness decide.

---

# License

MIT License

Feel free to modify, distribute, and extend the project.
