using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public int health;
    private int maxHealth;


    void Start()
    {
        TowerController TC = gameObject.GetComponent<TowerController>();
        maxHealth = TC.maxHealth;

        GameObject HB = GameObject.Find("Health bar (tower)");
        healthBar = HB.GetComponent<Slider>();

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }


    void Update()
    {
        
    }

    private void SetHealth(int HP)
    {
        healthBar.value = HP;
    }
}
