using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVector2
{
    public int x, z;

    // takes in x and z values and sets as the Object's value
    public IntVector2 (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // taking two vecotrs and adding them together
    public static IntVector2 operator+(IntVector2 a_A, IntVector2 a_B)
    {
        IntVector2 iTemp;
        iTemp.x = a_A.x + a_B.x;
        iTemp.z = a_A.z + a_B.z;
        // return a temporary vector
        return iTemp;
    }
}
