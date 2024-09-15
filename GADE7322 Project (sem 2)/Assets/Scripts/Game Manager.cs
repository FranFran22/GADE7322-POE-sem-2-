using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;

// Game only starts when the tower is placed
// Player will have 30 seconds to place the tower, otherwise it will be randomly spawned

public class GameManager : MonoBehaviour
{
    #region GAME OBJECTS
    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    public GameObject[] vertexArray; 

    private GameObject[] enemySpawnPoints;
    public Enemy[] enemies = new Enemy[15];
    public List<GameObject>[] paths = new List<GameObject>[3];
    private GameObject tower;
    #endregion

    public bool towerPlaced;
    private float timer;
    private float timeDuration = 90;

    [SerializeField]
    private bool canSpawn;

    public bool pathsCreated;
    private int numEnemies;

    #region TIMER OBJECTS
    [SerializeField]
    private TextMeshProUGUI minute1;

    [SerializeField]
    private TextMeshProUGUI minute2;

    [SerializeField]
    private TextMeshProUGUI seperator;

    [SerializeField]
    private TextMeshProUGUI second1;

    [SerializeField]
    private TextMeshProUGUI second2;
    #endregion

    void Awake()
    {
        
    }

    void Start()
    {
        //Initialisation
        ResetTimer();
        towerPlaced = false;
        pathsCreated = false;
        numEnemies = 0;

        LevelGeneration LG = gameObject.GetComponent<LevelGeneration>();
        enemySpawnPoints = LevelGeneration.enemySpawns;
    }


    void Update()
    {
        timer -= Time.deltaTime; //countdown
        UpdateTimerDisplay(timer);
        tower = GameObject.FindGameObjectWithTag("Tower");

        //Tower Checks
        if (tower != null)
        {
            towerPlaced = true;

            if (pathsCreated == false)
            {
                Debug.Log("Creating paths ...");
                CreatePaths();
                AssignPaths();
                pathsCreated = true;
            }     
        }
        
        if (towerPlaced == true)
        {
            canSpawn = true;
            timeDuration = 120;  
        }

        //Enemy Spawning Checks
        if (numEnemies > 15)
            canSpawn = false;

        if (canSpawn)
        {
            if (numEnemies < 16)
            {
                StartCoroutine(SpawnEnemy());
            }
        }


    }


    private void UpdateTimerDisplay(float time) 
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        minute1.text = currentTime[0].ToString();
        minute2.text = currentTime[1].ToString();
        second1.text = currentTime[2].ToString();
        second2.text = currentTime[3].ToString();
    }

    private void ResetTimer()
    {
        timer = timeDuration;
    }

    private void SpawnUnit()
    {
        GameObject enemy = Instantiate(enemyPrefab, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);
        enemy.transform.tag = "Enemy";

        List<GameObject> temp = new List<GameObject>();
        Enemy newEnemy = new Enemy(enemy, temp);

        enemies[numEnemies-1] = newEnemy;

        Debug.Log("Enemy " + numEnemies + " of 15 spawned");
        numEnemies++;
    }

    private GameObject RandomSpawnPoint(GameObject[] array)
    {
        float x = UnityEngine.Random.Range(0, array.Length);
        return array[(int)x];
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            yield return new WaitForSeconds(5);
            SpawnUnit();
        }   
    }

    private void CreatePaths()
    {
        Vector3 startPosition;
        Vector3 endPosition;
        float posX = 0;
        float posZ = 0;

        int i = 0; //path index
        foreach (GameObject spawn in enemySpawnPoints)
        {
            List<GameObject> holder = new List<GameObject>(); //creates a new list to store the pathing waypoints
            startPosition = spawn.transform.position;
            endPosition = new Vector3(tower.transform.position.x-2, tower.transform.position.y, tower.transform.position.z-2);

            for (int x = 0; x < 5; x++) //generate the waypoints
            {
                if (x > 0)
                    startPosition = holder[x - 1].transform.position;

                posX = UnityEngine.Random.Range(startPosition.x, endPosition.x);
                posZ = UnityEngine.Random.Range(startPosition.z, endPosition.z);

                GameObject temp = new GameObject();
                temp.transform.position = new Vector3(posX, spawn.transform.position.y, posZ);

                temp.transform.AddComponent<BoxCollider>();
                BoxCollider c = temp.transform.GetComponent<BoxCollider>();
                c.size = new Vector3(0.1f, 0.1f, 0.1f);

                c.isTrigger = true;
                temp.transform.tag = "Waypoint";

                holder.Add(temp);
            }

            //Debug.Log(holder.Count);

            paths[i] = new List<GameObject>();
            paths[i].AddRange(holder);
            i++;
        }

        pathsCreated = true;
    }

    private void AssignPaths()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            float randomNum = UnityEngine.Random.Range(0, paths.Length);
            //Debug.Log(paths[(int)randomNum].Count);

            enemies[i].waypointList = paths[(int)randomNum];
        }
    }
}
