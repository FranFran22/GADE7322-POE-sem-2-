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
    private GameObject[] enemySpawns = new GameObject[numOfSpawns];
    private GameObject[] tiles = new GameObject[9]; //change manually !!
    private Vector3[] vertices;


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

        int index = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + i * tileX, this.gameObject.transform.position.y, this.gameObject.transform.position.z + j * tileZ);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                tiles[index++] = tile;
            }
        }

        vertices = GetVertices(tiles);
        GenerateSpawnPoints();
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

    private void GenerateSpawnPoints() //generate enemy spawns
    {
        int i = 0;
        GameObject obj = new GameObject();

        foreach (GameObject spawn in enemySpawns)
        {
            enemySpawns[i] = Instantiate(obj, GeneratePosition(vertices), Quaternion.identity);
            enemySpawns[i].name = "Spawn";
            i++;
        }
    }

    private Vector3 GeneratePosition(Vector3[] vertices)
    {
        //generate positions for spawn points

        return Vector3.zero;
    }

    private Vector3[] GetVertices(GameObject[] array)
    {
        Vector3[] vertices = new Vector3[array.Length];
        Vector3[] temp;

        foreach (GameObject t in array)
        {
            MeshFilter meshFilter = t.GetComponent<MeshFilter>();

            if (vertices == null)
            {
                vertices = meshFilter.mesh.vertices;
            }

            else if (vertices != null)
            {
                temp = meshFilter.mesh.vertices;
                vertices = vertices.Concat(temp).ToArray();

                foreach (Vector3 v in temp)
                    Debug.Log(v);
            }
        }
        
        
        return vertices;
    }

}
