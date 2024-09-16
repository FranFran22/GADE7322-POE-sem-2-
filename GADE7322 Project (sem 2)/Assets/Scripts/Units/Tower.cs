using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    private float range { get; set; }
    

    public Tower(GameObject pref)
    {
        health = 500;
        damage = 35;
        speed = 0f;
        prefab = pref;
        this.range = range; 
    }

}
