using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTunnel : MonoBehaviour
{
    public float speed = 5;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0f, 0f, (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0f, 0f, -(speed * Time.deltaTime));
        }
    }
}
