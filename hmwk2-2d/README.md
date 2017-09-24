# Game AI Homework 2
# Enoch Huang
# Igor Carvalho
# Jeremy Torella
This is the second homework for RPI Game AI 2017.

In our game, Jerry the mouse uses pathfinding to collect wedges of cheese. 
Meanwhile, Tom the cat dynamically wanders around the screen until Jerry comes within Tom's search radius.
At this point, Tom will dynamically pursue Jerry, and once he gets close enough to Jerry, Tom will try to dynamically arrive at Jerry's location.
Once Tom gets close enough to Jerry's location, Jerry will start to stray from the path and dynamically evade Tom. 
Once Jerry gets far enough away from Tom, he will return to pathfinding and return to the point on the path that he was on before he began evading.
Furthermore, when Jerry gets far enough away from Tom, Tom will return to dynamically wandering the scene.