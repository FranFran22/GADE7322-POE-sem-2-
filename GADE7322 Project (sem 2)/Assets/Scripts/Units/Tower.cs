using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    private float range { get; set; }
    [SerializeField]
    private GameObject spawnPoint;
    

    public Tower()
    {
        health = 500;
        damage = 35;
        speed = 0f;
        spawn = spawnPoint;
    }
}
