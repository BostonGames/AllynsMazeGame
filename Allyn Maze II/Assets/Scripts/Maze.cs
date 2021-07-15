using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public static Maze instance;
    public IntVector2 iSize;

    public Vector3 spawnPos;

    public MazePassage mazePassagePrefab;
    public MazeWall mazeWallPrefab;
    public MazeCell mazeCellPrefab;

    private MazeCell[,] cells;


    public float fGenerationStepDelay;

    private void Awake()
    {
        instance = this;
    }


    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(fGenerationStepDelay);
        cells = new MazeCell[iSize.x, iSize.z];
        List<MazeCell> activeCells = new List<MazeCell>();

        DoFirstGenerationStep(activeCells);

        // while coordinates are inside the grid, we want to create a cell
        // only create a new cell if it has not been created in the new coordinates (is null)
        while(activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }

        if(activeCells.Count == 0)
        {
            MazeFinished();
        }
    }

    private void MazeFinished()
    {
        GameManager.instance.BeginMaze();
        GameManager.instance.SpawnPlayer(spawnPos);

    }


    private MazeCell CreateCell(IntVector2 a_Coordinates)
    {
        MazeCell newCell = Instantiate(mazeCellPrefab) as MazeCell;


        // in our array it will be placed according to it's coordinates
        cells[a_Coordinates.x, a_Coordinates.z] = newCell;

        // set the coordinates inputted as the new cell coordinates
        newCell.iCoordinates = a_Coordinates;

        // to keep track of the cells, we will name them as their coordinates on the grid
        newCell.name = "MazeCell " + a_Coordinates.x + " , " + a_Coordinates.z;

        // set it's position and make it a child of our maze
        newCell.transform.parent = transform;

        // set new cells position to align within the grid. Using 0.5f so that the entire grid centers
        newCell.transform.localPosition = new Vector3(a_Coordinates.x - iSize.x * 0.5f + 0.5f, 0f, a_Coordinates.z - iSize.z * 0.5f + 0.5f);

        if (newCell.iCoordinates.x == 0 && newCell.iCoordinates.z == iSize.z - 1)
        {
            spawnPos = newCell.transform.position;
        }

        return newCell;

    }

    // generate random coordinates
    public IntVector2 RandomCoordinates
    {
        get
        {
            // return a random number between 0 and the length of the grid
            return new IntVector2(Random.Range(0, iSize.x), Random.Range(0, iSize.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 a_Coordinates)
    {
        // check if it within the grid itself
        return a_Coordinates.x >= 0 && a_Coordinates.x < iSize.x && a_Coordinates.z >= 0 && a_Coordinates.z < iSize.z;
    }

    public MazeCell GetCell (IntVector2 a_Coordinates)
    {
        // will return a cell at given coordinates
        // useful for checking if a cell has already been activated
        return cells[a_Coordinates.x, a_Coordinates.z];
    }

    // implement backtracking
    // create a cell at random coordinates
    private void DoFirstGenerationStep (List<MazeCell> a_ActiveCells)
    {
        // taking the list and adding a new cell to the active cells list
        // using CreateCell as a method to return a new cell instead of a void function
        a_ActiveCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep (List<MazeCell> a_ActiveCells)
    {
        // create an integer to store what index we are at
        int iCurrentIndex = a_ActiveCells.Count - 1;
        MazeCell currentCell = a_ActiveCells[iCurrentIndex];

        if (currentCell.IsFullyInitialized)
        {
            a_ActiveCells.RemoveAt(iCurrentIndex);
            return;
        }

        // reset direction to be random
        // turn random value and store in direction itself
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        // add the direction to the coordinates given direction to the cell itself
        IntVector2 iCoordinates = currentCell.iCoordinates + direction.ToIntVector2();
        // check if coordinates are within the grid
        // and get cell coordinates to be = null, which means no cell is there yet
        // same as in the Generate function because we will be using this to replace it
        if(ContainsCoordinates(iCoordinates))
        {
            // this is all of the code for inside the grid
            MazeCell Neighbor = GetCell(iCoordinates);
            if(Neighbor == null)
            {
                // if cell adjacent to current cell has not been activated yet,
                // create cell at the coordinates
                Neighbor = CreateCell(iCoordinates);
                CreatePassage(currentCell, Neighbor, direction);
                a_ActiveCells.Add(Neighbor);
            }
            else
            {
                // if neighbor exists, separate the neighbor and current cell with a wall
                CreateWall(currentCell, Neighbor, direction);
            }
        }
        else
        {
            // if coordinates are outside of the grid / maze
            // neighboring cell is null becuase there is no neighboring cell at the edges
            CreateWall(currentCell, null, direction);
        }

    }

    // create and initialize the walls and passages
    private void CreatePassage(MazeCell a_CurrentCell, MazeCell a_NeighborCell, MazeDirection a_Direction)
    {
        MazePassage passage = Instantiate(mazePassagePrefab) as MazePassage;
        passage.Initialize(a_CurrentCell, a_NeighborCell, a_Direction);
        passage = Instantiate(mazePassagePrefab) as MazePassage;
        passage.Initialize(a_NeighborCell, a_CurrentCell, a_Direction.GetOpposite());
    }

    private void CreateWall (MazeCell a_CurrentCell, MazeCell a_NeighborCell, MazeDirection a_Direction)
    {
        MazeWall wall = Instantiate(mazeWallPrefab) as MazeWall;
        wall.Initialize(a_CurrentCell, a_NeighborCell, a_Direction);
        if(a_NeighborCell != null)
        {
            wall = Instantiate(mazeWallPrefab) as MazeWall;
            wall.Initialize(a_NeighborCell, a_CurrentCell, a_Direction.GetOpposite());
        }
    }

}
