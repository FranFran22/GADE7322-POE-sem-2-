using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    private GameManager GM;
    public Tower towerUnit;
    private GameObject tower;

    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;
    public Slider slider;

    [SerializeField]
    public List<Unit> enemiesInRange = new List<Unit>();


    void Start()
    {
        tower = gameObject;
        towerUnit = new Tower(gameObject); 

        maxHealth = towerUnit.health;
        currentHealth = maxHealth;

        //initialise health bar
        healthBar = gameObject.GetComponent<HealthBar>();
        GameObject HB = GameObject.Find("Health bar (tower)");
        slider = HB.GetComponent<Slider>();
        healthBar.healthBar = slider;
        healthBar.maxHealth = maxHealth;
    }


    void Update()
    {
        currentHealth = healthBar.currentHealth;
        CheckForEnemies();

        //if (enemiesInRange.Count > 0)
            //StartCoroutine(Attack());
    }

    private IEnumerator Attack(GameObject enemy)
    {
        yield return new WaitForSeconds(4);

        if (enemy != null)
        {
            HealthBar HB = enemy.GetComponent<HealthBar>();
            int newHealth = HB.currentHealth - towerUnit.damage;
            HB.SetHealth(newHealth);

            Debug.Log("Tower attacked an enemy");
        }
    }

    private void CheckForEnemies()
    {
        enemiesInRange.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                EnemyController EC = enemy.GetComponent<EnemyController>();
                float distanceTo = Vector3.Distance(enemy.transform.position, gameObject.transform.position);

                if (distanceTo < 2.5f)
                {
                    enemiesInRange.Add(EC.enemy);
                    StartCoroutine(Attack(enemy));
                }
            }

        }
    }
}
