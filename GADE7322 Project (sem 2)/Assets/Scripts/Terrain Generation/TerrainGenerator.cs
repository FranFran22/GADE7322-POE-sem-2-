using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

public class TerrainGenerator : MonoBehaviour
{
    // PROCEDURE
    // heightmap [done]
    // create mesh --> using heightmap
    // produce vertices from the mesh
    // supply UV coordinates (for texturing)
    // add triangles to produce the mesh (using temp arrays)

    // NOTES
    // unity3D mesh --> MeshFilter (on a GameObject) --> Meshrenderer
    // heightmap source is a function


    private static int size = 11;

    private float[,] noiseArray = new float[size, size];
    public static Vector3[] newVertices = new Vector3[size * size];
    private int scalingFactor = 3;
    private float heightMultiplier = 3;
    private MeshRenderer mRenderer;
    private MeshFilter mFilter;
    private MeshCollider mCollider;

    [SerializeField]
    private TerrainType[] terrainTypes;

    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private AnimationCurve heightCurve;


    void Start()
    {
        mFilter = GetComponent<MeshFilter>();
        mCollider = GetComponent<MeshCollider>();
        mRenderer = GetComponent<MeshRenderer>();

        GetComponent<LevelGeneration>();
        waves = LevelGeneration.waveSeeds;

        GenerateMesh();
    }


    void Update()
    {
        
    }


    private float[,] NoiseGeneration(float offsetX, float offsetZ, Wave[] waves)
    {
        float[,] map = new float[size, size];

        for (int h = 0; h < size; h++)
        {
            for (int w = 0; w < size; w++)
            {
                float sampleX = (w+ offsetX) / scalingFactor;
                float sampleZ = (h + offsetZ) / scalingFactor;

                float noise = 0f;
                float normalisation = 0f;

                foreach(Wave wave in waves) //generates noise value for each wave
                {
                    noise+= wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                    normalisation += wave.amplitude;
                }

                noise /= normalisation; //normalise the noise value to be between 0 and 1 (for heightmap)

                map[h, w] = noise;
                //Debug.Log(noise);
            }
        }

        return map;
    }


    private void GenerateMesh() //Tile generation
    {
        newVertices = mFilter.mesh.vertices; //redundant ??

        //offsets for tile edges
        float offsetX = -gameObject.transform.position.x;
        float offsetZ = -gameObject.transform.position.z;

        //heightmap generation (Perlin noise)
        noiseArray = NoiseGeneration(offsetX, offsetZ, waves);

        Texture2D tileTexture = BuildTexture(noiseArray);
        mRenderer.material.mainTexture = tileTexture;

        //update the vertices according to the noise map
        UpdateVertices(noiseArray);

    }

    private void UpdateVertices(float[,] heightmap)
    {
        Vector3[] vertices = mFilter.mesh.vertices;

        int index = 0;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float height = heightmap[i, j];
                Vector3 vertex = vertices[index];
                vertices[index] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier, vertex.z); //changes the y value 

                index++;
            }
        }

        //update the vertices & mesh
        mFilter.mesh.vertices = vertices;
        mFilter.mesh.RecalculateBounds();
        mFilter.mesh.RecalculateNormals();
        mCollider.sharedMesh = mFilter.mesh;

    }

    private Texture2D BuildTexture(float[,] map)
    {
        int depth = size;
        int width = size;

        Color[] colourMap = new Color[depth * width];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x ++)
            {
                int colourIndex = z * width + x;
                float height = map[z, x];
                TerrainType terrainType = ChooseTerrainType(height);
                colourMap[colourIndex] = terrainType.colour;
            }
        }

        Texture2D tileTexture = new Texture2D(width, depth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colourMap);
        tileTexture.Apply();

        return tileTexture;
    }

    private TerrainType ChooseTerrainType(float height)
    {
        foreach(TerrainType terrainType in terrainTypes)
        {
            if (height < terrainType.height)
                return terrainType;
        }

        return terrainTypes[terrainTypes.Length - 1];
    }

}
