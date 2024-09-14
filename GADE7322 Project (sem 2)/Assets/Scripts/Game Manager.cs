using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

// Game only starts when the tower is placed
// Player will have 30 seconds to place the tower, otherwise it will be randomly spawned

public class GameManager : MonoBehaviour
{
    private GameObject[] enemySpawnPoints;
    private GameObject[] defenderSpawnPoints = new GameObject[3];
    public GameObject Tower;

    #region GAME OBJECTS
    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject defenderPrefab;

    [SerializeField]
    private GameObject towerPrefab;
    #endregion

    private enum UnitType { Tower, Enemy, Defender };
    private UnitType unitType;

    private bool towerPlaced;
    private float timer;
    private float timeDuration = 90;

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

        GetComponent<LevelGeneration>();
        enemySpawnPoints = LevelGeneration.enemySpawns;

    }


    void Update()
    {
        timer -= Time.deltaTime; //countdown
        UpdateTimerDisplay(timer);

        //need to put tower-check code here
        if (towerPlaced == true)
        {
            CalculateDefenderSpawn();
            StartCoroutine(SpawnEnemy());
            towerPlaced = false;
            timeDuration = 120;
            ResetTimer();
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

    private void SpawnUnit(UnitType type, Vector3 value)
    {
        switch (type)
        {
            case UnitType.Tower:
                GameObject tower = Instantiate(towerPrefab, value, Quaternion.identity);
                Tower = tower;
                tower.transform.tag = "Tower";
                towerPlaced = true;
                break;

            case UnitType.Enemy:
                GameObject enemy = Instantiate(enemyPrefab, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);
                enemy.transform.tag = "Enemy";
                break;

            case UnitType.Defender:
                GameObject defender = Instantiate(defenderPrefab, RandomSpawnPoint(defenderSpawnPoints).transform.position, Quaternion.identity);
                defender.transform.tag = "Defender";
                break;

            default:
                break;
        }
    }

    private GameObject RandomSpawnPoint(GameObject[] array)
    {
        float x = UnityEngine.Random.Range(0, array.Length);
        return array[(int)x];
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        int count = 0;

        while (count < 10)
        {
            yield return new WaitForSeconds(3);
            SpawnUnit(UnitType.Enemy, Vector3.zero);

            count++;
        }
    }

    private IEnumerator SpawnDefender()
    {
        SpawnUnit(UnitType.Defender, Vector3.zero);
        yield return new WaitForSeconds(0);
    }

    public void CallDefenders()
    {
        StartCoroutine(SpawnDefender());
    }

    private void CalculateDefenderSpawn()
    {
        // spawn a defender within a 3 unit radius of the tower
        for (int i = 0; i < defenderSpawnPoints.Length; i++)
        {
            defenderSpawnPoints[i] = new GameObject();
        }

        defenderSpawnPoints[0].transform.position = new Vector3(Tower.transform.position.x + 3, Tower.transform.position.y, Tower.transform.position.z);
        defenderSpawnPoints[1].transform.position = new Vector3(Tower.transform.position.x - 3, Tower.transform.position.y, Tower.transform.position.z);
        defenderSpawnPoints[2].transform.position = new Vector3(Tower.transform.position.x, Tower.transform.position.y, Tower.transform.position.z + 3);
        defenderSpawnPoints[3].transform.position = new Vector3(Tower.transform.position.x, Tower.transform.position.y, Tower.transform.position.z - 3);
    }

    public void PlaceTower()
    {
        Vector3 position = new Vector3(10, 0.1f, 10);
        SpawnUnit(UnitType.Tower, position);
    }
}
