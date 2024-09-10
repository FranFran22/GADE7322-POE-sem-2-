using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemySpawnPoints;

    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private int numEnemySpawns;

    public static GameObject enemyPrefab;
    public static GameObject defenderPrefab;

    private int count;
    private int speed;

    private int terrainSize = 100;
    private Vector3[] vertices;
    private int numOfEnemies;

    private int timer; //game timer (wip)


    void Start()
    {

    }


    void Update()
    {
        
    }


    public void Timer()
    {
        //needs to run during game time --> stops when game is paused
    }

    #region DEFENDERS
    private void SpawnDefender()
    {
        // when the defender icon is clicked --> defender is placed
        // random spawn location near the tower
    }

    


    #endregion
}
