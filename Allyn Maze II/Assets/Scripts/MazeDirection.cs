using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum of all possible directions
public enum MazeDirection
{
    North,
    East,
    South,
    West
}

public static class MazeDirections
{
    // want to hold all different directions possible
    // our quad only has four sides so that stores 
    // the official number without having math errors
    public const int Count = 4;

    public static MazeDirection RandomValue
    {
        get
        {
            // converts the number to a maze direction
            // 0 = North
            // 1 = East
            // 2 = South
            // 3 = West
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    // convert the random dirction into coordinates
    // turn the vector into an array
    private static IntVector2[] iVectors =
    {
        new IntVector2(0,1),   // North
        new IntVector2(1,0),   // East
        new IntVector2(0, -1), // South
        new IntVector2(-1,0)   // West
    };

    // return intVector when taking a direction
    public static IntVector2 ToIntVector2 (this MazeDirection a_Direction)
    {
        // make extension method of maze direction ^above^ with "this" 

        // corresponding vector of the direction inputted
        // find the direction, turn it into an integer, and collect the vector of the according integer
        return iVectors[(int)a_Direction];
    }

    // add opposite direction to maze
    // making sure they are in the same direction as the enums
    private static MazeDirection[] opposites =
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    // retrieval function that will input the direciton and output the opposite
    public static MazeDirection GetOpposite(this MazeDirection a_Direction)
    {
        return opposites[(int)a_Direction];
    }

    // make the walls rotate to their appropriate position
    private static Quaternion[] rotations =
    {
        // has to be in same order as maze directions: North, South, East, West
        Quaternion.identity,                                        // North
        // rotate 90 degrees North to go in West direction          
        // rotates around y axis to keep on xz plane                // East
        Quaternion.Euler(0f, 90f, 0),                               // South
        Quaternion.Euler(0f, 180f, 0f),                             // West
        Quaternion.Euler(0f, 270f, 0f)
    };

    // return rotation if given directional input
    public static Quaternion ToRotate(this MazeDirection a_Direction)
    {
        // returns the corresponding value in the quaternion array above
        return rotations[(int)a_Direction];
    }
}