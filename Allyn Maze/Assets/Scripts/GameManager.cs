using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntVector2 iSize;
    public MazeCell mazeCellPrefab;
    private MazeCell mazeCellInstance;
    private MazeCell[,] cells;

    public Maze mazePrefab;
    private Maze mazeInstance;



    private void Start()
    {
        BeginGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        StartCoroutine(mazeInstance.Generate());
    }

    private void RestartGame()
    {
        // make sure maze is no longer generated
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
