using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitbuttons : MonoBehaviour
{
    public void DefenderButtonClicked()
    {
        // spawn in defenders
        GameManager gm =  GetComponent<GameManager>();
        gm.CallDefenders();
    }

    public void TowerButtonClicked()
    {
        // place tower
        GameManager gm = GetComponent<GameManager>();
        gm.PlaceTower();
    }
}
