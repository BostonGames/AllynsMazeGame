using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridForPathfinding : MonoBehaviour
{
    // canDelete FORTESTING 
    public Transform playerPlaceholder;
    
    private Node[,] grid;
 
    private Vector2 gridWorldSize;
    public float nodeRadius;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    public LayerMask unWalkableMask;


    public List<Node> path;
    private void OnDrawGizmos()
    {
        gridWorldSize.x = Maze.instance.iSize.x;
        gridWorldSize.y = Maze.instance.iSize.z;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));



        if (grid != null)
        {
            // have the pathfinding detect where the Player is
            // canDelete FORTESTING 
            Node playerNode = NodeFromWorldPoint(playerPlaceholder.position);

            foreach (Node n in grid)
            {
                // visualize what is walkable 
                Gizmos.color = (n.walkable ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f));
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = new Color(0, 1, 0, 0.5f);
                    }
                }

                // canDelete FORTESTING
                // make Player node a different color
                if (playerNode == n)
                {
                    Gizmos.color = new Color(0, 1, 1, 0.5f);
                }




                // so the cube is the size of the node, and leave a little room for the walls
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        // use a percentage to determine location on the grid
        // 0 = far left, 0.5 = center, 1 = right
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = 1 -(((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y));
        // clamp between 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        // need x and y indexes of grid array

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
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

        Vector3 gridTopLeft = transform.position = new Vector3(-Maze.instance.iSize.x/2, 0.4f, Maze.instance.iSize.z / 2 );


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridTopLeft - Vector3.left * (x * nodeDiameter + nodeRadius) + -Vector3.forward * (y * nodeDiameter + nodeRadius);
                // make walkable if there are no collisions in the Unwalkable mask
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));
                // pass this information into each individual node
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }

        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // search in a 3x3 block around the node
        // x = 0 && y = 0 means we are in the center of that (current Node position)
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                    continue;
                
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // check if the node is inside of the grid and if so, add to list
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX,checkY]);
                }
            }
        }

        return neighbors;

    }

}
