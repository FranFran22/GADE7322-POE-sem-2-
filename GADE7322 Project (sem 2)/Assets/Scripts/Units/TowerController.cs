using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private GameManager GM;
    public Tower towerUnit;
    private GameObject tower;

    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;


    void Start()
    {
        tower = gameObject;
        towerUnit = new Tower(gameObject); 

        healthBar = gameObject.GetComponent<HealthBar>();
        maxHealth = towerUnit.health;
        currentHealth = maxHealth;
    }


    void Update()
    {
        currentHealth = healthBar.currentHealth;
    }
}
