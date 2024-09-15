using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

// Game only starts when the tower is placed
// Player will have 30 seconds to place the tower, otherwise it will be randomly spawned

public class GameManager : MonoBehaviour
{
    #region GAME OBJECTS
    [SerializeField]
    private GameObject enemyTarget;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    public GameObject[] vertexArray; 

    private GameObject[] enemySpawnPoints;
    #endregion

    public bool towerPlaced;
    private float timer;
    private float timeDuration = 90;
    private bool vertexesAssigned;
    private bool canSpawn;
    private int enemyCount;


    #region TIMER OBJECTS
    [SerializeField]
    private TextMeshProUGUI minute1;

    [SerializeField]
    private TextMeshProUGUI minute2;

    [SerializeField]
    private TextMeshProUGUI seperator;

    [SerializeField]
    private TextMeshProUGUI second1;

    [SerializeField]
    private TextMeshProUGUI second2;
    #endregion

    void Awake()
    {
        
    }

    void Start()
    {
        //Initialisation
        ResetTimer();
        towerPlaced = false;
        vertexesAssigned = false;

        LevelGeneration LG = gameObject.GetComponent<LevelGeneration>();
        enemySpawnPoints = LevelGeneration.enemySpawns;


    }


    void Update()
    {
        timer -= Time.deltaTime; //countdown
        UpdateTimerDisplay(timer);

        GameObject tower = GameObject.FindGameObjectWithTag("Tower");

        if (tower != null)
            towerPlaced = true;

        if (vertexesAssigned == false)
        {
            //GenerateVertexObjects();
            //Debug.Log(vertexArray.Length);
            vertexesAssigned = true;
        }
        
        //need to put tower-check code here
        if (towerPlaced == true)
        {
            canSpawn = true;
            timeDuration = 120;
            //ResetTimer();

            
        }

        if (canSpawn)
        {
            if (enemyCount < 16)
            {
                //SpawnUnit();
                canSpawn = false;
                enemyCount++;
            }
        }
    }


    private void UpdateTimerDisplay(float time) 
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        minute1.text = currentTime[0].ToString();
        minute2.text = currentTime[1].ToString();
        second1.text = currentTime[2].ToString();
        second2.text = currentTime[3].ToString();
    }
    private void ResetTimer()
    {
        timer = timeDuration;
    }

    private void SpawnUnit()
    {
        GameObject enemy = Instantiate(enemyPrefab, RandomSpawnPoint(enemySpawnPoints).transform.position, Quaternion.identity);
        enemy.transform.tag = "Enemy";
    }

    private GameObject RandomSpawnPoint(GameObject[] array)
    {
        float x = UnityEngine.Random.Range(0, array.Length);
        return array[(int)x];
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        int count = 0;

        while (count < 10)
        {
            yield return new WaitForSeconds(3);
            SpawnUnit();

            count++;
        }
    }

    private void GenerateVertexObjects()
    {
        LevelGeneration LG = gameObject.GetComponent<LevelGeneration>();
        GameObject[] array = new GameObject[9 * 121]; //array to hold the vertex objects

        int index = 0;
        foreach (GameObject tile in LG.tiles)
        {
            MeshFilter mf = tile.GetComponent<MeshFilter>();
            Matrix4x4 localToWorld = transform.localToWorldMatrix;

            for (int i = 0; i < mf.mesh.vertices.Length; i++)
            {
                Vector3 worldV = localToWorld.MultiplyPoint3x4(mf.mesh.vertices[i]);
                array[index] = new GameObject();
                array[index].transform.position = worldV;
                array[index].transform.tag = "Vertex";

                index++;
            }


        }

        vertexArray = array;
    }
}
