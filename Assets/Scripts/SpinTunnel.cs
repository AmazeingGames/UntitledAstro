using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SpinTunnel : MonoBehaviour
{
    //SerializeField is virtually the same as using public, as it allows the variable to be set in the inspector, i.e. making it serialized, without the downside of having it be accessible via other scripts. This is considered a good coding practice. - Amazeing

    [SerializeField] float speed = 5;

    [SerializeField] bool shouldRotatePanels = true;
    [SerializeField] bool shouldStopAtThreshold = true;
    [SerializeField] float minimumStopThreshold = .5f;

    bool isOnBottom = true;

    readonly Quaternion bottomMinimum = new Quaternion(0.00000f, 0.00000f, -0.26234f, 0.96498f); //(-30)
    readonly Quaternion bottomMaximum = new Quaternion(0.00000f, 0.00000f, 0.70452f, 0.70968f); //(90)

    readonly Quaternion topMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion topMaximum = new Quaternion(0.00000f, 0.00000f, -0.70968f, 0.70452f); //(-90)

    readonly List<Transform> tunnels = new List<Transform>();
    readonly List<List<Transform>> listOfPanels = new();

    Vector3 tunnelCenter;

    void Start()
    {
        tunnelCenter = GameObject.Find("TunnelCenter").transform.position;

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

            foreach (Transform tunnel in tunnels)
            {
                //Flips all the tunnels by 180 degrees
                tunnel.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
            }
        }
    }

    //Rotates all the tunnels according to the horizontal axes keys (left, right)
    void RotateTunnels()
    {

        for (int i = 0; i < listOfPanels.Count; i++)
        {
            for (int n = 0; n < listOfPanels[i].Count; n++)
            {
                float input = Input.GetAxisRaw("Horizontal");

                var currentList = listOfPanels[i];
                var panel = currentList[n];

                float amountToRotate = speed * input * Time.deltaTime;
                panel.Rotate(0f, 0f, amountToRotate);
            }
        }
    }
}
