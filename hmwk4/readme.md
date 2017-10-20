### Game AI CSCI 4967 <br/>
### Homework 4<br/>
#### By:<br/>
+ ### Igor Carvalho<br/>
+ ### Enoch Huang<br/>
+ ### Jeremy Torella<br/>

### What did you use for obstacle avoidance?<br/>
+ #### You can use ray cast, cone check or collision predication<br/>
  + ##### For ray cast, you can use Unity’s function if you wish<br/>
+ #### You cannot just use the colliders in Unity<br/>
We used ray cast for avoiding walls. Collision predition for avoiding each other.<br/>
### What are the heuristics for the agents to go through the tunnels
We check and see when the flock is apporachiing the tunnel, when it is about to then it reorganized it to go through the tunner.<br/>
### Did you use any additional heuristics?
No.
### What are the differences in the three groups’ performances?
Scalabe formation seems like it has relatively good performance since it can dynamically scale the slots for each agent. This is useful when trying to maintain formation when rounding a corner. The scalable formation can simply shrink the slots so the agents are closer together when the leader is pathfinding around a corner. Emergent formation gives the agent more customizabilty because each agent can be given a different behavior pattern. This allows the formation as a whole to have more complex behaviors. For example, maybe in some situations it would be more advantageous for certain agents to move further apart from the rest of the group. Two-level formation is a good balance between the two previous behaviors. The flock is able to retain a strong formation and at the same time it is possible for the agents to be more flexible if the situation calls for it since each agent does collision avoidance by itself. This ends up being a little bit more expensive though, as the formation has to constantly check for formation as well as the individual avoidance checks. 
