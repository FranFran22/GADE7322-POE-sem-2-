using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Unit
{
    public Defender(GameObject obj)
    {
        health = 100;
        damage = 15;
        speed = 3f;
        prefab = obj;
    }
}
