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

    public bool hasGameBegan = true;

    GameObject tunnelManager;
    RepeatTunnel repeatTunnel;

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

        repeatTunnel = tunnelManager.GetComponent<RepeatTunnel>();

        //What this does is call this function (RemoveObstacles) whenever the evenet (resetTunnel) is called
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

        if (hasGameBegan && !hasCoroutineStarted)
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
        while (true)
        {
            hasCoroutineStarted = true;
            for (int i = 0; i < ObstaclesToSpawnPer; i++)
            {
                SpawnRoadblock();
            }
            yield return new WaitForSeconds(timeBetweenObstacles);
        }
    }


    void RemoveObstacles(Transform tunnel)
    {
        List<Transform> panels = RepeatTunnel.GetPanelsFromTunnel(tunnel);

        for (int i = 0; i < panels.Count; i++) 
        {
            var currentPanel = panels[i];

            for (int n = 0; n < currentPanel.childCount; n++)
            {
                var currentObstacle = currentPanel.GetChild(n);

                ReturnToObstaclePool(currentObstacle);
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

        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetParent(obstaclePool);
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

        //Degrees difference from the panel's reference rotation vs the actualy rotation
        //All I need to do is subtract the current z rotation by this number
        float differenceFromReference = Quaternion.Angle(spawnPanel.localRotation, panelReferenceRotation[spawnPanel.name]);

        Debug.Log($"difference from reference: {differenceFromReference}");

        Transform panelMatch = null;

        for (int i = 0; i < splitTunnelInstance.transform.childCount; i++)
        {
            var child = splitTunnelInstance.transform.GetChild(i);
            
            child.transform.localRotation = panelReferenceRotation[child.name];

            if (child.name == spawnPanel.name)
            {
                panelMatch = child;
            }
        }

        obstacle.transform.SetParent(panelMatch, true);

        obstacle.transform.localPosition = obstaclePosition;

        obstacleObstacle.ObstacleInstance.transform.localRotation = obstacleRotation;

        panelMatch.transform.localRotation = spawnPanel.transform.localRotation;

        obstacle.transform.SetParent(spawnPanel, true);

        obstacle.transform.localPosition = obstaclePosition;



    }
}
