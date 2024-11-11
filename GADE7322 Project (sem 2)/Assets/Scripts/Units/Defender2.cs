using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender2 : Unit
{
    public Defender2(GameObject obj)
    {
        health = 100;
        damage = 30;
        speed = 2.5f;
        prefab = obj;
        range = 2.5f;
    }
}
