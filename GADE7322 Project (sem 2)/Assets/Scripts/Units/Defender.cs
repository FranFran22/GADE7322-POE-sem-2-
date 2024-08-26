using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Unit
{
    public Defender(GameObject obj, GameObject spn)
    {
        health = 100;
        damage = 60;
        speed = 10f;
        spawn = spn;
        prefab = obj;
    }
}
