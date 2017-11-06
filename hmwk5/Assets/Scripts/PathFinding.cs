using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    ArrayList map;
    MapGen mapgen;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


/*def aStarSearch(problem, heuristic= nullHeuristic):
    "Search the node that has the lowest combined cost and heuristic first."
    
    fringe = util.PriorityQueue()
    fringe.push((problem.getStartState(), []), heuristic(problem.getStartState(), problem) )                   
    visited = []
    while fringe: 					                                    #while fring is not empty
        node, actions = fringe.pop()		                            #get the first node, actions
        if problem.isGoalState(node):					                #if it is a goal state
                return actions      	                                #return succes     
        if not node in visited:                                         #if its not in visited     
            visited.append(node);
            for state, nswe, steps in problem.getSuccessors(node):           
                fringe.push((state, actions + [nswe] ), problem.getCostOfActions(actions + [nswe]) + heuristic(state, problem) )*/
