using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstacle;
    private Vector3 spawnPos = new Vector3(0, 0, 0);
    private float spawnInterval = 2;
    private float repeatRate = 2;
    void Start()
    {
        InvokeRepeating("SpawnObstacle", spawnInterval, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnObstacle()
    {
        Instantiate(obstacle, spawnPos, obstacle.transform.rotation);
    }

}
