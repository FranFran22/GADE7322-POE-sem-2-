using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Unit
{
    public Defender(GameObject obj)
    {
        health = 70;
        damage = 15;
        speed = 2f;
        prefab = obj;
        range = 1.5f;
    }
}
