using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    public float speed = 5;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        if (transform.position.z > startPos.z + 5)
        {
            transform.position = startPos;
        }
    }
}
