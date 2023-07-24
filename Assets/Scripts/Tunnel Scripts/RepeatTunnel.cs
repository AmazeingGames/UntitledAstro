using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    public float speed = 5;
    public float behindplayer = 14;
    public float teleporttoZ;
    List<Transform> tunnels = new List<Transform>();
    private Transform lastTunnel;
    public bool shouldmovetunnel = true;
    void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tunnel = transform.GetChild(i);
            tunnels.Add(tunnel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        for (int i = 0; i < tunnels.Count; i++)
        {
            Transform teleport = tunnels[i];
            if (teleport.transform.position.z >= behindplayer)
            {
                float smallestZ = tunnels[0].transform.position.z;
                for (int n = 0; n < tunnels.Count; n++)
                {
                    if (smallestZ > tunnels[n].transform.position.z)
                    {
                        smallestZ = tunnels[n].transform.position.z;
                        lastTunnel = tunnels[n];
                    }
                }

                float lengthofpanel = teleport.GetChild(0).GetComponent<Collider>().bounds.size.z;

                tunnels[i].transform.position = new Vector3(0, 0, smallestZ - lengthofpanel);
                    
            }
        }
    }
}
