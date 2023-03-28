# Super Mario Bros Clone
___*Note: This game is made to develop and explain Unity3D and C# skills of mine.*___

<hr>

## Mario's Game Physics
<br>
For this, the Rigidbody2D component of the Unity3D game engine and the values at which Mario's movements will accelerate when the keys are pressed, the friction force etc. are written in such a way that it can be changed externally.

![Physics](https://i.ibb.co/4242f42/Ekran-Resmi-2023-03-28-12-39-42.png)

<hr>

## Animations

<br>

There are three layers of animation (Little Mario, Super Mario, Fiery Mario).
According to the power enhancements the player receives, these animations change between layers and start playing the current level animations. 

<br>

![Animations GIF](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExODIyOGZhMjAyYzM2MWEyOTk2NTE5YWY5YmZmMWVjZGZjNjY3MGEzMyZjdD1n/NuNQpQYXfLPbbLfd0p/giphy.gif)

<hr>

## Level Design

<br>

The floor is made with the TileMap feature developed by Unity3D developers.
Objects that require interaction and have different width and height dimensions are placed on the game stage in the form of prefabs.

<br>

![TileMap GIF](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYzc3OGM0N2FjMWYwZGVkOTNjZTdmZWRhNDBhZGY2NjYyYTllZWJmNyZjdD1n/hmlOyanezMlSOsJXSw/giphy.gif)

<hr>

## Particle System

<br>

In the game, the effect of breaking the empty brick and the gold effect shown in contact with blocks with gold are made with particle system.

<br>

![Particle System From The Game Engine](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExMDIyOTA3YjA5YTRmNDY3YjM2NTQzMDUxODdmZTE3NWE5MTdkOGIwZiZjdD1n/MR1EuSxF3ikgZLvSJn/giphy.gif)

![Particle System From The Game Engine](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExZDU2ODVmMDgxNDQ3M2RkYmM1ZDU5MWMwNWFiNWJmNWFiMjhlNDg5MSZjdD1n/gKkIAkgboevSjMMR99/giphy.gif)

![Particle System From The Game](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExZmMwMzkyNDVmOTFhMjJlMjU1Y2U0Y2IyODkyNjVjMDBjN2IzZWNkZiZjdD1n/O7Ha03OjAdn7frDgTm/giphy.gif)

<hr>

## Custom Inspector

<br>

Since some pipes and blocks have different functions, a special interface has been designed to facilitate level design.

<br>

![Custom Inspector Block](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExNmFiOGVjY2QzZDE0YTEwMzU2YmM1YTAyOWUyZWEyNTJlMGM0OTBjMiZjdD1n/8MCqmrFawdJqa1huXJ/giphy.gif) ![Custom Inspector Pipe](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExM2MzYWQzMWJlNmNjZGM4OWU5ZGNjNmU4NzQ5MTRkZGFlOTM3MjVjNSZjdD1n/RquXrnAf4eRMgEIrRX/giphy.gif)

<hr>

## Game Footage

![Footage 1](https://media.giphy.com/media/NsOhVj8RII7ZgHH6YN/giphy.gif)
![Footage 2](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYzg0Yjk2YjhiYTk0YzRmMzUzNzZlZDBjMDc0NTI3MGJjMTQwMDQ1ZSZjdD1n/9XCkRMslJgCQsy1wlt/giphy.gif)
![Footage 3](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExZDU3YzJhYmU0MzI0MGI2MWY1MGFmMjJiZTc5NDRhMjMwODExODU4MiZjdD1n/Pii4BwowkNeVP6NHL3/giphy.gif)
![Footage 4](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYTAxZjA5NzEzYmI1MWExODkwMmUzMzIxOTZhZTQyNTA4NjhkNzQ1ZCZjdD1n/HRMZysoE2DkJwibs9I/giphy.gif)

## Game Controller

← → : Movement
↓ : Crouch
Z : Fire/Run
X : Jump
Enter : Select/Pause
