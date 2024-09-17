using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetLocation;
    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();

    public Enemy enemy;
    public GameManager GM;
    private GameObject tower;
    private float speed;
    private Tower towerUnit;
    private Defender targetDefender;

    public bool inRange;
    public int maxHealth;
    public int currentHealth;

    public Material defaultMaterial;
    public Material damagedMaterial;

    public bool takingDamage;
    private MeshRenderer meshR;

    private Slider slider;
    public HealthBar healthBar;


    void Start()
    {
        //Get values from GameManager:
        GameObject obj = GameObject.Find("Game Manager");
        meshR = gameObject.GetComponent<MeshRenderer>();

        GM = obj.GetComponent<GameManager>();
        enemy = GM.enemies[(int)RandomNum(GM.enemies.Length)];
        tower = GM.tower;

        defaultMaterial = GM.enemyDefaultMaterial;
        damagedMaterial = GM.enemyDamagedMaterial;

        TowerController TC = tower.GetComponent<TowerController>();
        towerUnit = TC.towerUnit;

        //targetDefender = --> need to assign this when near a defender

        waypoints = enemy.waypointList;
        speed = enemy.speed;
        targetLocation = waypoints[0].transform.position;

        maxHealth = enemy.health;
        currentHealth = maxHealth;

        inRange = false;

        //initialise health bar
        healthBar = gameObject.GetComponent<HealthBar>();
        GameObject HB = transform.Find("Health bar prefab").gameObject;
        slider = HB.GetComponent<Slider>();
        healthBar.healthBar = slider;
        healthBar.maxHealth = maxHealth;
    }


    void Update()
    {
        //when damaged --> change material for a small amount of time
        //if (takingDamage == true)
            //StartCoroutine(ShowAttack());

        //movement
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetLocation, step);

        if (inRange == true && healthBar.currentHealth <= 0)
        {
            Debug.Log("Enemey destroyed!");
            Destroy(gameObject);
        }
    }

    private float RandomNum(int range)
    {
        float r = Random.Range(0, range);
        return r;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            inRange = true;

            int index = 0;
            foreach (GameObject item in waypoints)
            {
                if (other.gameObject == item && index < waypoints.Count)
                    targetLocation = waypoints[index + 1].transform.position;

                if (item == waypoints[waypoints.Count - 1])
                    targetLocation = tower.transform.position;

                index++;
            }
        }

        if (other.tag == "Tower")
        {
            Debug.Log("Enemy near tower!");
            targetLocation = tower.transform.position;
            speed = 0;

            Rigidbody RB = gameObject.GetComponent<Rigidbody>();
            RB.constraints = RigidbodyConstraints.FreezePosition;

            //attack tower
            Attack(other.tag);
        }
    }

    private void Attack(string unitName)
    {
        //need to get the unit type & change the game manager's stored health value

        switch (unitName)
        {
            case "Tower":
                StartCoroutine(AttackTower());
                break;

            case "Defender":
                StartCoroutine(AttackDefender());
                break;

            default:
                break;
        }
    }

    private IEnumerator AttackTower()
    {

        HealthBar towerHB = tower.GetComponent<HealthBar>();
        int newHealth = towerHB.currentHealth - enemy.damage;
        towerHB.SetHealth(newHealth);

        Debug.Log("Enemy attacked tower");

        yield return new WaitForSeconds(1);
    }

    private IEnumerator AttackDefender()
    {
        yield return new WaitForSeconds(2);
        targetDefender.health = targetDefender.health - enemy.damage;
        Debug.Log("Enemy attacked defender");
    }

    private Defender FindDefender()
    {
        float distance;
        List<Defender> closeDefenders = new List<Defender>();

        foreach (Defender defender in GM.defenders)
        {
            distance = Vector3.Distance(defender.prefab.transform.position, gameObject.transform.position);

            if (distance < 4)
                closeDefenders.Add(defender);
        }

        float x = RandomNum(closeDefenders.Count);
        return closeDefenders[(int) x];
    }

    private IEnumerator ShowAttack()
    {
        yield return new WaitUntil(() => takingDamage == true);
        yield return new WaitForSecondsRealtime(2);
        
        meshR.material = damagedMaterial;

        yield return new WaitForSecondsRealtime(1);

        meshR.material = defaultMaterial;
        takingDamage = false;

        Debug.Log("Enemy took damage from tower");
    }
}
