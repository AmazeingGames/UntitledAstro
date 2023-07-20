using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTunnel : MonoBehaviour
{
    public float speed = 5;

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
            tunnel.transform.Rotate(0f, 0f, speed * input * Time.deltaTime);

            Quaternion tunnelRotation = tunnel.transform.rotation;

            Debug.Log(tunnel.transform.rotation);

            Quaternion minimum = new Quaternion(0.00000f, 0.00000f, -0.26234f, 0.96498f);

            Quaternion maximum = new Quaternion(0.00000f, 0.00000f, 0.70452f, 0.70968f);





            //Mathf.Clamp(tunnel.transform.rotation, minimum, maximum);
        }


    }
}
