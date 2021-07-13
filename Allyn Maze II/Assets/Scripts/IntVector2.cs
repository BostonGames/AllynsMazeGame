using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntVector2
{
    public int x, z;

   
    public IntVector2 (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // takes two vectors and adds them together
    public static IntVector2 operator+(IntVector2 a_A, IntVector2 a_B)
    {
        IntVector2 iTemp;
        iTemp.x = a_A.x + a_B.x;
        iTemp.z = a_A.z + a_B.z;
        return iTemp;

    }
}
