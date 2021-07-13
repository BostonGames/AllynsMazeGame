using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// keeps track of the connections between the cells given a reference to the cell and the neighboring cell
// abstract so we dont need to create an instance of the generic MazeCellEdge
public abstract class MazeCellEdge : MonoBehaviour
{
    public MazeCell currentCell, neighborCell;
    public MazeDirection direction;

    // make edges the child of the cell and place in the same location
    public void Initialize(MazeCell a_CurrentCell, MazeCell a_NeighborCell, MazeDirection a_Direction)
    {
        // set all the input values to the vars
        this.currentCell = a_CurrentCell;
        this.neighborCell = a_NeighborCell;
        this.direction = a_Direction;

        // states that whatever type of cell edge that is a passage or wall will be put here
        a_CurrentCell.SetEdge(direction, this);

        // make child of the cell
        transform.parent = a_CurrentCell.transform;
        transform.localPosition = Vector3.zero;

        transform.localRotation = direction.ToRotate();
    }
}
