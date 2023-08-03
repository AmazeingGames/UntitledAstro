using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;
using Input = UnityEngine.Input;

public class SpinTunnel : MonoBehaviour
{
    //SerializeField is virtually the same as using public, as it allows the variable to be set in the inspector, i.e. making it serialized, without the downside of having it be accessible via other scripts. This is considered a good coding practice. - Amazeing

    [SerializeField] bool canTunnelFlip = true;
    [SerializeField] float flipRotationSpeed;
    [SerializeField] float snapRotationSpeed;
    [SerializeField] bool rotateSmoothly;

    [SerializeField] float degreesToSnap;
    RepeatTunnel repeatTunnel;

    bool isOnBottom = true;
    bool shouldFlip = false;
    bool isFlipping = false;
    bool canRotate = true;

    GameObject gameManager;
    Pause pause;

    ObstacleManager obstacleManager;

    Quaternion target;

    [SerializeField] float speed = 5;

    [SerializeField] bool shouldRotatePanels = true;
    [SerializeField] bool shouldStopAtThreshold = true;
    [SerializeField] float minimumStopThreshold = .5f;

    [SerializeField] Transform botPanel;
    [SerializeField] Transform topPanel;

    readonly Quaternion bottomMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion bottomMaximum = new Quaternion(0.00000f, 0.00000f, 0.70968f, -0.70452f); //(90)

    readonly Quaternion topMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion topMaximum = new Quaternion(0.00000f, 0.00000f, -0.70968f, 0.70452f); //(270)

    readonly List<Transform> tunnels = new List<Transform>();
    readonly List<List<Transform>> listOfPanels = new();

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        pause = gameManager.GetComponent<Pause>();
        obstacleManager = gameManager.GetComponent<ObstacleManager>();

        int tunnelsToGet = transform.childCount;

        repeatTunnel = GetComponent<RepeatTunnel>();

        for (int i = 0; i < tunnelsToGet; i++)
        {
            tunnels.Add(transform.GetChild(i));
        }

        for (int i = 0; i < tunnels.Count; i++)
        {
            var tunnel = tunnels[i];

            var panelsList = new List<Transform>();

            for (int n = 0; n < tunnel.childCount; n++)
            {
                var panel = tunnel.GetChild(n);

                panelsList.Add(panel);
            }
            listOfPanels.Add(panelsList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause.IsGameRunning || pause.IsPaused)
            return;

        RotateTunnels();

        FlipTunnels();
    }

    void FlipTunnels()
    {
        if (Input.GetButtonDown("Jump") && !isFlipping)
        {
            isOnBottom = !isOnBottom;
            shouldFlip = true;

            Debug.Log("We Flipped!");
        }

        if (shouldFlip && canTunnelFlip)
        {
            FlipAllPanels(180, true, true, snapRotationSpeed);
        }
    }

    void FlipAllPanels(float degrees, bool spinClockwise, bool stopTunnelMovement, float rotationSpeed)
    {
        for (int i = 0; i < listOfPanels.Count; i++)
        {
            for (int n = 0; n < listOfPanels[i].Count; n++)
            {
                //Flips all the tunnels by 180 degrees
                var panel = listOfPanels[i][n];
                target = panel.rotation * Quaternion.AngleAxis(degrees, Vector3.back);

                StartCoroutine(RotatePanel(panel, target, spinClockwise, stopTunnelMovement, rotationSpeed));
            }
        }
    }

    //Rotates all the tunnels according to the horizontal axes keys (left, right)
    void RotateTunnels()
    {
        if (!canRotate)
            return;

        float input = Input.GetAxisRaw("Horizontal");
        float amountToRotate;

        if (rotateSmoothly)
            amountToRotate = speed * input * Time.deltaTime;
        else
            amountToRotate = degreesToSnap * input;

        if (ShouldStopRotating(amountToRotate))
            return;

        if (rotateSmoothly)
        {
            for (int i = 0; i < listOfPanels.Count; i++)
            {
                for (int n = 0; n < listOfPanels[i].Count; n++)
                {
                    var currentList = listOfPanels[i];
                    var panel = currentList[n];

                    panel.Rotate(0f, 0f, amountToRotate);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                if (input == 0)
                    return;

                bool spinClockwise = true;

                if (input < 0)
                    spinClockwise = false;

                FlipAllPanels(-amountToRotate, spinClockwise, false, snapRotationSpeed);
            }
        }
    }

    IEnumerator RotatePanel(Transform panel, Quaternion target, bool spinClockwise, bool shouldStopTunnelMovement, float rotationSpeed)
    {
        if (shouldStopTunnelMovement)
        {
            repeatTunnel.canMove = false;
            obstacleManager.SetShouldSpawnObstacles(false);
        }

        shouldFlip = false;
        isFlipping = true;
        canRotate = false;

        while (true)
        {
            int spinDirection = 1;
            if (!spinClockwise)
            {
                spinDirection = -1;
            }

            panel.Rotate(0, 0, rotationSpeed * spinDirection);

            float differenceFromTarget = Quaternion.Angle(panel.rotation, target);

            if (differenceFromTarget < .5f)
            {
                obstacleManager.SetShouldSpawnObstacles(true);
                repeatTunnel.canMove = true;
                isFlipping = false;
                canRotate = true;

                yield break;
            }
            yield return null;
        }
    }

    
        

    bool ShouldStopRotating(float amountToRotate)
    {
        Quaternion minimumToUse = isOnBottom ? bottomMinimum : topMinimum;
        Quaternion maximumToUse = isOnBottom ? bottomMaximum : topMaximum;

        Transform panelToUse = isOnBottom ? botPanel : topPanel;

        float differenceInMinimumAngle = Quaternion.Angle(panelToUse.rotation, minimumToUse);
        float differenceInMaximumAngle = Quaternion.Angle(panelToUse.rotation, maximumToUse);

        //Returns true if we're too close to the rotational bounds
        if (shouldStopAtThreshold && ((amountToRotate < 0 && differenceInMinimumAngle < minimumStopThreshold) || (amountToRotate > 0 && differenceInMaximumAngle < minimumStopThreshold)))
        {
            Debug.Log("We are too close to the edge; can't move any farther");
            return true;
        }
        return false;
    }
}
