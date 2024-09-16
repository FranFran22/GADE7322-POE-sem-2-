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

    [SerializeField]
    public List<Enemy> enemiesInRange = new List<Enemy>();


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
        CheckForEnemies();

        //if (enemiesInRange.Count > 0)
            //StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2);

        foreach (Enemy enemy in enemiesInRange)
        {
            EnemyController EC = enemy.prefab.GetComponent<EnemyController>();
            EC.currentHealth = EC.currentHealth - towerUnit.damage;
            EC.takingDamage = true;

            Debug.Log("Tower attacked an enemy");
        }

    }

    private void CheckForEnemies()
    {
        enemiesInRange.Clear();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyController EC = enemy.GetComponent<EnemyController>();
            if (EC.inRange == true)
            {
                enemiesInRange.Add(EC.enemy);
                Debug.Log(enemiesInRange.Count + " enemies n range of tower");
            }
        }
    }
}
