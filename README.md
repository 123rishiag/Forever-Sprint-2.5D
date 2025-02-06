# Forever Sprint 2.5D

## Overview

Forever Sprint 2.5D is an action-packed **endless runner** where players **jump, dash, slide, and climb** through dynamically generated levels while collecting 
collectibles. The game integrates various design patterns, including **Service Locator**, **Dependency Injection**, **Model-View-Controller (MVC)**, 
**Observer Pattern**, **Object Pooling**, and **State Machine**, ensuring modularity and scalability. **Scriptable Objects** handle flexible data storage, 
while **Unity's New Input System** provides precise player controls.

---

## Architectural Overview

Below is the block diagram illustrating the **core architecture**:

![Architectural Overview -- Adding Soon](docs/block_diagram.png)

---

## Gameplay Elements

### **1. Controls**
| **Action**         | **Key/Input**                |
|--------------------|------------------------------|
| Jump               | `Spacebar`                   |
| Air Jump           | `Spacebar` (Mid-air)         |
| Slide              | `Left Control`               |
| Dash               | `Left Shift`                 |
| Toggle Mini Camera | `C`                          |
| Pause Menu         | `Escape`                     |

---

### **2. Game States**
| **Game State**   | **Description**                          |
|------------------|------------------------------------------|
| `GAME_START`     | The first state of game.                 |
| `GAME_MENU`      | Displays the main menu.                  |
| `GAME_PLAY`      | Active gameplay state.                   |
| `GAME_PAUSE`     | Displays the pause menu.                 |
| `GAME_RESTART`   | Resets the game for replay.              |
| `GAME_OVER`      | Triggers when the player dies.           |

---

### **3. Player States**
| **Player State**   | **Description**                           |
|--------------------|-------------------------------------------|
| `IDLE`             | The player stands still.                  |
| `MOVE`             | The player is running.                    |
| `JUMP`             | The player performs a normal jump.        |
| `AIR_JUMP`         | Allow jumps mid-air.                      |
| `FALL`             | The player is falling.                    |
| `BIG_FALL`         | The player is falling from a great height.|
| `DEAD_FALL`        | Falling with height great enough to die.  |
| `ROLL`             | The player rolls forward after Big Fall.  |
| `SLIDE`            | The player slides under obstacles.        |
| `DASH`             | The player dashes forward.                |
| `CLIMB`            | The player climbs a vertical surface.     |
| `KNOCK`            | Upon collision while dashing or falling from Dead Fall. |
| `GET_UP`           | The player recovers after being knocked.  |
| `DEAD`             | The player has died.                      |

---

### **4. Collectible Types**
| **Collectible Type** | **Description**            |
|----------------------|----------------------------|
| `CUBE_ONE`           | Basic collectible item.    |
| `CUBE_TWO`           | Special collectible item.  |

---

### **5. Level Types**
| **Level Type**       | **Description**                           |
|----------------------|-------------------------------------------|
| `BACKGROUND`         | Static background elements.               |
| `GROUND_TERRAIN`     | Main ground terrain for movement.         |
| `GROUND_PLATFORM`    | Floating platforms for jumping sections.  |
| `FOREGROUND`         | Static foreground elements.               |

---

## Design Patterns and Programming Principles

### 1. **Service Locator**  
Centralizes access to shared services such as `UIService`, `SoundService`, and `EventService`.

### 2. **Dependency Injection**  
Decouples services for flexibility and maintainability.

### 3. **Model-View-Controller (MVC)**  
Separates concerns for data, visuals, and interactions:
- **Controller**: Coordinates interactions between the model and view.
- **Model**: Handles data and game logic.
- **View**: Manages visuals and rendering.

### 4. **Observer Pattern**  
Enables event-driven communication between game elements, ensuring modular design.

### 5. **Object Pooling**  
Optimizes memory usage for collectibles & levels.

### 6. **State Machine** 
Manages transitions between states like `Game_Start`, `Game_Play`, and `Game_Over`.

### 7. **Scriptable Objects**  
Stores reusable configurations for collectibles, levels and player etc.

---

## Services and Components

1. **GameService**: Manages and fetches the core game components and initializes the `GameController`.
   - **GameController**: Centralized service for managing the game's core mechanics, including game state transitions and controller management.
   - **GameStateMachine**: A specific implementation of `GenericStateMachine` that governs the overall game flow.
     - **GameStartState**: Prepares the game.
     - **GameMenuState**: Displays the main menu.
     - **GamePlayState**: Manages active gameplay.
     - **GamePauseState**: Displays the pause menu.
     - **GameRestartState**: Resets the game for replay.
     - **GameOverState**: Handles game-over logic.

2. **PlayerService**: Manages player movement, animations, and interactions.
   - **PlayerConfig**: Stores properties like speed and health for players.
   - **PlayerController**: Handles **player input and movement logic**.
   - **PlayerModel**: Stores **player attributes** like speed, jump force, and health.
   - **PlayerView**: Manages **animations and player rendering**.
   - **PlayerStateMachine**: A specific implementation of `GenericStateMachine` that manages player state transitions.
     - **PlayerIdleState**: The player remains stationary.
     - **PlayerMoveState**: The player runs forward.
     - **PlayerJumpState**: Handles jumping mechanics.
     - **PlayerAirJumpState**: Manages jumps in mid-air.
     - **PlayerFallState**: Controls normal falling behavior.
     - **PlayerBigFallState**: Triggers after falling from a high altitude.
     - **PlayerDeadFallState**: Results in death when falling from a fatal height.
     - **PlayerRollState**: A recovery move performed after a big fall.
     - **PlayerSlideState**: Enables the player to slide under obstacles.
     - **PlayerDashState**: Grants a burst of forward momentum.
     - **PlayerClimbState**: Handles climbing mechanics.
     - **PlayerKnockState**: Triggers on impact from collision with levels while dashing or fatal falls.
     - **PlayerGetUpState**: Recovery from the knocked state.
     - **PlayerDeadState**: The player is dead.

3. **LevelService**: Manages the procedural generation and updates of level structures dynamically.
   - **LevelType**: Enum defining **types of terrain**.
   - **LevelConfig**: Stores **configuration settings** for level layouts.
   - **LevelPool**: Implements **object pooling** to optimize performance.
   - **LevelController**: Handles **level functionalities** on creation.
   - **LevelModel**: Stores **level configurations**.
   - **LevelView**: Controls the **visual representation of levels**.

4. **CollectibleService**: Manages collectible behavior, spawning, and interaction logic.
   - **CollectibleType**: Enum defining collectible types.
   - **CollectibleConfig**: Stores **configuration settings** for collectibles.
   - **CollectiblePool**: Implements **object pooling** to optimize collectible instantiation.
   - **CollectibleController**: Handles **collectible functionalities**.
   - **CollectibleModel**: Defines **collectible attributes**.
   - **CollectibleView**: Controls the **visual representation of collectibles**.

5. **ScoreService**: Manages **score tracking and updates** the UI dynamically.

6. **EventService**: Manages event-driven communication across services.
   - **EventController**: Inherits from `EventController` to manage event registrations and notifications.

7. **SoundService**: Manages sound effects and background music for immersive gameplay.
   - **SoundType**: Enum categorizing sound effects (e.g., jumping, sliding, dashing).
   - **SoundConfig**: Stores audio clips for game.

8. **UIService**: Manages user interface interactions and HUD updates dynamically.
   - **UIController**: Handles menu interactions and HUD logic.
   - **UIView**: Manages the visual representation of UI elements.

9. **InputService**: Processes and manages game and player inputs using Unity's New Input System for precise control.

10. **CameraService**: Handles camera setting such as setting object to follow for Main & Mini Camera and toogling for Mini Camera.
    - **CameraConfig**: Stores camera-related configurations.

11. **Utilities**: Provides generic, reusable utilities for better performance and scalability.
    - **GenericObjectPool**: Handles **object pooling** for collectible & levels.
    - **GenericStateMachine**: Manages **state transitions for modular components**.
    - **IState**: Interface defining state behaviors.
    - **IStateOwner**: Interface for state machine controllers.

---

## Development Workflow

| **Branch**                         | **Feature**                                        |
|-------------------------------------|----------------------------------------------------|
| `Branch-0-Game-Setup`              | Initial project setup.                            |
| `Branch-1-Player-Setup`            | Implemented **player movement, animations & collisions**. |
| `Branch-2-Level-Generation`        | Implemented **procedural level generation**.      |
| `Branch-3-Collectibles`            | Added **collectibles and pickup logic**.         |
| `Branch-4-Score-System`            | Integrated **score tracking mechanics**.         |
| `Branch-5-Game-Controller-And-UI`  | Developed **core game controller & UI system**.  |
| `Branch-6-Sound-System`            | Added **sound effects and background music**.    |
| `Branch-7-Service-Locator-DI`      | Implemented **Service Locator & Dependency Injection**. |
| `Branch-8-MVC`                     | Structured the game using **MVC architecture**.   |
| `Branch-9-Object-Pool`             | Implemented **object pooling for performance**.  |
| `Branch-10-State-Machine`          | Created **state machines for game and player**.  |
| `Branch-11-Observer-Pattern`       | Integrated **event-driven communication**.       |
| `Branch-12-Polish`                 | Improved **assets, mini-camera & did final touches**. |
| `Branch-13-Documentation`          | Created **detailed project documentation**.      |

---

## Events

| **Event Name**                 | **Description**                                      |
|--------------------------------|------------------------------------------------------|
| `GetPlayerTransformEvent`      | Returns the player's `Transform`.                    |
| `CreateCollectiblesEvent`      | Generates new collectibles on level.                 |
| `OnCollectiblePickupEvent`     | Triggered when a player picks up a collectible.      |
| `UpdateHealthUIEvent`          | Updates the health display in the UI.                |
| `UpdateScoreUIEvent`           | Updates the score display in the UI.                 |
| `PlaySoundEffectEvent`         | Plays a specific sound effect.                       |

---

## Script and Asset Hierarchy

1. **Scripts**:
   - **Main**: Core game mechanics and states.
   - **Player**: Player behavior.
   - **Level**: Procedural level system.
   - **Collectible**: Collectible system.
   - **Score**: Score system.
   - **Camera**: Camera maintenance.
   - **UI**: Menus and in-game UI.
   - **Sound**: Audio playback.
   - **Event**: Event-based communication.
   - **Input**: Decoupled new input system.
   - **Utility**: Game utilities like Generic Object Pooling and Generic State Machine.

2. **Assets**:
   - **Character Model & Animations**: From Adobe Mixamo.
   - **Prefabs**: Self-created using Unity tools.
   - **Art**: Art is downloaded from [Screaming Brain Studios](https://screamingbrainstudios.itch.io/).
   - **Sounds**: Royalty-free sources.

---

## Setting Up the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/123rishiag/Forever-Sprint-2.5D.git
   ```
2. Open the project in Unity.

---

## Video Demo

[Watch the Gameplay Demo](https://www.loom.com/share/bc7e1eaa35ba4ea486a278d4bab17028?sid=60d4b5e5-eeca-45e4-af23-5027c8240ed6)  
[Watch the Architecture Explanation](https://www.loom.com/share/2c637015af0a4544ad7ba3a5777664cc?sid=d7b4c8a9-5bc5-492c-9a9f-657de6d7faf9)

---

## Play Link

[Play the Game](https://123rishiag.github.io/Forever-Sprint-2.5D/)

---