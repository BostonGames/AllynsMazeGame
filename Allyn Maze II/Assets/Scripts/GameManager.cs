using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public IntVector2 iSize;

    [SerializeField] GameObject startUI;
    [SerializeField] GameObject gameUI;
    public GameObject gridPathfinder;
    [SerializeField] GameObject warningMessage;



    [SerializeField] GameObject Player;
    public GameObject mazeGoalTarget;
    public GameObject waypointDot;

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
        
    }


    // Update is called once per frame
    void Update()
    {
        // TODO change this call method to a button or something. 
        if (Input.GetKey(KeyCode.G))
        {
            RestartGame();
        }

        if (goalIsPlaced)
        {
            gridPathfinder.SetActive(true);
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
        startUI.SetActive(false);
        gameUI.SetActive(true);

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
        gridPathfinder.SetActive(false);
      
    }

    public void ShowWarning()
    {
        // warning has a self-disabling script and will disappear after x time in the script
        warningMessage.SetActive(true);
    }
}
