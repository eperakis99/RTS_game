using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public delegate void GameStateAction(GameState state);
    public static event  GameStateAction OnGameStateChanged;

    public Mesh stage; //the stage mesh
    [SerializeField] public NavMeshSurface navmesh_humanoid;
    private float time;
    private bool isBuilt;
    private bool isPlaying;
    private int enemiesToEnd;
    private int unitsToEnd;

    private void Awake()
    {
        instance = this;
        gameState = GameState.start;
    }

    private void Start()
    {
        stage = TerrainGenerator.mesh;

        time = 2f;
        isBuilt = false;
        isPlaying = false;

        enemiesToEnd = EnemyManager.instance.EnemyDictionary.Count;
        unitsToEnd = UnitManager.instance.FightersList.Count + UnitManager.instance.FlyingList.Count;

    }

    private void Update()
    {
        enemiesToEnd = EnemyManager.instance.EnemyDictionary.Count;
        unitsToEnd = UnitManager.instance.FightersList.Count + UnitManager.instance.FlyingList.Count;

        if (!isBuilt) StartCoroutine(rebuildNavMesh());

        if (enemiesToEnd == 0 && unitsToEnd >= 15 && !isPlaying)
            UpdateGameState(GameState.playing);
        else if (enemiesToEnd == 0 && unitsToEnd >= 15 && isPlaying)
            UpdateGameState(GameState.end);

        if (!isPlaying && enemiesToEnd > 0) isPlaying = true;
        
    }

    public IEnumerator rebuildNavMesh()
    {

        isBuilt = true;

        while (time > 0f) //postpone the build of the navmeshs
        {
            time -= Time.deltaTime;
            yield return null;
            
        }

        navmesh_humanoid.BuildNavMesh();
        time = 4f;
        //UnitManager.instance.createUnit(new Vector3(5f, 1.2f, 5f), Quaternion.identity);
        //isBuilt = false;
    }


    public void UpdateGameState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.start:
                break;
            case GameState.playing:
                break;
            case GameState.end:
                rollCredits();
                break;
            default:
                throw new UnityException();
                    
        }

    }


    public void rollCredits()
    {
        SceneManager.LoadScene(2);
    }

}



public enum GameState
{
    start,
    playing,
    end

}
