using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DefenderPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject defender1Prefab;

    [SerializeField]
    private GameObject defender2Prefab;

    [SerializeField]
    private GameObject defender3Prefab;

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private KeyCode hotkey = KeyCode.D;

    private GameObject placeableObject;
    private Vector3 position = new Vector3(10, 0.1f, 10);
    private GameObject[] defenderSpawns = new GameObject[3];
    private GameObject[] vertexes;
    private int defenderCount;
    private GameObject tower;
    private Vector3[] positions = new Vector3[4];

    [SerializeField]
    private bool clicked;

    private bool spawnsGenerated;


    private void Start()
    {
        clicked = false;
        spawnsGenerated = false;
    }

    void Update()
    {
        HandleInput();

        if (GM.towerPlaced && !spawnsGenerated)
        {
            tower = GM.tower;
            CalculatePosition();
            SpawnCreation();

            spawnsGenerated = true;
        }
           
    }

    private void HandleInput()
    {
        if ((Input.GetKeyDown(hotkey) || clicked == true) && GM.towerPlaced == true)
        {
            if (placeableObject == null || defenderCount <= 3)
            {
                Vector3 position = defenderSpawns[RandomNum(defenderSpawns.Length)].transform.position;

                placeableObject = Instantiate(RandomPrefab(), position, Quaternion.identity);
                defenderCount++;
            }

            clicked = false;

            Debug.Log("D clicked");
        }

     
    }

    private void CalculatePosition()
    {
        //generate spawns
        positions[0] = new Vector3(tower.transform.position.x + 2, tower.transform.position.y, tower.transform.position.z);
        positions[1] = new Vector3(tower.transform.position.x - 2, tower.transform.position.y, tower.transform.position.z);
        positions[2] = new Vector3(tower.transform.position.x, tower.transform.position.y, tower.transform.position.z + 2);
        positions[3] = new Vector3(tower.transform.position.x + 2, tower.transform.position.y, tower.transform.position.z - 2);

    }

    public void ButtonClicked()
    {
        clicked = true;
    }

    private void SpawnCreation()
    {
        for (int i = 0; i < defenderSpawns.Length; i++)
        {
            defenderSpawns[i] = new GameObject();
            defenderSpawns[i].name = "Defender spawn";
            defenderSpawns[i].transform.position = positions[i];
            defenderSpawns[i].tag = "Defender";
        }
    }

    private int RandomNum(int range)
    {
        float x = Random.Range(0, range);
        return (int) x;
    }  

    private GameObject RandomPrefab()
    {
        float x = RandomNum(3);
        GameObject prefab = new GameObject();

        switch(x)
        {
            case 0:
                prefab = defender1Prefab;
                break;

            case 1:
                prefab = defender2Prefab;
                break;

            case 2:
                prefab = defender3Prefab;
                break;

            default:
                break;
        }

        return prefab;
    }
   
}
