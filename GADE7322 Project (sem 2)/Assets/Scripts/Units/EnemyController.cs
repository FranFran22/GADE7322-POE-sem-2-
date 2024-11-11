using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetLocation;

    [SerializeField]
    //private List<GameObject> waypoints = new List<GameObject>();

    public List<Vector3> pathLocations = new List<Vector3>();
    private GameObject[] vertices;
    private Vector3 currentPos;
    private int posIndex;
    private int enemyIndex;

    public Unit enemy;
    public GameManager GM;
    private GameObject tower;
    private float speed;
    private Tower towerUnit;
    private Unit targetDefender;
    private GameObject[] defenders;

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
        //Inititalisation
        GameObject obj = GameObject.Find("Game Manager");
        meshR = gameObject.GetComponent<MeshRenderer>();
        GM = obj.GetComponent<GameManager>();
        enemyIndex = (int)RandomNum(GM.tier1Enemies.Count);
        enemy = GM.tier1Enemies[enemyIndex];
        tower = GM.tower;
        TowerController TC = tower.GetComponent<TowerController>();
        towerUnit = TC.towerUnit;
        speed = enemy.speed;
        inRange = false;
        posIndex = 1;

        //pathing
        pathLocations = FindPath();
        targetLocation = pathLocations[0];
        currentPos = gameObject.transform.position;

        //find defenders
        defenders = GameObject.FindGameObjectsWithTag("Defender");

        //initialise health
        maxHealth = enemy.health;
        currentHealth = maxHealth;

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
        currentPos = Vector3.MoveTowards(currentPos, targetLocation, step);
        gameObject.transform.position = currentPos;

        if (Vector3.Distance(targetLocation, currentPos) < 0.01f)
        {
            Debug.Log(posIndex);

            if (posIndex < pathLocations.Count)
            {
                targetLocation = pathLocations[posIndex];
                posIndex++;
            }

            else
                targetLocation = tower.transform.position;
            
        }


        if (defenders != null)
        {
            targetDefender = FindDefender();
            Attack("Defender");
        }
        
        if (inRange == true && healthBar.currentHealth <= 0)
        {
            Debug.Log("Enemy destroyed!");

            float n = RandomNum(GM.tier1Enemies.Count);
            GM.tier1Enemies.RemoveAt((int)n);
            Destroy(gameObject);
        }
    }

    private float RandomNum(int range)
    {
        float r = UnityEngine.Random.Range(0, range);
        return r;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Vertex")
        {
            inRange = true;

            Debug.Log("Hit vertex");
        }

        if (other.tag == "Tower")
        {
            Debug.Log("Enemy near tower!");
            targetLocation = tower.transform.position;
            speed = 0;

            GM.phaseTwo = true;

            Rigidbody RB = gameObject.GetComponent<Rigidbody>();
            RB.constraints = RigidbodyConstraints.FreezePosition;

            //attack tower
            //Attack(other.tag);
            StartCoroutine(AttackTower());

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

    private List<Vector3> FindPath()
    {
        LevelGeneration LG = GM.GetComponent<LevelGeneration>();
        vertices = LG.vertexObjects;

        List<Vector3> positions = new List<Vector3>();
        Vector3 currentVertex = gameObject.transform.position;
        int index = 0;
        float distance = 1.414f;
        float distToTower = 0;

        foreach (GameObject vertex in vertices)
        {
            // is this vertex next to me ?
            // is this vertex position closer to the tower?
            // is this vertex position below 0.7 on the map

            distToTower = Vector3.Distance(tower.transform.position, currentVertex);

            //Debug.Log(index);

            if (index > 0 && positions.Count > 1)
            {
                //Debug.Log(positions.Count);
                currentVertex = positions[positions.Count - 1];
                distToTower = Vector3.Distance(tower.transform.position, positions[positions.Count - 1]);
            }

            if (Vector3.Distance(tower.transform.position, vertex.transform.position) < distToTower)
            {
                if (Vector3.Distance(vertex.transform.position, currentVertex) <= distance)
                {
                    if (vertex.transform.position.y < 0.7)
                    {
                        positions.Add(vertex.transform.position);
                    }
                }
            }

            index++;

            if (distToTower <= 3)
                break;
        }

        return positions;
    }

    private IEnumerator AttackTower()
    {
        HealthBar towerHB = tower.GetComponent<HealthBar>();
        int newHealth = towerHB.currentHealth - (enemy.damage + 15);
        towerHB.SetHealth(newHealth);

        Debug.Log("Enemy attacked tower");

        yield return new WaitForSeconds(1);
    }

    private IEnumerator AttackDefender()
    {
        if (targetDefender != null)
        {
            yield return new WaitForSeconds(2);
            targetDefender.health = targetDefender.health - enemy.damage;
            Debug.Log("Enemy attacked defender");
        }   
    }

    private Defender FindDefender()
    {
        if (GM.defenders != null)
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
            return closeDefenders[(int)x];
        }

        else
            return null;
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
