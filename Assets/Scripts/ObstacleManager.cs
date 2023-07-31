using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleManager : MonoBehaviour
{
    Transform obstaclePool;

    [SerializeField] GameObject splitTunnelReference;
    GameObject splitTunnelInstance;

    [SerializeField] Obstacle obstacleObject;

    [SerializeField] List<ObstacleData> obstacleData = new();
    [SerializeField] int obstaclePoolSizePerObject;

    [SerializeField] int ObstaclesToSpawnPer;
    [SerializeField] float timeBetweenObstacles;

    Pause pause;
    GameObject tunnelManager;
    RepeatTunnel repeatTunnel;

    Quaternion startingObstacleRotation;
    Vector3 startingObstaclePosition;

    Dictionary<String, Quaternion> panelReferenceRotation = new Dictionary<string, Quaternion>()
    {
        { "top",        new Quaternion(0.00000f, 0.00000f, 0.25882f, 0.96593f) },
        { "topLeft",    new Quaternion(0.00000f, 0.00000f, -0.25882f, 0.96593f) },
        { "topRight",   new Quaternion(0.00000f, 0.00000f, 0.70711f, 0.70711f) },
        { "bot",        new Quaternion(0.00000f, 0.00000f, 0.96593f, -0.25882f) },
        { "botLeft",     new Quaternion(0.00000f, 0.00000f, -0.70711f, 0.70711f) },
        { "botRight",   new Quaternion(0.00000f, 0.00000f, 0.96593f, 0.25882f) },
    };

    bool hasCoroutineStarted = false;

    void Awake()
    {
        StartCoroutine(CreateObstaclePool());
    }

    void Start()
    {
        obstaclePool = GameObject.Find("Obstacle Pool").transform;
        tunnelManager = GameObject.Find("TunnelManager");
        pause = GetComponent<Pause>();

        repeatTunnel = tunnelManager.GetComponent<RepeatTunnel>();

        repeatTunnel.ResetTunnel += RemoveObstacles;

        splitTunnelInstance = Instantiate(splitTunnelReference);

        splitTunnelInstance.transform.position = new Vector3(0, -20);
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnRoadblock();
        }

        if (!hasCoroutineStarted)
            StartCoroutine(SpawnObstacles());
    }


    IEnumerator CreateObstaclePool()
    {
        if (obstaclePool == null)
        {
            obstaclePool = GameObject.Find("Obstacle Pool").transform;
        }

        for (int n = 0; n < obstacleData.Count; n++)
        {
            for (int i = 0; i < obstaclePoolSizePerObject; i++)
            {
                var currentObstacle = Instantiate(obstacleObject, obstaclePool);

                startingObstacleRotation = currentObstacle.transform.rotation;
                startingObstaclePosition = currentObstacle.transform.position;

                currentObstacle.SetObstacleData(obstacleData[n]);
            }
        }

        yield return null;

        foreach (Transform obstacle in obstaclePool)
        {
            obstacle.gameObject.SetActive(false);
        }

        yield break;
    }

    GameObject GetFromObstaclePool()
    {
        for (int i = 0; i < obstaclePool.childCount; i++)
        {
            var currentObstacle = obstaclePool.GetChild(i);

            if (!currentObstacle.gameObject.activeSelf)
                return currentObstacle.gameObject;
        }
        return null;
    }

    IEnumerator SpawnObstacles()
    {
        hasCoroutineStarted = true;

        while (true)
        {
            //Debug.Log($"IsGameRunning: {pause.IsGameRunning} | IsPaused: {pause.IsPaused}");

            if (!pause.IsGameRunning || pause.IsPaused)
            {
                yield return null;
                continue;
            }

            for (int i = 0; i < ObstaclesToSpawnPer; i++)
            {
                SpawnRoadblock();
            }
            yield return new WaitForSeconds(timeBetweenObstacles);
        }
    }


    //I don't know why, but this function doesn't work properly. Sometimes panels are removed sometimes they're not. It all has to do with "obstacle.transform.SetParent(obstaclePool);" for some reason.
    void RemoveObstacles(Transform tunnel)
    {
        List<Transform> panelsList = RepeatTunnel.GetPanelsFromTunnel(tunnel);

        for (int i = 0; i < panelsList.Count; i++) 
        {
            
            var currentPanel = panelsList[i];

            for (int n = 0; n < currentPanel.childCount; n++)
            {
                ReturnToObstaclePool(currentPanel.GetChild(n));
            }
        }
    }

    void ReturnToObstaclePool(Transform obstacle)
    {
        if (obstacle == null)
        {
            Debug.LogWarning("ObstacleNull");
            return;
        }

        if (obstacle.GetComponent<Obstacle>() == null)
        {
            Debug.LogWarning("NotObstacle");
            return;
        }

        obstacle.transform.SetParent(obstaclePool);

        obstacle.gameObject.SetActive(false);
        obstacle.transform.rotation = startingObstacleRotation;
        obstacle.transform.position = startingObstaclePosition;
    }

    //This code is so hack and I am never working with rotations ever again.
    void SpawnRoadblock()
    {
        var obstacle = GetFromObstaclePool();

        if (obstacle == null)
            return;

        obstacle.SetActive(true);

        var spawnPanel = repeatTunnel.GetRandomPanel();

        Obstacle obstacleObstacle = obstacle.GetComponent<Obstacle>();

        Quaternion obstacleRotation = new();
        Vector3 obstaclePosition = new();

        switch (spawnPanel.name)
        {
            case "top":
                obstaclePosition = (new Vector3(0.00428f, 0.00754f));

                obstacleRotation = new Quaternion(0.50000f, -0.50000f, -0.50000f, 0.50000f);
                break;

            case "topLeft":
                obstaclePosition = (new Vector3(0.00439f, 0.00767f));

                obstacleRotation = new Quaternion(-0.68301f, 0.18301f, 0.68301f, -0.18301f);
                break;

            case "topRight":
                obstaclePosition = (new Vector3(0.00431f, 0.00755f));

                obstacleRotation = new Quaternion(0.18301f, -0.68301f, -0.18301f, 0.68301f);
                break;

            case "bot":
                obstaclePosition = (new Vector3(0.0045f, 0.00768f));

                obstacleRotation = new Quaternion(0.50000f, 0.50000f, -0.50000f, -0.50000f);
                break;

            case "botLeft":
                obstaclePosition = (new Vector3(0.00437f, 0.00754f));

                obstacleRotation = new Quaternion(-0.68301f, -0.18301f, 0.68301f, 0.18301f);
                break;

            case "botRight":
                obstaclePosition = (new Vector3(0.00437f, 0.00762f));

                obstacleRotation = new Quaternion(-0.18301f, -0.68301f, 0.18301f, 0.68301f);
                break;
        }

        for (int i = 0; i < splitTunnelInstance.transform.childCount; i++)
        {
            var child = splitTunnelInstance.transform.GetChild(i);
  
            if (child.name == spawnPanel.name)
            {
                child.transform.localRotation = panelReferenceRotation[child.name];
                obstacle.transform.SetParent(child, true);
                
                obstacle.transform.localPosition = obstaclePosition;
                obstacleObstacle.ObstacleInstance.transform.localRotation = obstacleRotation;

                child.transform.localRotation = spawnPanel.transform.localRotation;
                
                obstacle.transform.SetParent(spawnPanel, true);

                obstacle.transform.localPosition = obstaclePosition;
                break;
            }
        }

        


    }
}
