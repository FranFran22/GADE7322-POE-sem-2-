using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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


    void Start()
    {
        //Get values from GameManager:
        GameObject obj = GameObject.Find("Game Manager");

        GM = obj.GetComponent<GameManager>();
        enemy = GM.enemies[(int)RandomNum(GM.enemies.Length)];
        tower = GM.tower;
        towerUnit = GM.towerUnit;
        //targetDefender = --> need to assign this when near a defender

        waypoints = enemy.waypointList;
        speed = enemy.speed;
        targetLocation = waypoints[0].transform.position;
    }


    void Update()
    {
        //update health


        //movement
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetLocation, step);
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

            //attack tower
            Attack(other.tag);
        }
    }

    private void Attack(string unitName)
    {
        //need to access the GM --> UI will use this as its info
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
        yield return new WaitForSeconds(2);
        towerUnit.health = towerUnit.health - enemy.damage;
    }

    private IEnumerator AttackDefender()
    {
        yield return new WaitForSeconds(2);
        targetDefender.health = targetDefender.health - enemy.damage;
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
}
