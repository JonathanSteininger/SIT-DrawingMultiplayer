# DrawingMultiplayer

<h2>Why I made this</h2>
This project was made for the final project in my studies in Level 6.

The goal was to make two clients be able to communicate with each other in any way.
This could have been a simple as makeing to clients be able to send messages accross each other like a chat app.

With this project I wanted to push myself. It ended up a bit rushed so I spagetified my code a bit in the end due to having a short time limit.

<h2>About the project</h2>

This application is a Winform GUI application as a MS Paint clone with the ability to play multiplayer. I never added the ability to save images.

I added a scene manager to easily swap states in the application. This allows for easy transitions between menus.

<h3>The MS Paint portion</h3>
When the user start a game they will be presented by a large canvas, and a banner at the top containing drawing options and a way to return to menu.
The user can select different colours and pen sizes to draw with. The user can draw on the canvas just by dragging their mouse over the canvas.

<h3>Single-Player</h3>

When the user start a single player game they will be put into the MS Paint canvas like normal.

<h3>Multi-Player</h3>

When the user start a multi-player game they can enter a port number their game will be accessible on. When the game start a TCP listener will be opened on the port number selected.
The user's application that started the multiplayer game will become the host of the game and anyone that joins are the client applications. 

<h4>Another player Joining</h4>
In order for someone to join the have to be on the same network and enter the same port number as the host. If it finds the host it will join the game, otherwise it will create a new game.

When a client connects to the host the host will automatically create a new worker thread to manage the connection. 

The host will send and recive a bunch of data to set up the state of the game and players.
Each line has a time of creation on it to manage old lines and duplicate lines. if there are new lines on a client, it will send them to the host.
When the host will recive the lines, it will periodically draw all the lines in its buffer and send all the new lines drawn to every other client which will keep everything in sync.

<h2>Things I haven't done</h2>
<h4>Timing</h4>
I never fixed an issue where the lifetime of the game is nevery synchronised for each connecting client causing the time stamps of drawn lines to be incorrect. This can lead to the lines recived by the host to not draw the lines in the correct order due to the times being incorrect.

<h4>Saving</h4>
I never added the ability to save the image or state of the game.
