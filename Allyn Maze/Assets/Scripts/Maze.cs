using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public IntVector2 iSize;
    public MazeCell mazeCellPrefab;
    private MazeCell mazeCellInstance;
    private MazeCell[,] cells;

    public float fGenerationStepDelay;

    // Using an enum to generate the maze one cell at a time
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(fGenerationStepDelay);

        //set the size of the maze 
        cells = new MazeCell[iSize.x, iSize.z];

        //generate matrice one cell at a time 
        for(int i = 0 ; i < iSize.x; i++)
        {
            for(int j = 0 ; j < iSize.z; j++)
            {
                yield return delay;
                CreateCell(new IntVector2(i, j));
            }
        }

    }

    //will take two values as coordinates to instantiate a clone maze cell
    private void CreateCell(IntVector2)
    {
        MazeCell newCell = Instantiate(mazeCellPrefab) as MazeCell;
        //place new cell into array
        cells[a_Coordinates.x, a_Coordinatess.z] = newCell; 
        newCell.iCoordinates
            // keep track of cells according to position in the grid
    }
}
