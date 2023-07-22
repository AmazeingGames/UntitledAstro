using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    public GameObject SpawnProjectile;
    public AudioSource ShootingSound;
    public ObjectPool GameManager;
    // Start is called before the first frame update
    void Start()

    {
        GameManager = GameObject.Find("GameManager").GetComponent<ObjectPool>();
        SpawnProjectile = GameObject.Find("SpawnProjectile");
        ShootingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootingSound.Play();
            var projectile = GameManager.GetPooledObject();
            if (projectile != null)
            {
                projectile.SetActive(true);
                projectile.transform.position = SpawnProjectile.transform.position;
            }
            
        }
    }
}
