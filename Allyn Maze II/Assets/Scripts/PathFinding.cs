using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    // both the GridForPathfinding and Pathfinding script need to be on the same GameObject in the Heirarchy to work
    // ther is probably a cleaner way to do this, but for now:
    GridForPathfinding grid;
    public GameObject seeker, target;


    private void Awake()
    {
        grid = GetComponent<GridForPathfinding>();
    }


    private void OnEnable()
    {
        if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Goal"))
        {
            seeker = GameObject.FindWithTag("Player");
            target = GameObject.FindWithTag("Goal");
        }
        else { return; }
    }

    public void FindPath()
    {
        if (GameManager.instance.goalIsPlaced)
        {
            FindPath(seeker.transform.position, target.transform.position);
        }
        else 
        {
            GameManager.instance.ShowWarning();
        }

    }

    private void FindPath(Vector3 startPos, Vector3 goalPos)
    {
         Stopwatch stopWatch = new Stopwatch();
         stopWatch.Start();
        
         Node startNode = grid.NodeFromWorldPoint(startPos);
         Node targetNode = grid.NodeFromWorldPoint(goalPos);

         // list of nodes for Open set
         // **I did not implement the Heap because it kept making my editor crash
         List<Node> openSet = new List<Node>();
         HashSet<Node> closedSet = new HashSet<Node>();

        // add the start node to the open set
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            // need to find the node with the lowest f value
            // want to optimize from this nested structure later
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                // move to node with lowest f cost
                // if fCosts are equal, move to the node with the lowest hCost
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // if we have found the path
            if(currentNode == targetNode)
            {
                stopWatch.Stop();
                print("path found in: " + stopWatch.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(currentNode))
            {
                // if neighbor is not traversable or neighbor is in CLOSED list, skip to next neighbor
                if(!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
               
                // if the gCost is lower than the current node and is not already in the Neighbor list
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) 
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    // hCost is the distnace from this node to the Goal or Target Node
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    // set parent of neighbor to currentNode so it can point toward it's origin
                    neighbor.parent = currentNode;

                    // check if the neighbor is not in the open set, then add
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
               
            }
        }
    }

    // get path from Start node to Goal/Target node
    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // retrace steps until we reach the start node
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        // ^^ path is currently in reverse, we need to invert that
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            // formula for getting the shortest path between 2 points
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);

        }
    }
}
