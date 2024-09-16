using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private GameManager GM;
    private Tower towerUnit;
    private GameObject tower;

    public int maxHealth;
    public int currentHealth;
    //public HealthBar healthBar; --> ??


    void Start()
    {
        maxHealth = towerUnit.health;
        currentHealth = maxHealth;
    }


    void Update()
    {
        
    }
}
