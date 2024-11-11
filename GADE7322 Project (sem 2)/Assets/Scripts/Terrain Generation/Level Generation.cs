using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    public List<float[,]> heightMaps = new List<float[,]>();
    private List<Vector3[]> newVertices = new List<Vector3[]>();
    private Vector3[] vertices = new Vector3[121 * 9];
    public GameObject[] vertexObjects = new GameObject[121 * 9];

    void Start()
    {
        waveSeeds = GenerateSeeds();
        GenerateLevel();

        GenerateVertices();

        GenerateSpawnPoints();
        Debug.Log("Level generated");
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

                //Debug.Log("spawned tile " + i + j);

                tiles[x] = tile;
                x++;
            }
        }
    }

    private Wave[] GenerateSeeds()
    {
        int seed = 1000;

        Wave[] gameSeeds = new Wave[seeds];

        for (int i = 0; i < gameSeeds.Length; i++)
        {
            gameSeeds[i] = new Wave();

            //random frequency
            float R = UnityEngine.Random.Range(0, 3);
            gameSeeds[i].frequency = frequencies[(int)R];

            //random amplitude
            float r = UnityEngine.Random.Range(1, 6);
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
            enemySpawns[i] = Instantiate(obj, GeneratePosition(), Quaternion.identity);
            enemySpawns[i].name = "Spawn";
            enemySpawns[i].transform.tag = "Enemy Spawn";
            i++;
        }
    }

    private Vector3 GeneratePosition()
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        Vector3 posVector = Vector3.zero;

        foreach (GameObject obj in vertexObjects)
        {
            if ((obj.transform.position.z == 25) || (obj.transform.position.z == -5) || (obj.transform.position.x == 25) || (obj.transform.position.x == -5))
            {
                spawnPositions.Add(obj.transform.position);
            }
        }

        float rndmNum = UnityEngine.Random.Range(0, spawnPositions.Count);
        List<Vector3> shuffled = spawnPositions.OrderBy(_ => rndmNum).ToList();

        for (int i = 0; i < 3; i++)
        {
            float randm = UnityEngine.Random.Range(0, spawnPositions.Count);
            posVector = spawnPositions[(int)randm];

        }
        
        return posVector;
    }

    private void GenerateVertices()
    {
        //tile by tile, adjust the vertices

        foreach (GameObject tile in tiles)
        {
            Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
            Vector3[] temp = mesh.vertices;
            Vector3[] nVertices = new Vector3[121];


            for (int i = 0; i < temp.Length; i++)
            {
                //convert vertex positions to world space
                nVertices[i] = transform.TransformPoint(temp[i]);
            }

            TerrainGenerator TG = tile.GetComponent<TerrainGenerator>();
            int index = Array.IndexOf(tiles, tile);
            float[,] noise = TG.noiseArray;
            int x = 0;

            switch (index)
            {
                case 0:              
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x].y = noise[i, j];
                            x++;
                        }
                        
                    }
                    newVertices.Add(nVertices);
                   // Debug.Log("tile 0's vertices corrected");
                    break;

                case 1:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x, noise[i, j], nVertices[x].z + 10);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 1's vertices corrected");
                    break;

                case 2:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x, noise[i, j], nVertices[x].z + 20);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 2's vertices corrected");
                    break;

                case 3:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 10, noise[i, j], nVertices[x].z);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 3's vertices corrected");
                    break;

                case 4:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 10, noise[i, j], nVertices[x].z + 10);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 4's vertices corrected");
                    break;

                case 5:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 10, noise[i, j], nVertices[x].z + 20);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 5's vertices corrected");
                    break;

                case 6:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 20, noise[i, j], nVertices[x].z);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 6's vertices corrected");
                    break;

                case 7:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 20, noise[i, j], nVertices[x].z + 10);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 7's vertices corrected");
                    break;

                case 8:
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            nVertices[x] = new Vector3(nVertices[x].x + 20, noise[i, j], nVertices[x].z + 20);
                            nVertices[x].y = noise[i, j];
                            x++;
                        }

                    }
                    newVertices.Add(nVertices);
                    //Debug.Log("tile 8's vertices corrected");
                    break;

                default:
                    break;
            }
       
        }

        //assign vertices to an array
        int p = 0;
        for (int i = 0; i < newVertices.Count; i++)
        {
            for (int j = 0; j < 121; j++)
            {

                Vector3[] temp2 = newVertices[i];

                //Debug.Log(p);
                //Debug.Log(temp2[j]);

                vertices[p] = temp2[j];

                //create a gameobject for the position
                vertexObjects[p] = new GameObject();
                vertexObjects[p].transform.position = vertices[p];
                vertexObjects[p].tag = "Vertex";
                vertexObjects[p].name = "Vertex " + p.ToString();

                vertexObjects[p].transform.AddComponent<BoxCollider>();
                BoxCollider c = vertexObjects[p].transform.GetComponent<BoxCollider>();
                c.size = new Vector3(0.2f, 0.2f, 0.2f);
                c.isTrigger = true;

                //Debug.Log("vertex object " + p + " created");

                p++;
            }
        }

        //Debug.Log("all vertices assigned");
        Debug.Log(vertexObjects.Length);
    }

}
