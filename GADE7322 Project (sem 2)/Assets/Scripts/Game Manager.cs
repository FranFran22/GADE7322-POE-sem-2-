using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemySpawnPoints;
    [SerializeField]
    private GameObject enemyTarget;
    [SerializeField]
    private GameObject enemyPrefab;

    private int count;
    private int speed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>();
        count = 0;

        Coroutine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SpawnEnemies()
    {
        GameObject obj = RandomPoint(enemySpawnPoints);
        Instantiate(enemyPrefab, obj.transform.position, Quaternion.identity);

        Debug.Log("enemy spawned");
 
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.MovePosition(enemyTarget.transform.position * speed);
    }

    IEnumerator Coroutine()
    {
        while (count < 11)
        {
            yield return new WaitForSeconds(5);
            SpawnEnemies();

            count++;

            Debug.Log("enemy +1");
        }
    }

    private GameObject RandomPoint(GameObject[] array)
    {
        int i = UnityEngine.Random.Range(0, array.Length);
        return array[i];
    }
}
