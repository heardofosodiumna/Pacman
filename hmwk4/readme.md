### 1) What did you do for avoiding a group of agents? What are the weights of path following and evade behavior? <br />
+ We implemented cone checking and collision prediction and realized that collision prediction is a better since it provides a better outcome. We found that weighting all steering behaviors the same resulted in the best outcome.<br />
      
### 2) What are the differences in cone check and collision prediction’s performances? <br />
+ In cone checking we are only checking in a cone shape in front of a unit. If a target is infront of the unit but not within the cone then cone checking is not activated.<br />
+ In collision prediction we check for the future position of two units using their current position and velocities. If the colissions are close enough for them to collide then a evade is called for the units not to collide.<br />
+ Cone check is not consistant and sometimes make a unit halt due to the conflicting forces. It would be better used for predicting collisions with static objects since they do not have velocity. Collision prediction is a lot smoother and allows units to continue in their path while avoiding obstacles.
