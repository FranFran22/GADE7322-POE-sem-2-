using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private int width;

    [SerializeField]
    private int depth;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private int seeds;

    public static Wave[] waveSeeds;
    private float[] frequencies = new float[] { 0.25f, 0.5f, 1f };
    private static int numOfSpawns = 3;
    public static GameObject[] enemySpawns = new GameObject[numOfSpawns];
    public GameObject[] tiles = new GameObject[9]; //change manually !!


    void Start()
    {
        waveSeeds = GenerateSeeds();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

        int tileX = (int)tileSize.x;
        int tileZ = (int)tileSize.z;

        int x = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + i * tileX, this.gameObject.transform.position.y, this.gameObject.transform.position.z + j * tileZ);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                tiles[x] = tile;
                x++;
            }
        }

        GenerateSpawnPoints();
        Debug.Log("Level generated");
    }

    private Wave[] GenerateSeeds()
    {
        int seed = 1000;

        Wave[] gameSeeds = new Wave[seeds];

        for (int i = 0; i < gameSeeds.Length; i++)
        {
            gameSeeds[i] = new Wave();

            //random frequency
            float R = Random.Range(0, 3);
            gameSeeds[i].frequency = frequencies[(int)R];

            //random amplitude
            float r = Random.Range(1, 6);
            gameSeeds[i].amplitude = r;

            gameSeeds[i].seed = seed;

            //Debug.Log(gameSeeds[i].seed);
            seed++;
        }

        return gameSeeds;
    }

    //Enemy Spawning
    private void GenerateSpawnPoints() //generate enemy spawns
    {
        int i = 0;
        GameObject obj = new GameObject();

        foreach (GameObject spawn in enemySpawns)
        {
            enemySpawns[i] = Instantiate(obj, GeneratePosition(), Quaternion.identity);
            enemySpawns[i].name = "Spawn";
            enemySpawns[i].transform.tag = "Enemy Spawn";
            i++;
        }
    }

    private Vector3 GeneratePosition()
    {
        Vector3 posVector;
        float rndmNum;
        float randm = Random.Range(0, 4);
        
        switch (randm)
        {
            case 0: //left edge
                rndmNum = Random.Range(-5, 26);
                posVector = new Vector3(rndmNum, 0.1f, 25);
                break;

            case 1: // top edge
                rndmNum = Random.Range(-5, 26);
                posVector = new Vector3(25, 0.1f, rndmNum);
                break;

            case 2: // right edge
                rndmNum = Random.Range(-5, 26);
                posVector = new Vector3(rndmNum, 0.1f, -5);
                break;

            case 3: // bottom edge
                rndmNum = Random.Range(-5, 26);
                posVector = new Vector3(-5, 0.1f, rndmNum);
                break;

            default:
                posVector = Vector3.zero;
                break;
        }

        return posVector;
    }

}
