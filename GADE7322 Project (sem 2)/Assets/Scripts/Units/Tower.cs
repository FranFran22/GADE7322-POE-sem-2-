using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{

    public Tower(GameObject pref)
    {
        health = 500;
        damage = 25;
        speed = 0f;
        prefab = pref;
    }

}
