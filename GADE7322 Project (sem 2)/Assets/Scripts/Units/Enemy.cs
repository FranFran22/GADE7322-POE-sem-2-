using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Enemy()
    {
        health = 100;
        damage = 50;
        speed = 15f;
        spawn = null;
    }
}
