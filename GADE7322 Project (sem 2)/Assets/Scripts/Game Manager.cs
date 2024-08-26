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

    private int timer; //game timer (wip)


    void Start()
    {
        Vector3[] temp = GetComponent<TerrainGenerator>().newVertices;
        vertices = temp;

        GetComponent<Enemy>();
        GetComponent<Defender>();

        //enemy spawning
        GenerateSpawnPoints();
        StartCoroutine(eSpawner());

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


    private void SpawnEnemy()
    {
        float x = Random.Range(0, enemySpawnPoints.Length - 1);

        Enemy enemy = new Enemy(enemyPrefab, enemySpawnPoints[(int) x]);

    }

    private IEnumerator eSpawner()
    {
        while (timer < 30) //only spawn after 30 seconds
        {
            SpawnEnemy();
            yield return new WaitForSeconds(3f); //spawn ever 3 seconds
        }
    }
    #endregion
}
