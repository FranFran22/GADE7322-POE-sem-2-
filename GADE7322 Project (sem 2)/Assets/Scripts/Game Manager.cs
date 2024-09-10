using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] enemySpawnPoints;
    private GameObject[] defenderSpawnPoints = new GameObject[3];
    public GameObject Tower;

    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject defenderPrefab;

    [SerializeField]
    private GameObject towerPrefab;

    private enum UnitType { Tower, Enemy, Defender };
    private UnitType unitType;

    private int timer; //game timer (wip)
    private bool towerPlaced;


    void Start()
    {
        towerPlaced = false;
        GetComponent<LevelGeneration>();
        enemySpawnPoints = LevelGeneration.enemySpawns;

        // need a check for if the tower has been placed
        CalculateDefenderSpawn();
        StartCoroutine(SpawnEnemy());
    }


    void Update()
    {
        
    }


    public void Timer()
    {
        //needs to run during game time --> stops when game is paused
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
