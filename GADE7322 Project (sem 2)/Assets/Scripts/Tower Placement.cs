using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private Vector3 position;

    [SerializeField]
    private bool clicked;

    private void Start()
    {
        clicked = false;
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
                CalculatePosition();
                placeableObject = Instantiate(towerPrefab, position, Quaternion.identity);
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
