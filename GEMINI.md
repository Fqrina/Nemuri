# Nemuri - Project Instructions

## Overview
Nemuri is a 2D RPG inspired by **OMORI**. It focuses on a rich narrative, turn-based combat with emotional mechanics, and surreal puzzles.

## Core Pillars
- **Dialogue System:** Branching dialogue, portrait support, and text effects similar to OMORI's expressive text.
- **Mechanics:** Emotion-based combat (Neutral, Happy, Sad, Angry), exploration-based interaction, and a "Follower" system for party movement.
- **Puzzles:** Environmental puzzles that often leverage the surreal nature of the world or character-specific abilities.

## Technical Standards
- **Unity Version:** 6000.4.0f1
- **Render Pipeline:** Universal Render Pipeline (URP) - 2D
- **Input System:** New Input System (`InputSystem_Actions.inputactions`)
- **Follower System:** A 4-player party system where characters follow each other in a snake-like queue (OMORI style).
- **Coding Style:** 
  - PascalCase for classes and public methods.
  - camelCase for private fields (with `_` prefix).
  - Use `[SerializeField]` for inspector visibility.
  - Adhere to SOLID principles.

## Folder Structure
- `Assets/Scripts/`: All C# scripts.
- `Assets/Sprites/`: All 2D visual assets.
- `Assets/Prefabs/`: Reusable GameObjects.
- `Assets/Dialogue/`: Dialogue data files (e.g., JSON or ScriptableObjects).
- `Assets/Scenes/`: Game levels.

## Key Workflows
- When creating dialogue, ensure compatibility with the upcoming Dialogue System.
- Puzzles should be designed as modular components where possible.
