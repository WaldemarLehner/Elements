
# Project: Elements

Project: Elements is a 2D Rogue-Like. The game's goal is completing all rooms by defeating the enemies in it. After completing a room the player can purchase a powerup

![Screenshot](https://i.imgur.com/06rzDMw.png)
[Trailer](https://www.youtube.com/watch?v=IAqc7bcY9iM)

# Controls

The game is played with Keyboard and Mouse.

WASD moves the player.

Shift makes the player sprint.

Space makes the player dash in a direction. During that dash the player is invulnerable.


# Project Structure

The project is set up using the MVC-Pattern

## Model

All Definitions and Logic of the game elements is contained in the Model. These get called by the model. 

## View

The View implements the Renderer. We used OpenTK, a C# Wrapper for openGL. It also contains Helper-Classes to, for example, convert between World- and Normalised Device Coordinates.

## Controller

The Controller contains the logic to get the current mouse and keyboard inputs, and passes these to the model.
It also triggers the View and Model.

# Sources

All the code and textures are made by ourselves.
The music is from Castle Crashers and has been released under CC BY-NC-SA, but the original links seems to be dead. You can find all sound tracks from that game [here](https://castlecrashers.fandom.com/wiki/Castle_Crashers_Soundtrack). [Here](https://creativecommons.org/2008/09/22/castle-crashers-soundtrack-released-under-cc-license/) is a CC Article talking about the soundtrack. If you have a working link, feel free to open up an issue.
