using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 targetLocation;
    private List<GameObject> waypoints = new List<GameObject>();
    public Enemy enemy;
    public GameManager GM;
    private GameObject tower;
    private float speed;


    void Start()
    {
        GameObject obj = GameObject.Find("Game Manager");

        GM = obj.GetComponent<GameManager>();
        enemy = GM.enemies[(int)RandomNum()];

        Debug.Log(enemy.waypointList.Count);

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

    private float RandomNum()
    {
        float r = Random.Range(0, GM.enemies.Length);
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

                index++;
            }
        }
    }
}
