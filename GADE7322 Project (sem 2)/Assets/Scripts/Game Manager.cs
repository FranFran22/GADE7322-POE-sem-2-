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
        vertices = mapVertices(); //get the vertices from the terrain map

        //enemy spawning
        GenerateSpawnPoints();
        //StartCoroutine(eSpawner());

    }


    void Update()
    {
        
    }


    public void Timer()
    {
        //needs to run during game time --> stops when game is paused
    }

    private Vector3[] mapVertices()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<TerrainGenerator>();

            if (vertices == null)
                vertices = TerrainGenerator.newVertices;

            else
            {
                Vector3[] temp = TerrainGenerator.newVertices;
                Vector3[] result = vertices.Concat(temp).ToArray();
                vertices = result;
            }
        }

        return vertices;
    }


    #region ENEMIES
    private void GenerateSpawnPoints() //generates enemy spawn points
    {
        for (int i = 0; i < numEnemySpawns; i++)
        {
            GameObject spawn = new GameObject();
            enemySpawnPoints[i] = Instantiate(spawn, GenerateCoords(), Quaternion.identity);
        }
    }

    private Vector3 GenerateCoords()
    {
        // takes random vertex and generates coords along the map edge
        float i = Random.Range(0, vertices.Length);
        float j = Random.Range(0, 2);

        Vector3 val;

        if (j == 1)
            val = new Vector3(0.1f, vertices[(int)i].y, vertices[(int)i].z);

        val = new Vector3(vertices[(int)i].x, vertices[(int)i].y, 0.1f);

        Debug.Log(val);
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

    #region DEFENDERS
    private void SpawnDefender()
    {
        // when the defender icon is clicked --> defender is placed
        // random spawn location near the tower
    }

    


    #endregion
}
