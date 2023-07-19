using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startPos.z + 5)
        {
            transform.position = startPos;
        }
    }
}
