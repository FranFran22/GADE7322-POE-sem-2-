using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public int currentHealth;
    public int maxHealth;


    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }


    void Update()
    {
        currentHealth = (int)healthBar.value;
    }

    public void SetHealth(int HP)
    {
        healthBar.value = HP;
    }
}
