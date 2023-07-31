using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody Rb;
    public float speed = 1.0f;
    [SerializeField] Vector3 startingPosition;

    public Transform projectileModel { get; private set; }

    private void Awake()
    {
        projectileModel = transform.GetChild(0);
    }

    void Start()
    {
        Rb = projectileModel.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Rb.velocity = new Vector3(0, 0, -speed);
    }

    public void SetStartingPosition()
    {
        projectileModel.localPosition = startingPosition;
    }
}
