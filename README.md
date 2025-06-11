## **1v1 Pokémon-esque Turn-based Combat Manager Tool**

## Introduction
This is a tool that helps with setting up a pokémon-esque 1v1 turn-based combat system.
It is made using Unity version 6000.0.39f1
It uses Scriptable Objects and Classes as its basis to work.

It makes it easy to set up the following:<br/>
-Allies and enemies, each with their own types, stats and movesets. <br/>
-New types with their own weaknesses and strengths <br/>
-Custom statistics <br/>
-Custom resources, like mana, shield or HP. <br/>
-Secondary effects like statboosts and resource drain <br/>
-Moves with their own stats and secondary effects <br/>
-Attack types (which stat the move uses to attack). <br/>

## Installation

if using Github, download the codebase like this: <br/>
 <img src="ImagesInReadme/downloadOrClone.png" alt="clone repo" width="200"> 

## Project Setup
*Where do I create a new move/statistic/resource/attacktype/ally or enemy?*

Creating an individual element is done in its respective folder. <br/>
<img src="ImagesInReadme/Libraries and data.png" alt="clone repo" width="500"> <br/>
Then, add it to its respective library to make sure the rest of the system use it.

Adding extra values or a check can be done in the script. Usually called ...data, but some scripts use ...definition (this is an issue and should still be updated):
<img src="ImagesInReadme/Scripts.png" alt="clone repo" width="500"> <br/>

## Use Case Examples

Making moves (as found in the Libraries and Data folder) <br/>
<img src="ImagesInReadme/CreatingAMoveBarrelRoll.png" alt="clone repo" width="500"> 

Making allies or enemies <br/>
<img src="ImagesInReadme/CreatingAnAlly.png" alt="clone repo" width="500"> 


## AI Usage
This project uses AI in its code

## Issues

Everything about setting up the data containers and values works, but there are still some problems with the combat code, battleUI and TurnManager <br/>
-The current combat system only allows for doing damage (not added effects or status effects) <br/>
-Moves can't use resources in actual battle. </br>
-BattleUI currently does not correctly display and update the currenthealth values. <br/>


## Contributing
Contributing to this code is entirely allowed, but code must be approved by the creator via a push request

## License
CC0 -[Creative Commmons](https://creativecommons.org/public-domain/cc0/)
