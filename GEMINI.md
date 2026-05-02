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
- **Input System:** New Input System. Action Asset: `GameInput.inputactions`.
- **Input Management:** Only ONE `PlayerInput` component should exist in the scene (on the lead character). Other managers (like `DialogueManager`) should reference this singleton to avoid input focus conflicts.
- **Follower System:** A snake-like queue system managed by `PartyManager`. It records position/direction history and updates followers based on a configurable frame/point delay.
- **Coding Style:** 
  - PascalCase for classes and public methods.
  - camelCase for private fields (with `_` prefix).
  - Use `[SerializeField]` for inspector visibility.
  - Adhere to SOLID principles.

## Core Systems
### Player & Movement
- **`PlayerMovement`**: Uses a `static Instance`. Includes `SetCanMove(bool)` to pause movement during cutscenes or dialogue.
- **Animations**: Uses `IsMoving` (bool), `MoveX` (float), and `MoveY` (float) parameters.

### Dialogue
- **`DialogueManager`**: A singleton that handles UI and text typing. It automatically pauses the `PlayerMovement` instance when a conversation starts.

### Party Management
- **`PartyManager`**: Records "Breadcrumbs" from the leader. Followers interpolation towards these breadcrumbs to maintain a consistent trail.

## Folder Structure
- `Assets/Scripts/Core/`: Global managers and utility scripts.
- `Assets/Scripts/Player/`: Player-specific logic.
- `Assets/Scripts/Dialogue/`: Dialogue system scripts.
- `Assets/Sprites/`: All 2D visual assets.
- `Assets/Prefabs/`: Reusable GameObjects.
- `Assets/Dialogue/`: Dialogue data files (JSON).
- `Assets/Scenes/`: Game levels.

## Key Workflows
- **Character Setup**: Ensure 2D Sprites have `Texture Type` set to `Sprite (2D and UI)` and `Pixels Per Unit` is consistent (default 100).
- **Dialogue**: Ensure JSON files match the `DialogueSequence` data structure.
- **Git**: Always commit changes before switching branches to avoid Unity scene synchronization errors.
