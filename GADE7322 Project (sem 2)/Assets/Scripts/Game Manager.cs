using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemySpawnPoints;
    [SerializeField]
    private GameObject enemyTarget;

    public static GameObject enemyPrefab;
    public static GameObject defenderPrefab;

    private int count;
    private int speed;

    private int terrainSize = 100;
    private Vector3[] vertices;
    private int numOfEnemies;


    void Start()
    {
        Vector3[] temp = GetComponent<TerrainGenerator>().newVertices;
        vertices = temp;

        GetComponent<Enemy>();
        GetComponent<Defender>();

        //enemy spawning
        GenerateSpawnPoints();

    }


    void Update()
    {
        
    }


    #region ENEMIES
    private void GenerateSpawnPoints()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject spawn = new GameObject();

            spawn.transform.position = GenerateCoords();
        }
    }

    private Vector3 GenerateCoords()
    {
        float x = Random.Range(0, vertices.Length / 2); //choose a random vertex from the array
        Vector3 val = vertices[(int) x];

        return val;
    }


    private void SpawnEnemies()
    {
        float x = Random.Range(0, enemySpawnPoints.Length - 1);


        Enemy enemy = new Enemy(enemyPrefab, enemySpawnPoints[(int) x]);

    }
    #endregion
}
