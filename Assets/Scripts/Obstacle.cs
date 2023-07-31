using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObstacleData;

//I think all the code related to obstacle spawning may be stupid and overengineered and needlessly scalable, but it is what it is
public class Obstacle : MonoBehaviour
{
    [SerializeField] Vector3 distanceForGameOver;
    
    GameObject player;
    GameObject playerModel;

    GameObject GameManager;
    GameOver GameOver;

    public ObstacleData ObstacleData { get; private set; }
    public GameObject ObstacleInstance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");

        GameOver = GameManager.GetComponent<GameOver>();

        CreateInstance();

        player = GameObject.Find("Player");

        Debug.Log($"Player is null : {player == null}");

        for (int i = 0; i < player.transform.childCount; i++)
        {
            var child = player.transform.GetChild(i);
            
            if (child.gameObject.name == "spaceship")
            {
                playerModel = child.gameObject;
                Debug.Log("found model!");
                break;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
        MatchObstacleData();

        if (Mathf.Abs(ObstacleInstance.transform.position.x - playerModel.transform.position.x) < distanceForGameOver.x)
        {
            if (Mathf.Abs(ObstacleInstance.transform.position.z - playerModel.transform.position.z) < distanceForGameOver.z)
            {
                if (Mathf.Abs(ObstacleInstance.transform.position.y - playerModel.transform.position.y) < distanceForGameOver.y)
                {
                    GameOver.EndGame();
                }
            }
        }
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
