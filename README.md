# Basic Platformer

A small 2D platformer built in Unity 6 LTS (Universal 2D template).

**Author:** Brando Banco
**Course:** CPSC-386, Project 1
**Repository:** https://github.com/bbanco-h/BasicPlatformer

## Overview

### Controls

| Action | Keys |
|---|---|
| Move | A / D or arrow keys |
| Jump | Space |
| Fire | K or Left Ctrl |
| Pause | Escape |

### Scoring

- 10 points per ember
- 25 points per enemy
- 100 point goal bonus

The best score is saved between sessions.

### Running the project

1. Open the project folder with Unity Hub using Unity 6 LTS.
2. Open `Assets/Scenes/MainMenu` and press Play. Both scenes are already in Build Settings.

## Script Summary

| Script | What it does |
|---|---|
| `PlayerController` | Reads the Move and Jump actions from the Input Actions asset and moves the player's Rigidbody2D in FixedUpdate. Uses an OverlapCircle ground check on the Ground layer, flips the player to face the move direction, and exposes `FacingDirection`. |
| `PlayerShooter` | Reads the Fire action and takes a bullet from the pool, placing it at the fire point and launching it in the facing direction. A cooldown limits fire rate. |
| `PlayerInteraction` | `OnTriggerEnter2D` with `CompareTag`: Ember (collect, add score), Hazard or Enemy (end game as a loss), Goal (bonus, end game as a win). |
| `ObjectPool` | Queue-based pool. Pre-instantiates inactive copies of a prefab, hands them out with `Get()`, and recycles them with `Return()`. Bullets are never destroyed. |
| `Bullet` | Pooled projectile. Flies horizontally, kills enemies on trigger (tag Enemy), and returns to the pool on hitting Ground or after its lifetime expires. |
| `Enemy` | Patrols left and right around its start position using a kinematic Rigidbody2D. `Die()` deactivates it and raises a static `Killed` event carrying its point value. |
| `GameManager` | Tracks the score, updates the HUD, subscribes to `Enemy.Killed`, and handles `EndGame` (win or lose): saves the high score, shows the end panel, and freezes time. |
| `HighScoreStorage` | Small static wrapper around PlayerPrefs (key `"HighScore"`). Saves only when the score beats the stored best. |
| `SceneLoader` | Button methods: `LoadGame`, `LoadMenu`, `ReloadCurrent`, and `QuitGame`. Resets `Time.timeScale` to 1 before every load. |
| `PauseMenu` | Toggles the pause panel with the Pause action. Sets `Time.timeScale` to 0 or 1 and ignores input once the game has ended. |
| `MainMenuDisplay` | Shows the saved high score on the Main Menu. |
| `CameraFollow` | Smoothly follows the player in `LateUpdate` with a fixed offset. |

## Scenes

### MainMenu

Shows the title, the saved high score, and Start and Quit buttons.

- **Main Camera:** renders the menu.
- **EventSystem:** uses the Input System UI Input Module so buttons work with the new Input System.
- **Canvas:** Title, HighScoreText, StartButton, QuitButton.
- **MenuManager:** holds `SceneLoader` and `MainMenuDisplay`. StartButton calls `SceneLoader.LoadGame`, QuitButton calls `SceneLoader.QuitGame`, and `MainMenuDisplay` reads `HighScoreStorage` on Start.

### Game

The playable level.

- **Main Camera:** has `CameraFollow` targeting the Player.
- **GlobalLight:** a dim Global Light 2D that sets the dark mood.
- **Grid > Ground:** Tilemap (floor, two pits, blocks, platforms, boundary walls) with Tilemap Collider 2D, Composite Collider 2D and a Static Rigidbody2D. Layer and tag are both Ground.
- **Player:** Rigidbody2D, Box Collider 2D with a zero-friction Physics Material 2D, `PlayerController`, `PlayerShooter`, `PlayerInteraction`. Children: GroundCheck, FirePoint, and a warm Point Light 2D.
- **BulletPool:** `ObjectPool` holding 10 Bullet prefab instances.
- **Embers:** collectible prefab instances (trigger collider, tag Ember, layer Pickup, Point Light 2D, unlit glow material).
- **Enemies:** patrolling Enemy prefab instances (tag Enemy, trigger collider).
- **Hazards:** Spike prefab instances (tag Hazard).
- **KillZone:** a wide trigger below the level (tag Hazard) that ends the game if the player falls.
- **Goal:** trigger (tag Goal) with a green light at the end of the level.
- **Canvas:** ScoreText (HUD), PausePanel (Resume, Quit to Menu, Quit), EndPanel (title, score, Retry, Quit to Menu).
- **UIController:** holds `PauseMenu` and `SceneLoader`.
- **GameManager:** holds `GameManager` and is referenced by `PlayerInteraction` and `PauseMenu`.

**How they interact:** the player moves through input actions and physics. `PlayerInteraction` reacts to trigger overlaps and reports to `GameManager`. `PlayerShooter` pulls bullets from `BulletPool`, and bullets kill enemies, which raise `Enemy.Killed` so `GameManager` adds score. `GameManager` ends the run, saves the high score, and shows the end panel. `PauseMenu` freezes time and checks `GameManager.IsPlaying`.

## Requirements

| # | Requirement | How it is satisfied |
|---|---|---|
| 1 | Scenes and scene management | MainMenu and Game are both in Build Settings. Start loads Game via `SceneLoader`. Quit to Menu on the pause and end panels returns to MainMenu. |
| 2 | Input System player control | `PlayerControls.inputactions` is read through `InputActionReference`. The player moves with Rigidbody2D and Collider2D physics. |
| 3 | Tilemap level layout | The Ground Tilemap forms the whole level and uses Tilemap Collider 2D. |
| 4 | Prefab pooling | `ObjectPool` recycles 10 Bullet prefab instances with `SetActive` and a queue. Bullets are never destroyed during play. |
| 5 | Tags, layers, triggers | `OnTriggerEnter2D` with `CompareTag` for Ember, Hazard, Enemy and Goal. The Layer Collision Matrix stops Projectile from colliding with Player and Pickup. |
| 6 | Pause menu | Escape opens a Canvas panel, sets `Time.timeScale` to 0, and offers Resume, Quit to Menu and Quit. Fire and jump input is ignored while paused. |
| 7 | PlayerPrefs persistence | The best score is saved under the key `"HighScore"` and shown on the Main Menu after relaunching the build. |
| 8 | Lighting or material | Global Light 2D plus Point Light 2D on the player, embers and goal, and a custom unlit `EmberGlow` material on embers, bullets and the goal. |

## Third-Party Assets, Packages and Tutorials

- **Unity packages (from the Unity registry):** Universal Render Pipeline with the 2D Renderer (Light2D), Input System, 2D Tilemap, and TextMeshPro (uGUI).
- **Course scripts:** used the example scripts provided in Canvas from past lectures and assignments as a reference and starting point when making the game scripts.
- **AI assistance:** AI was used to help make the scripts when I was unable to figure out how to update the provided scripts to match my function needs. I also used it for guidance on implementing and debugging certain actions that caused trouble, and to make this outline and help explain how everything interacts and works.
