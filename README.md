# Hexwars!
## About
Hexwars is a 2D mobile gaming app designed for Android phones. The basics of the game involves a small hexagon, controlled by the player, which is vulnerable to collision attacks from other nearby hexagons, controlled by other players. To play the game, you must increase your score by colliding into and defeating the other hexagons. Players gain points by destroying small destructible objects in the environment, as well as other hexagons. These points can be spent to upgrade the player’s hexagon by attaching new parts (of various shapes) to it.

Think agar.io meet Robot wars!

![Hexwars, the game](/screenshot.png?raw=true "Hexwars")

## Deploy Instructions
 * Ensure that you have Unity v5.4.1+ installed on your computer
  * `NOTE: Having Unity v5.4.1+ is critical, as this project uses functions that are not yet implemented in previous versions of Unity`  
 * Clone the repository `git clone git@bitbucket.org:teambronze/comp30022-team-bronze.git hexwars`
 * Open the `hexwars` project that you just cloned in Unity
 * Locate `Launcher` in Assets, and put it in the Hierarchy (ensure it is the only scene in the Hierarchy)
 * Press play to view the game. For full functionality, build the game as an Android APK and run it on your phone.

## Testing Instructions
 * Open the Editor Tests Runner (built into Unity 5.4.1+)
 * Select the test you would like to run
 * Press 'Run Selected'
