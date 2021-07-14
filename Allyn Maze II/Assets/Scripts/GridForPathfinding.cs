using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridForPathfinding : MonoBehaviour
{
    private Node[,] grid;
 
    private Vector2 gridWorldSize;
    public float nodeRadius;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    public LayerMask unWalkableMask;


    private void OnDrawGizmos()
    {
        gridWorldSize.x = Maze.instance.iSize.x;
        gridWorldSize.y = Maze.instance.iSize.z;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // visualize what is walkable 
                Gizmos.color = (n.walkable ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f));
                // so the cube is the size of the node, and leave a little room for the walls
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    private void OnEnable()
    {
        CalculateGridSizeForPathfinding();
    }

    //calculates the grid size to be navigated
    public void CalculateGridSizeForPathfinding()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(Maze.instance.iSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(Maze.instance.iSize.z/ nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 gridTopLeft = transform.position = new Vector3(-Maze.instance.iSize.x/2, 1, Maze.instance.iSize.z / 2 );


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridTopLeft - Vector3.left * (x * nodeDiameter + nodeRadius) + -Vector3.forward * (y * nodeDiameter + nodeRadius);
                // make walkable if there are no collisions in the Unwalkable mask
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));
                // pass this information into the node
                grid[x, y] = new Node(walkable, worldPoint);
            }

        }
    }

}
