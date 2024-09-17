using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//will manage pathing, movement and attacks

public class DefenderController : MonoBehaviour
{
    private GameObject[] enemies;

    public Defender defender;
    private Vector3 targetLocation;
    public GameManager GM;
    private GameObject targetEnemy;

    public int maxHealth;
    public int currentHealth;
    private Slider slider;
    public HealthBar healthBar;

    void Start()
    {
        defender = new Defender(gameObject);

        GameObject obj = GameObject.Find("Game Manager");
        GM = obj.GetComponent<GameManager>();

        //initialise health bar
        healthBar = gameObject.GetComponent<HealthBar>();
        GameObject HB = transform.Find("Health bar prefab").gameObject;
        slider = HB.GetComponent<Slider>();
        healthBar.healthBar = slider;
        healthBar.maxHealth = maxHealth;
    }


    void Update()
    {
        FindEnemies();

        float step = defender.speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetLocation, step);
    }

    private void FindEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceTo = Vector3.Distance(enemy.transform.position, gameObject.transform.position);

            if (distanceTo < 5)
            {
                targetEnemy = enemy;
                targetLocation = targetEnemy.transform.position;

                StartCoroutine(Attack(targetEnemy));
            }
                
        }

    }

    private IEnumerator Attack(GameObject enemy)
    {
        HealthBar HB = enemy.GetComponent<HealthBar>();
        int newHealth = HB.currentHealth - defender.damage;
        HB.SetHealth(newHealth);

        Debug.Log("Defender attacked enemy");

        yield return new WaitForSeconds(1.5f);
    }
}
