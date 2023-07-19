using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    public Projectile projectile;
    public GameObject SpawnProjectile;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnProjectile = GameObject.Find("SpawnProjectile");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
            Instantiate(projectile, SpawnProjectile.transform.position, SpawnProjectile.transform.rotation);
        }
    }
}
