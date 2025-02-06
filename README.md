# **Forever Sprint 2.5D**

## **Overview**
Forever Sprint 2.5D is an action-packed **endless runner** where players **jump, dash, slide, and climb** through dynamically generated levels while collecting 
collectibles. The game integrates various design patterns, including **Service Locator**, **Dependency Injection**, **Model-View-Controller (MVC)**, 
**Observer Pattern**, **Object Pooling**, and **State Machine**, ensuring modularity and scalability. **Scriptable Objects** handle flexible data storage, 
while **Unity's New Input System** provides precise player controls.

---

## **Architectural Overview**
Below is the block diagram illustrating the **core architecture**:

![Architectural Overview -- Adding Soon](docs/block_diagram.png)

---

## **Gameplay Elements**

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
| `AIR_JUMP`         | Allows a jump mid-air.                    |
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

## **Events**
| **Event Name**                 | **Description**                                      |
|--------------------------------|------------------------------------------------------|
| `GetPlayerTransformEvent`      | Returns the player's `Transform`.                    |
| `CreateCollectiblesEvent`      | Generates new collectibles on level.                 |
| `OnCollectiblePickupEvent`     | Triggered when a player picks up a collectible.      |
| `UpdateHealthUIEvent`          | Updates the health display in the UI.                |
| `UpdateScoreUIEvent`           | Updates the score display in the UI.                 |
| `PlaySoundEffectEvent`         | Plays a specific sound effect.                       |

---

## **Development Workflow**
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

## **Setting Up the Project**
1. Clone the repository:
   ```bash
   git clone https://github.com/123rishiag/Forever-Sprint-2.5D.git
   ```
2. Open the project in Unity.

---

## **Video Demo**
[Watch the Gameplay Demo](https://www.loom.com/share/bc7e1eaa35ba4ea486a278d4bab17028?sid=60d4b5e5-eeca-45e4-af23-5027c8240ed6)  
[Watch the Architecture Explanation](https://www.loom.com/share/2c637015af0a4544ad7ba3a5777664cc?sid=d7b4c8a9-5bc5-492c-9a9f-657de6d7faf9)

---

## **Play Link**
[Play the Game](https://123rishiag.github.io/Forever-Sprint-2.5D/)
---