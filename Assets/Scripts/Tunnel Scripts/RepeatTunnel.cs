using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    public float speed = 5;
    public float behindPlayer = -2;

    public Transform LastTunnel { get; private set; }

    readonly List<Transform> tunnels = new();
    float small;
    private void Awake()
    {
        LastTunnel = transform.GetChild(transform.childCount - 1);
    }
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
            Transform tunnelToTeleport = tunnels[i];

            if (tunnelToTeleport.transform.position.z >= behindPlayer)
            {
                float smallestZ = tunnels[0].transform.position.z;

                for (int n = 0; n < tunnels.Count; n++)
                {
                    if (smallestZ > tunnels[n].transform.position.z)
                    {
                        smallestZ = LastTunnel.transform.position.z;

                        small = smallestZ;
                    }
                }

                float lengthOfPanel = tunnelToTeleport.GetChild(0).GetComponent<Collider>().bounds.size.z;

                LastTunnel = tunnels[i];
                tunnels[i].transform.position = new Vector3(0, 0, smallestZ - lengthOfPanel);
            }
        }
    }

    public Transform GetRandomPanel()
    {
        return GetRandomPanel(LastTunnel);
    }

    public Transform GetRandomPanel(Transform tunnel)
    {
        int randomNumber = Random.Range(0, tunnel.childCount);

        randomNumber = 3;

        return tunnel.GetChild(randomNumber);
    }
}
