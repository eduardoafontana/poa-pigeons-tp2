# PigeonsTP2

### Compilation and execution

The system was developed using the C# language, using the .NET Framework 4.7, through the Visual Studio 2017 IDE. The project uses the Windows Forms library as a visual resource.

To open the system through Visual Studio, you need to open the PigeonsTP2.sln file.
To compile and run the system through Visual Studio, use the Play button at the top center.

The source code to analyze can be found at:
- poa-pigeons-tp2\Config.cs
- poa-pigeons-tp2\Engine.cs
- poa-pigeons-tp2\Presentation.cs
- poa-pigeons-tp2\Program.cs
- poa-pigeons-tp2\Randomize.cs
- poa-pigeons-tp2\Characters\*
- poa-pigeons-tp2\Environment\*

To run the application without using Visual Studio, you must run the following file:
poa-pigeons-tp2\bin\Release\PigeonsTP2.exe

### Instructions and operation of the game

The aim of the game is to constantly feed the pigeons. 
To feed the pigeon the player has to click on the region of the window where the pigeon is located, as shown in the figure below.

A maximum of 5 pigeons will appear, each pigeon is a thread. They appear in random positions. They are waiting for food. If you don't feed them, they will fall asleep.
If you add food, represented by an apple, the pigeons move around to collect the food. The first pigeon that reaches the food manages to eat it.

If the apple is not eaten by the pigeon, it will rot. Each food is a thread. Food has three states: good, half rotten, totally rotten. When it has finished rotting, the food disappears. 
The pigeon only eats the food that is good, if the food rots while the pigeon is moving to catch it, when it realizes that it is rotting, it gives up.

There is a third character in the scene, which is the cat. The chat is also a thread. The cat is used to disrupt the scene. 
The cat appears from time to time, the time is random. When the cat appears, the sleeping pigeons wake up and the awakened pigeons randomly change their position.

[Image]
