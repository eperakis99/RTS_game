using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private int enemiesToSpawn = 10;
    private int counter = 0;
    private bool started = false; //Time in seconds after which enemies start spawning

    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public Rigidbody bulletPrefab;

    public GameObject healthbarPrefab;
    public RectTransform healthbarCanvas;

    public Dictionary<int, Enemy> EnemyDictionary { get; private set; }


    void Awake()
    {
        instance = this; 
        counter = 0;
        enemiesToSpawn = 10;
        healthbarCanvas = GameObject.Find("HealthBarsCanvas").GetComponent<RectTransform>();
        EnemyDictionary = new Dictionary<int, Enemy>();
        StartCoroutine(SpawnEnemy(15f, 10f));
    }

    private IEnumerator SpawnEnemy(float initialDelay, float interval)
    {
        if (!started)
        {
            started = true;
            yield return new WaitForSeconds(initialDelay);
        }

        while (counter < enemiesToSpawn)
        {
            createEnemy();
            counter++;
            
            yield return new WaitForSeconds(interval);
            if (counter == enemiesToSpawn) break;
        }
        
    }

    private void OnEnemyDeath(int id)
    {
        Enemy enemy = EnemyDictionary[id];

        enemy.enemyDeath -= OnEnemyDeath; //Unsubscribe from the unitDeath event of the Unit
        Destroy(enemy.healthBar);

        EnemyDictionary.Remove(id); //Remove the dead Unit from the dictionary
        Destroy(enemy.getClone()); //Also destroy the gameobject instance to free space 

    }

    private int createEnemy()
    {
        int id = 0;
        GameObject enemygo = null;
        Vector3 pos = 3 * UnityEngine.Random.onUnitSphere;
        pos.y = 0f;//Enemy should spawn on ground
        pos += this.transform.position + new Vector3(20f, 0f, 20f);//Spawn next to stronghold
        enemygo = Instantiate(enemyPrefab, pos, Quaternion.Euler(0f, -90f, 0f)) as GameObject; //Instantiate a unit



        Enemy enemy = enemygo.AddComponent<Enemy>(); //add the script with all the data to the unit
        enemy.setClone(enemygo); //Set the member variable of the class Unit to the created gameobject
        enemy.bulletPrefab = this.bulletPrefab;
        id = enemygo.GetInstanceID(); //Get the id of the created gameobject 
        enemy.setId(id);
        EnemyDictionary.Add(id, enemy); //add the unit to the dictionary with key its id

        enemy.enemyDeath += OnEnemyDeath; //subscribe to the unitDeath event of the Unit

        Debug.Log(id);

        GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
        healthbar.GetComponent<HealthBar>().SetHealthBarData(enemygo.transform, healthbarCanvas, 80f);
        healthbar.transform.SetParent(healthbarCanvas, false);
        enemy.healthBar = healthbar;

        return id;
    }
}
