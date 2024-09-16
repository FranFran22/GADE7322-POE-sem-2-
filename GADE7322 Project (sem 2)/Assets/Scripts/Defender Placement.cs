using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DefenderPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject defenderPrefab;

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private KeyCode hotkey = KeyCode.D;

    private GameObject placeableObject;
    private Vector3 position = new Vector3(10, 0.1f, 10);
    private GameObject[] defenderSpawns = new GameObject[3];
    private GameObject[] vertexes;
    private int defenderCount;

    [SerializeField]
    private bool clicked;


    private void Start()
    {
        clicked = false;
        SpawnCreation();

    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if ((Input.GetKeyDown(hotkey) || clicked == true) && GM.towerPlaced == true)
        {
            if (placeableObject == null || defenderCount <= 3)
            {
                placeableObject = Instantiate(defenderPrefab, position, Quaternion.identity);
                defenderCount++;
            }

            clicked = false;

            Debug.Log("D clicked");
        }

     
    }

    private void CalculatePosition(GameObject[] vertexArray)
    {
        //generate spawns
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
        }
    }
}
