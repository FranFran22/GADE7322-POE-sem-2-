using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    private GameObject[] spawnPoints;

    public Enemy()
    {
        health = 100;
        damage = 50;
        speed = 15f;
        spawn = RandomPoint(spawnPoints);
    }

    private GameObject RandomPoint(GameObject[] array)
    {
        int i = UnityEngine.Random.Range(0, array.Length);
        return array[i];
    }
}
