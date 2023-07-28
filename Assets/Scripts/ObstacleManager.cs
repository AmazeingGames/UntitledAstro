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

    [SerializeField] Obstacle obstacleObject;

    [SerializeField] List<ObstacleData> obstacleData = new();
    [SerializeField] int obstaclePoolSizePerObject;

    GameObject tunnelManager;
    RepeatTunnel repeatTunnel;

    void Start()
    {
        obstaclePool = GameObject.Find("Obstacle Pool").transform;
        tunnelManager = GameObject.Find("TunnelManager");

        repeatTunnel = tunnelManager.GetComponent<RepeatTunnel>();
    }

    void Awake()
    {
        StartCoroutine(CreateObstaclePool());    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnRoadblock();
        }
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

    void SpawnRoadblock()
    {
        var obstacle = GetFromObstaclePool();

        if (obstacle == null)
            return;

        obstacle.SetActive(true);

        var panel = repeatTunnel.GetRandomPanel();

        obstacle.transform.SetParent(panel, true);

        obstacle.transform.localPosition = Vector3.zero;

        switch (panel.name)
        {
            case "top":
                break;

            case "topLeft":
                Debug.Log(panel.name);
                break;

            case "topRight":
                Debug.Log(panel.name);
                break;

            case "bot":
                obstacle.transform.localPosition = (new Vector3(0.0045f, 0.00768f));

                Obstacle obstacleObstacle = obstacle.GetComponent<Obstacle>();

                obstacleObstacle.ObstacleInstance.transform.localRotation = new Quaternion(0.50000f, 0.50000f, -0.50000f, -0.50000f);

                Debug.Log($"obstacle position: {obstacle.transform.localPosition}");
                break;

            case "botLeft":
                Debug.Log(panel.name);
                break;

            case "botRight":
                Debug.Log(panel.name);
                break;
        }

    }
}
