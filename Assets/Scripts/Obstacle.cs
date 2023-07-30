using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObstacleData;

//I think all the code related to obstacle spawning may be stupid and overengineered and needlessly scalable, but it is what it is
public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public ObstacleData ObstacleData { get; private set; }
    public GameObject ObstacleInstance { get; private set; }

    public float differenceFromRef;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ObstacleData);
        CreateInstance();
    }

    // Update is called once per frame
    void Update()
    {

        Spin();
        MatchObstacleData();
    }

    void Spin()
    {
        if (ObstacleData.ShouldSpin)
            ObstacleInstance.transform.Rotate(0f, 0f, ObstacleData.RotationSpeed);
    }

    void CreateInstance()
    {
        ObstacleInstance = Instantiate(ObstacleData.ObstacleReference, transform);

        ObstacleInstance.transform.localPosition = Vector3.zero;
        ObstacleInstance.transform.localScale = Vector3.one;

        //ObstacleInstance.SetActive(false);
    }

    public void SetObstacleData(ObstacleData obstacleData)
    {
        ObstacleData = obstacleData;
    }

    void MatchObstacleData()
    {
        //Debug.Log($"position: {transform.localPosition} | instance rotation: {ObstacleInstance.transform.localRotation}");

        SetGlobalScale(transform, ObstacleData.ConstantScale);

    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
}
