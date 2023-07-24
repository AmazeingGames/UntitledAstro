using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;
using Input = UnityEngine.Input;

public class SpinTunnel : MonoBehaviour
{
    //SerializeField is virtually the same as using public, as it allows the variable to be set in the inspector, i.e. making it serialized, without the downside of having it be accessible via other scripts. This is considered a good coding practice. - Amazeing

    [SerializeField] float speed = 5;

    [SerializeField] bool shouldRotatePanels = true;
    [SerializeField] bool shouldStopAtThreshold = true;
    [SerializeField] float minimumStopThreshold = .5f;

    [SerializeField] Transform botPanel;
    [SerializeField] Transform topPanel;
    bool isOnBottom = true;

    readonly Quaternion bottomMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion bottomMaximum = new Quaternion(0.00000f, 0.00000f, 0.70968f, -0.70452f); //(90)

    readonly Quaternion topMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion topMaximum = new Quaternion(0.00000f, 0.00000f, -0.70968f, 0.70452f); //(270)

    readonly List<Transform> tunnels = new List<Transform>();
    readonly List<List<Transform>> listOfPanels = new();

    void Start()
    {
        int tunnelsToGet = transform.childCount;

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
        RotateTunnels();

        FlipTunnels();
    }

    void FlipTunnels()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isOnBottom = !isOnBottom;

            Debug.Log("We Flipped!");

            for (int i = 0; i < listOfPanels.Count; i++)
            {
                for (int n = 0; n < listOfPanels[i].Count; n++)
                {
                    //Flips all the tunnels by 180 degrees
                    var panel = listOfPanels[i][n];
                    panel.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
                }
            }
        }
    } 

    //Rotates all the tunnels according to the horizontal axes keys (left, right)
    void RotateTunnels()
    {
        Debug.Log("Bot Panel: " + botPanel.rotation);
        Debug.Log("Top Panel: " + topPanel.rotation);
        
        float input = Input.GetAxisRaw("Horizontal");
        float amountToRotate = speed * input * Time.deltaTime;

        if (ShouldStopRotating(amountToRotate))
            return;

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
