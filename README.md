MonoTest
--------

Description:
* Abstractron example MonoGame solution set for VS2010 and VS Express for Windows 8.
* Loads background texture (abstractron logo) and two sprite textures that travel within screen bounds. Collision with edge of screen reflects velocity of sprite, collision between sprites results in laser sound at specific volume.

Solutions:
* MonoTest (VS Express Win8)
** This MonoGame project contains a reference to MonoGame framework for Windows 8. The MonoTest project is the actual game itself, containing a main MonoTestGame class that manages the run loop.

* MonoTestContent (VS2010)
** This XNA project is where all game content (images, sounds, etc.) are imported and built into .xnb files required by MonoGame. XNB file format is the content file format used by XNA.