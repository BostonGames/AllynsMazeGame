using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX, gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // this way you will not have to assign fCost; it will be given with h+gCosts
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int heapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // we want to compare the fCosts of the two nodes 
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        // if the two fCosts are equal
        if(compare == 0)
        {
            // returns 1 if int is higher
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        // we want to return which node has a Lower hCost though, so reverse the sign to swap the correct node
        return -compare;
    }

}
