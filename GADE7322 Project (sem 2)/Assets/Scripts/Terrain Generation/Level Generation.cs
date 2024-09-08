using System.Collections;
using System.Collections.Generic;
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

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + i * tileX, this.gameObject.transform.position.y, this.gameObject.transform.position.z + j * tileZ);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
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

}
