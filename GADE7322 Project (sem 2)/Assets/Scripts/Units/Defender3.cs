using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender3 : Unit
{
    public Defender3(GameObject obj)
    {
        health = 150;
        damage = 50;
        speed = 4f;
        prefab = obj;
        range = 3f;
    }
}
