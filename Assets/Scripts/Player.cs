using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem explosionParticle1;
    public ParticleSystem explosionParticle2;
    // Start is called before the first frame update
    void Start()
    { 
    
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        explosionParticle1.Play();
        explosionParticle2.Play();
    }
}
