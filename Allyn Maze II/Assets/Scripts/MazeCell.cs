using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public IntVector2 iCoordinates;
    private int iInitialisedEdgeCount;

    // this will be 4 since there are 4 edges to a cell
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];


    private void OnMouseDown()
    {
        if (GameManager.instance.mazeIsGenerated && !GameManager.instance.goalIsPlaced)
        {
            Vector3 spawnPos = transform.position;
            GameManager.instance.SpawnGoal(spawnPos);
            GameManager.instance.goalIsPlaced = true;
        }
    }

    public MazeCellEdge GetEdge (MazeDirection a_Direction)
    {
        return edges[(int)a_Direction];
    }

    public void SetEdge(MazeDirection a_direction, MazeCellEdge a_Edge)
    {
        // turn direction into integer
        edges[(int)a_direction] = a_Edge;
        // keep track of how many edges are created on the cell
        iInitialisedEdgeCount += 1;
    }

    // keep track of how often a cell has been set
    public bool IsFullyInitialized
    {
        // return true if all the edges on a Mazecell have been initialized
        get
        {
                                            // 4; for each of the 4 sides           
            return iInitialisedEdgeCount == MazeDirections.Count;
        }
    }

    // choose a random uninitialized direction
    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            // this checks the edge array to see if there are any skips left, it chooses the current direction
            // if no skips left, it decreases skips by 1
            // initially skips are a random number between 0 and the number of walls available
            int iSkips = Random.Range(0, MazeDirections.Count - iInitialisedEdgeCount);
            // run through the edge array and see if there is an opening (uninitialized or "null"), check how many skips are left
            // starts on the South edge
            for(int i = 0; i <  MazeDirections.Count; i++)
            {
                // if there is an opening in the wall
                if(edges[i] == null)
                {
                    // are there any skips left?
                    if(iSkips == 0)
                    {
                        // if not, then that is the direction it chooses
                        return (MazeDirection)i;
                    }
                    iSkips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left. Refer to MazeCell script.");
        }
    }
}
