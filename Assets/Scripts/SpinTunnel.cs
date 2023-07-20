using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTunnel : MonoBehaviour
{
    //SerializeField is virtually the same as using public, as it allows the variable to be set in the inspector, i.e. making it serialized, without the downside of having it be accessible via other scripts. This is considered a good coding practice. - Amazeing

    [SerializeField] float speed = 5;

    [SerializeField] bool shouldStopAtThreshold = true;
    [SerializeField] float minimumStopThreshold = .5f;

    readonly Quaternion minimum = new Quaternion(0.00000f, 0.00000f, -0.26234f, 0.96498f);

    readonly Quaternion maximum = new Quaternion(0.00000f, 0.00000f, 0.70452f, 0.70968f);

    List<Transform> tunnels = new List<Transform>();
    void Start()
    {
        int tunnelsToGet = transform.childCount;

        for (int i = 0; i < tunnelsToGet; i++)
        {
            tunnels.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        foreach (Transform tunnel in tunnels)
        {
            float amountToRotate = speed * input * Time.deltaTime;

            //This code determines the angle of the tunnels and the tunnels stopping point, and returns an angle of how close they match.
            //The closer to 0, the closer the two angles are.
            float differenceInMinimumAngle = Quaternion.Angle(tunnel.rotation, minimum);
            float differenceInMaximumAngle = Quaternion.Angle(tunnel.rotation, maximum);

            //Only rotates if we're not close the rotational bounds
            if (shouldStopAtThreshold && ((amountToRotate < 0 && differenceInMinimumAngle < minimumStopThreshold) || (amountToRotate > 0 && differenceInMaximumAngle < minimumStopThreshold)))
            {
                Debug.Log("We are too close to the edge; can't move any farther");
                break;
            }
            tunnel.transform.Rotate(0f, 0f, amountToRotate);

            //Mathf.Clamp(tunnel.transform.rotation, minimum, maximum);
        }


    }
}
