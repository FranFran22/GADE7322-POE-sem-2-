using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.UIElements;

// Game only starts when the tower is placed
// Player will have 30 seconds to place the tower, otherwise it will be randomly spawned

public class GameManager : MonoBehaviour
{
    #region GAME OBJECTS
    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3;

    [SerializeField]
    public GameObject[] vertexArray;

    [SerializeField]
    public Tower towerUnit;

    [SerializeField]
    private GameObject GameOverCanvas;

    public Defender[] defenders;

    private GameObject[] enemySpawnPoints;
    //public Unit[] enemies = new Unit[15]; //need to remove
    public List<GameObject>[] paths = new List<GameObject>[3];
    public GameObject tower;
    #endregion

    #region ENEMIES
    [SerializeField]
    public List<Enemy> tier1Enemies = new List<Enemy>();
    [SerializeField]
    public List<Enemy2> tier2Enemies = new List<Enemy2>();
    [SerializeField]
    public List<Enemy3> tier3Enemies = new List<Enemy3>();
    #endregion

    public bool towerPlaced;
    private float timer;
    private float timeDuration = 0;

    [SerializeField]
    public bool canSpawn;

    public bool pathsCreated;
    private int numEnemies;
    public int towerHealth;
    public bool gameOver;
    public bool phaseTwo;
    public bool phaseThree;
    public bool phaseFour;

    private int spawnInterval;
    private int spawnGroup;
    private int enemyTypesToSpawn;
    private int numEnemies1, numEnemies2, numEnemies3;

    public enum Difficulty { Easy, Intermediate, Hard, Difficult  };
    public Difficulty difficulty;

    private enum EnemyType { tier1, tier2, tier3 };
    private EnemyType enemyType;

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
        numEnemies1 = 0;
        numEnemies2 = 0;
        numEnemies3 = 0;

        canSpawn = false;
        gameOver = false;
        phaseTwo = false;
        difficulty = Difficulty.Easy;

        LevelGeneration LG = gameObject.GetComponent<LevelGeneration>();
        enemySpawnPoints = LevelGeneration.enemySpawns;
    }


    void Update() 
    {
        timer += Time.deltaTime; 
        UpdateTimerDisplay(timer);
        tower = GameObject.FindGameObjectWithTag("Tower");

        DifficultyCheck(difficulty);
        
        //Tower Checks
        if (tower != null)
        {
            towerPlaced = true;

            GameStateManager(difficulty);
            //timeDuration = 180;
            TowerController TC = tower.GetComponent<TowerController>();
            towerHealth = TC.currentHealth;

            //check tower health value
            if (towerHealth <= 0 && phaseTwo == true)
            {
                gameOver = true;
                GameOverCanvas.SetActive(true);
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

    private GameObject RandomSpawnPoint(GameObject[] array)
    {
        float x = UnityEngine.Random.Range(0, array.Length);
        return array[(int)x];
    }

    private void SpawnEnemy(EnemyType type)
    {
        List<GameObject> temp = new List<GameObject>();
        float randomNum;

        switch (type)
        {
            case EnemyType.tier1:
                GameObject enemy1 = Instantiate(enemyPrefab1, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);

                //create enemy object
                randomNum = UnityEngine.Random.Range(0, paths.Length);
                temp = paths[(int)randomNum];
                Enemy newEnemy = new Enemy(enemy1, temp);
                enemy1.transform.tag = "Enemy";

                tier1Enemies.Add(newEnemy);

                numEnemies1++;
                numEnemies++;
                Debug.Log("Enemy " + numEnemies + " of 15 spawned");

                break;


            case EnemyType.tier2:
                GameObject enemy2 = Instantiate(enemyPrefab2, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);

                //create enemy object
                randomNum = UnityEngine.Random.Range(0, paths.Length);
                temp = paths[(int)randomNum];
                Enemy2 newEnemy2 = new Enemy2(enemy2, temp);
                enemy2.transform.tag = "Enemy";

                tier2Enemies.Add(newEnemy2);

                numEnemies2++;
                numEnemies++;
                Debug.Log("Enemy " + numEnemies + " of 15 spawned");

                break;


            case EnemyType.tier3:
                GameObject enemy3 = Instantiate(enemyPrefab3, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);

                //create enemy object
                randomNum = UnityEngine.Random.Range(0, paths.Length);
                temp = paths[(int)randomNum];
                Enemy3 newEnemy3 = new Enemy3(enemy3, temp);
                enemy3.transform.tag = "Enemy";

                tier3Enemies.Add(newEnemy3);

                numEnemies3++;
                numEnemies++;
                Debug.Log("Enemy " + numEnemies + " of 15 spawned");

                break;


            default:
                break;
        }


    }

    private void DifficultyCheck(Difficulty dif)
    {
        //adjust values for each game state

        switch (dif)
        {
            case Difficulty.Easy:
                spawnGroup = 1;
                spawnInterval = 5;
                enemyTypesToSpawn = 1;
                break;

            case Difficulty.Intermediate:
                spawnGroup = 3;
                spawnInterval = 5;
                enemyTypesToSpawn = 2;
                break;

            case Difficulty.Hard:
                spawnGroup = 3;
                spawnInterval = 2;
                enemyTypesToSpawn = 2;
                break;

            case Difficulty.Difficult:
                spawnGroup = 3;
                spawnInterval = 2;
                enemyTypesToSpawn = 3;
                break;

            default:
                spawnGroup = 1;
                spawnInterval = 5;
                enemyTypesToSpawn = 1;
                break;
        }
    }

    private float RandomNum(int min, int max)
    {
        float value = UnityEngine.Random.Range(min, max + 1);
        return value;
    }

    #region COROUTINES
    private IEnumerator spawnEasy()
    {
        //spawn only tier 1

        while (numEnemies1 < 15)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy(EnemyType.tier1);
            numEnemies1++;
            Debug.Log(numEnemies1);
        }

    }

    private IEnumerator spawnIntermediate()
    {
        //spawn tier 1 and 2

        while ((numEnemies1 + numEnemies2) < 30)
        {
            for (int i = 0; i < spawnGroup; i++)
            {
                float val = RandomNum(1, 2);
                yield return new WaitForSeconds(spawnInterval);

                if (val == 1)
                {
                    SpawnEnemy(EnemyType.tier1);
                    numEnemies1++;
                }

                else
                {
                    SpawnEnemy(EnemyType.tier2);
                    numEnemies2++;
                }
            }

        }

    }

    private IEnumerator spawnHard()
    {
        //spawn tier 1 and 2

        while ((numEnemies1 + numEnemies2) < 50)
        {
            float val = RandomNum(1, 2);
            yield return new WaitForSeconds(spawnInterval);

            if (val == 1)
            {
                SpawnEnemy(EnemyType.tier1);
                numEnemies1++;
            }

            else
            {
                SpawnEnemy(EnemyType.tier2);
                numEnemies2++;
            }

        }

    }

    private IEnumerator spawnDifficult()
    {
        //spawn all tiers

        while ((numEnemies1 + numEnemies2 + numEnemies3) < 60)
        {
            float val = RandomNum(1, 3);
            yield return new WaitForSeconds(spawnInterval);

            if (val == 1)
            {
                SpawnEnemy(EnemyType.tier1);
                numEnemies1++;
            }

            else if (val == 2)
            {
                SpawnEnemy(EnemyType.tier2);
                numEnemies2++;
            }

            else
            {
                SpawnEnemy(EnemyType.tier3);
                numEnemies3++;
            }

        }
    }
    #endregion

    private void GameStateManager(Difficulty dif)
    {
        //start coroutines
        //stop coroutines
        //check enemies being spawned
        //change game state when parameters are met:
        //  --> intermediate starts when the player has killed all the tier 1 enemies
        //  --> hard starts when the player has killed all the tier 1 and 2 enemies
        //  --> difficult will ONLY start if the tower is >75 health and all remaining enemies have been killed

        if (dif == Difficulty.Easy)
        {
            if (canSpawn)
            {
                StartCoroutine(spawnEasy());
                Debug.Log("Tier 1 enemies spawning");
                canSpawn = false;
            }

            //Debug.Log("Changed difficulty to: Easy");
        }

        if(tier1Enemies.Count == 0 && phaseTwo && dif == Difficulty.Easy)
        {
            StopCoroutine(spawnEasy());

            difficulty = Difficulty.Intermediate;

            if (phaseTwo)
                canSpawn = true;

            if (canSpawn)
            {
                StartCoroutine(spawnIntermediate());
                Debug.Log("Tier 1 & 2 enemies spawning");
                canSpawn = false;
            }

            //Debug.Log("Changed difficulty to: Intermediate");
        }

        if (tier1Enemies.Count == 0 && phaseThree && tier2Enemies.Count == 0 && dif == Difficulty.Intermediate)
        {
            StopCoroutine(spawnIntermediate());

            difficulty = Difficulty.Hard;

            if (canSpawn)
            {
                StartCoroutine(spawnHard());
                Debug.Log("Tier 1 & 2 enemies spawning (2.0)");
                canSpawn = false;
            }

            //Debug.Log("Changed difficulty to: Hard");
        }

        if (tier1Enemies.Count == 0 && phaseFour && tier2Enemies.Count == 0 && tier3Enemies.Count == 0 && dif == Difficulty.Hard)
        {
            StopCoroutine(spawnHard());

            difficulty = Difficulty.Difficult;

            if (canSpawn)
            {
                StartCoroutine(spawnDifficult());
                Debug.Log("Tier 1, 2 and 3 enemies spawning");
                canSpawn = false;
            }

            //Debug.Log("Changed difficulty to: Difficult");
        }
    }
}
