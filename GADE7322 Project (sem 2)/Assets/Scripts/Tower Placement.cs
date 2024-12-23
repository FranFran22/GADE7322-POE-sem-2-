using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private GameManager GM;

    [SerializeField]
    private KeyCode hotkey = KeyCode.T;

    public static GameObject placeableObject;
    private Vector3 position = new Vector3(10, 0.2f, 10);

    [SerializeField]
    private bool clicked;

    private void Start()
    {
        clicked = false;
        GM.canSpawn = true;
    }

    private void Update()
    {
        HandleInput();
    }

    private void CalculatePosition()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            Debug.Log("Hit " + hitInfo);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(hotkey) || clicked == true)
        {
            if (placeableObject == null)
            {
                //CalculatePosition(); --> not working as needed
                placeableObject = Instantiate(towerPrefab, position, Quaternion.identity);

                placeableObject.transform.AddComponent<BoxCollider>();
                BoxCollider BC = placeableObject.transform.GetComponent<BoxCollider>();
                BC.size = new Vector3(2, 0.1f, 2);
                BC.isTrigger = true;
            }
                
            else
                Destroy(placeableObject);

            clicked = false;

            Debug.Log("T clicked");
        }
        
    }

    public void ButtonClicked()
    {
        clicked = true;
    }
}
