using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Unit
{
    public List<GameObject> waypointList;

    public Enemy3(GameObject pref, List<GameObject> list)
    {
        health = 150;
        damage = 80;
        speed = 3.5f;
        prefab = pref;
        range = 2.5f;
        waypointList = list;
    }

    // dash ability
}
