using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    //An event is pretty much a way to notify other scripts that something happened so that script can respond by calling its own function
    public event Action<Transform> ResetTunnel;
    public float speed = 5;
    public float behindPlayer = -2;

    public bool canMove = true;
    [SerializeField] bool fixedRandom = false;
    [SerializeField] int fixedRandomNumber;
    public Transform LastTunnel { get; private set; }

    readonly List<Transform> tunnels = new();

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
        if (canMove)
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
                        }
                    }

                    float lengthOfPanel = tunnelToTeleport.GetChild(0).GetComponent<Collider>().bounds.size.z;

                    LastTunnel = tunnelToTeleport;
                    tunnelToTeleport.transform.position = new Vector3(0, 0, smallestZ - lengthOfPanel);

                    ResetTunnel?.Invoke(tunnelToTeleport);

                    if (ResetTunnel == null)
                        Debug.LogWarning("ResetTunnelNull");
                }
            }
        }
    }

    public Transform GetRandomPanel()
    {
        return GetRandomPanel(LastTunnel);
    }

    public Transform GetRandomPanel(Transform tunnel)
    {
        int randomNumber = UnityEngine.Random.Range(0, tunnel.childCount);

        if (fixedRandom)
            randomNumber = fixedRandomNumber;

        return tunnel.GetChild(randomNumber);
    }

    public static List<Transform> GetPanelsFromTunnel(Transform tunnel)
    {
        var panelsList = new List<Transform>();

        for (int i = 0; i < tunnel.childCount; i++)
            panelsList.Add(tunnel.GetChild(i));

        return panelsList;
    }
}
