using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Unit
{
    public List<GameObject> waypointList;

    public Enemy2(GameObject pref, List<GameObject> list)
    {
        health = 100;
        damage = 65;
        speed = 2f;
        prefab = pref;
        range = 4f;
        waypointList = list;
    }

    // stun ability
    // dash ability
}
