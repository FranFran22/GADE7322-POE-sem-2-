using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public List<GameObject> waypointList;

    public Enemy(GameObject pref, List<GameObject> list)
    {
        health = 65;
        damage = 30;
        speed = 2f;
        prefab = pref;
        range = 1.5f;
        waypointList = list;
    }

    //stun ability
}
