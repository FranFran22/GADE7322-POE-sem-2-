using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Unit
{
    [SerializeField]
    private GameObject spawnPoint;

    public Defender()
    {
        health = 100;
        damage = 60;
        speed = 10f;
        spawn = spawnPoint;
    }
}
