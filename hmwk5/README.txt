# Game AI Homework 2
# Enoch Huang
# Igor Carvalho
# Jeremy Torella
This is the fifth homework for RPI Game AI 2017.

For this homework, we implemented a program that will generate a map, allow the player to place start and end points, and find the shortest path from start to end via the A* algorithm. 

In more detail, when the program is run, a map is generated using an input text file by procedurally creating a texture to lay over a mesh. The vertices, norms, and uv's were all procedurally generated based on the text file, as well. The player has the option to choose one of two different maps via a button on the UI. Then, the player can press the button labeled "Representation" to toggle between tile and waypoint representations of the map. In the tile representation, a grid of tiles is generated, with each tile being three pixels wide and three pixels tall. 

Using WASD, the player can pan around the map, and scrolling the mouse wheel up and down will zoom in and out of the map.

The player can click the "Set Start Location" button and then click anywhere on the map and a start location will be placed. Likewise, the player can click on the "Set End Location" button and then click anywhere on the map and an end location will be placed.

Once both start and end locations have been placed, the player can press the "Find Path" button and the program will begin running the A* algorithm in order to find the shortest path from the start to end location. The player can also toggle the heuristic button and change the heuristic weight slider to modify how the A* algorithm computes the cost of the tiles. The two heuristics we used for the grid were calculating the Manhattan distances and the Euclidian distances. The waypoints heuristic was the actual world distance to the end point.

Furthermore, the player has the ability to place additional tree obstacles by clicking the right mouse button while hovering over a walkable tile on the map.

NOTES:
When we ran our program, we noticed that the more zoomed out of the map we were, the speed at which the paths are checked and drawn is slower than if the camera was zoomed in. Therefore, we found that our program works best if the camera is slightly zoomed in.