using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public List<GameObject> waypointList;

    public Enemy(GameObject pref, List<GameObject> list)
    {
        health = 100;
        damage = 50;
        speed = 10f;
        prefab = pref;
        waypointList = list;
    }
}
