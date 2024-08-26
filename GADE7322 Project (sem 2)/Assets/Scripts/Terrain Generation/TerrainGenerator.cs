using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // REFERENCES
    // https://discussions.unity.com/t/mesh-generation-out-of-array/682340
    // https://docs.unity3d.com/ScriptReference/Mesh.html

    // PROCEDURE
    // heightmap [done]
    // create mesh --> using heightmap
    // produce vertices from the mesh
    // supply UV coordinates (for texturing)
    // add triangles to produce the mesh (using temp arrays)

    // NOTES
    // unity3D mesh --> MeshFilter (on a GameObject) --> Meshrenderer
    // heightmap source is a function


    private float maxValue = 1f;
    private float minValue = 0.1f;
    private static int size = 100;

    private float[,] noiseArray = new float[size, size];
    private Vector3[] newVertices = new Vector3[size * size];
    private Vector2[] newUV = new Vector2[size * size];
    private int[] newTriangles = new int[(size - 1) * (size - 1) * 2 * 3];
    private int index;
    private int scalingFactor = 10;



    void Start()
    {
        NoiseGeneration();
        GenerateMesh();
    }


    void Update()
    {
        
    }


    private void NoiseGeneration()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                noiseArray[i, j] = Noise();
            }
        }
    }


    private float Noise()
    {
        float frequency = Random.Range(0, 9);
        frequency = frequency / 10;

        float noiseVal = Mathf.PerlinNoise(maxValue * frequency, minValue * frequency);

        return noiseVal;
    }


    private void GenerateMesh()
    {
        Mesh terrain = new Mesh();
        MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh = terrain;

        Generate();

        terrain.vertices = newVertices;
        terrain.uv = newUV;
        terrain.triangles = newTriangles;
    }


    private void Generate()
    {
        index = 0;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                newVertices[index++] = new Vector3(i, noiseArray[i, j] * scalingFactor, j);
                newUV[index++] = new Vector2(i / (size - 1), j / (size - 1));
            }
        }

        index = 0;

        for (int x = 0; x < size - 1; x++)
        {
            for (int y = 0; y < size - 1; y++)
            {
                // triangle #1
                newTriangles[index++] = x + y * size;
                newTriangles[index++] = x + 1 + y * size;
                newTriangles[index++] = x + 1 + (y + 1) * size;

                // triangle #2
                newTriangles[index++] = x + y * size;
                newTriangles[index++] = x + 1 + (y + 1) * size;
                newTriangles[index++] = x + (y + 1) * size;
            }
        }
    }
}
