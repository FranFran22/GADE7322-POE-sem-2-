using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private float maxValue; //--> noise value
    [SerializeField]
    private float minValue; //--> noise value

    private float[,] noiseArray = new float[50,50];
    private Vector3[] newVertices;
    private Vector2[] newUV;
    private int[] newTriangles;



    void Start()
    {
        NoiseGeneration();
    }


    void Update()
    {
        
    }

    private void NoiseGeneration()
    {
        for (int i = 0; i < noiseArray.Length; i++)
        {
            for (int j = 0; j < noiseArray.Length; j++)
            {
                float sample = Mathf.PerlinNoise(maxValue, minValue);
                noiseArray[i, j] = sample;
            }
        }
    }

    private Mesh GenerateMesh()
    {
        Mesh terrain = new Mesh();
        GetComponent<MeshFilter>().mesh = terrain;

        terrain.vertices = newVertices;
        terrain.uv = newUV;
        terrain.triangles = newTriangles;

        return terrain;
    }
}
