using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Enemy(GameObject pref, GameObject spn)
    {
        health = 100;
        damage = 50;
        speed = 15f;
        spawn = spn;
        prefab = InstantiatePrefab(pref, spn); ;
    }

    public override GameObject InstantiatePrefab(GameObject prefab, GameObject spawn)
    {
        GameObject obj = GameObject.Instantiate(prefab, spawn.transform);
        return obj;
    }
}
