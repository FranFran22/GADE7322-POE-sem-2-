using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    public int health { get; set; }
    public int damage { get; set; }
    public float speed { get; set; }
    public GameObject prefab { get; set; }


    public Unit()
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.prefab = prefab;
    }
}
