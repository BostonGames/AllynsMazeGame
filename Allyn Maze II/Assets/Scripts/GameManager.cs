using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public IntVector2 iSize;
    
    public MazeCell mazeCellPrefab;
    private MazeCell mazeCellInstance;
    private MazeCell[,] cells;

    public Maze mazePrefab;
    private Maze mazeInstance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BeginGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void BeginGame()
    {
       mazeInstance = Instantiate(mazePrefab) as Maze;

        StartCoroutine(mazeInstance.Generate());
    }

    public void BeginMaze()
    {
        Debug.Log("Maze Successfully Generated");
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
