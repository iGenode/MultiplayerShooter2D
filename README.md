# MultiplayerShooter2D

MultiplayerShooter2D is a multiplayer 2D shooter built with Photon Fusion targeting Android platform

# Features
Current version of the game includes some features of the 3 days task:

- The __lobby scene__, which has input fields for a player nickname and lobby names.
  * Player can create a new lobby or join an existing one by typing the name of the lobby and pressing the corresponding button

- The __game scene__ with a WIP map.
  * Players spawn at a random position as soon as they join the server.
  * Players have the ability to move around via on screen joystick and shoot in a direction of their rotation.
  * Players loose health as they get hit; players have 5 health by default and despawn after dying.
  * Players are identified by nicknames

Gameplay video is available [here (Google Drive)](https://drive.google.com/file/d/1UVVH8pF7GY2x3qeOyEUqy4I-mybIsoiD)

# Tech stack

Here's a brief overview of the technologies used in this project:
- Photon Fusion
- Unity UI

# Install

Download the __latest release__ apk on repo's [release page](https://github.com/iGenode/MultiplayerShooter2D/releases):

___

##### TODO's

- 2D character controller
- All the other features listed in 3 days task
- Object Pooling for projectiles and particles
- Player join/leave/death notifications
- Host migration?
- Ready up menu
- Sprites and sounds
- Map layout
