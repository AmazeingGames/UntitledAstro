using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    public float zModifier = 5;

    private Vector3 startPos;


    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startPos.z + zModifier)
        {
            transform.position = startPos;
        }
    }
}
