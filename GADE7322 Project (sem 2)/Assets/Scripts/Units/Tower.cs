using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    private float range { get; set; }
    

    public Tower(GameObject pref, GameObject spn)
    {
        health = 500;
        damage = 35;
        speed = 0f;
        spawn = spn;
        prefab = InstantiatePrefab(pref, spn);
        this.range = range; 
    }

    public override GameObject InstantiatePrefab(GameObject prefab, GameObject spawn)
    {
        GameObject obj = GameObject.Instantiate(prefab, spawn.transform);
        return obj;
    }
}
