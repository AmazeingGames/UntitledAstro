using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTunnel : MonoBehaviour
{
    //SerializeField is virtually the same as using public, as it allows the variable to be set in the inspector, i.e. making it serialized, without the downside of having it be accessible via other scripts. This is considered a good coding practice. - Amazeing

    [SerializeField] float speed = 5;

    [SerializeField] bool shouldStopAtThreshold = true;
    [SerializeField] float minimumStopThreshold = .5f;

    bool isOnBottom = true;

    readonly Quaternion bottomMinimum = new Quaternion(0.00000f, 0.00000f, -0.26234f, 0.96498f); //(-30)
    readonly Quaternion bottomMaximum = new Quaternion(0.00000f, 0.00000f, 0.70452f, 0.70968f); //(90)

    readonly Quaternion topMinimum = new Quaternion(0.00000f, 0.00000f, 0.96498f, 0.26234f); //(150)
    readonly Quaternion topMaximum = new Quaternion(0.00000f, 0.00000f, -0.70968f, 0.70452f); //(-90)

    readonly List<Transform> tunnels = new List<Transform>();

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
        RotateTunnels();

        FlipTunnels();
    }

    void FlipTunnels()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isOnBottom = !isOnBottom;

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
        float input = Input.GetAxisRaw("Horizontal");

        foreach (Transform tunnel in tunnels)
        {
            Debug.Log(tunnel.rotation);

            float amountToRotate = speed * input * Time.deltaTime;

            /* This code looks more complicated than it really is.
             * What this does is if the given value (isOnBottom) is true, the value we're trying to assign becomes the value after the ? operator (bottomMaximum), else it becomes the value after the : operator (topMaximum)
             * It's essentially the same as:
             * 
             * if (isOnBottom == true)
             * {
             *    minimumToUse = bottomMinimum;
             *    maximumToUse = bottomMaximum;
             * }
             * else
             * {
             *   minimumToUse = topMinimum;
             *     maximumToUse = topMaximum;
             * }
            */

            Quaternion minimumToUse = isOnBottom ? bottomMinimum : topMinimum;
            Quaternion maximumToUse = isOnBottom ? bottomMaximum : topMaximum;

            //This code determines the angle of the tunnels and the tunnels stopping point, and returns an angle of how close they match.
            //The closer to 0, the closer the two angles are.
            float differenceInMinimumAngle = Quaternion.Angle(tunnel.rotation, minimumToUse);
            float differenceInMaximumAngle = Quaternion.Angle(tunnel.rotation, maximumToUse);

            //Only rotates if we're not close the rotational bounds
            if (shouldStopAtThreshold && ((amountToRotate < 0 && differenceInMinimumAngle < minimumStopThreshold) || (amountToRotate > 0 && differenceInMaximumAngle < minimumStopThreshold)))
            {
                Debug.Log("We are too close to the edge; can't move any farther");
                break;
            }
            tunnel.transform.Rotate(0f, 0f, amountToRotate);
        }
    }
}
