using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public IntVector2 iSize;

    [SerializeField] GameObject Player;
    public GameObject mazeGoalTarget;
    public bool mazeIsGenerated;
    public bool goalIsPlaced;
    public Vector3 offset = new Vector3(0f, 0.4f, 0f);


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

    public void SpawnPlayer(Vector3 spawnPos)
    {
        Instantiate(Player, spawnPos + offset, Quaternion.identity);
    }

    public void SpawnGoal(Vector3 spawnPos)
    {
        Instantiate(mazeGoalTarget, spawnPos + offset, Quaternion.identity);
    }


    public void BeginGame()
    {
       mazeInstance = Instantiate(mazePrefab) as Maze;

        StartCoroutine(mazeInstance.Generate());
    }

    public void BeginMaze()
    {
        mazeIsGenerated = true;
        
    }

    public void RestartGame()
    {
        if (mazeIsGenerated)
        {
            Destroy(GameObject.FindWithTag("Player"));
        }
        if (goalIsPlaced)
        {
            Destroy(GameObject.FindWithTag("Goal"));
        }

        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
        mazeIsGenerated = false;
        goalIsPlaced = false;
    }
}
