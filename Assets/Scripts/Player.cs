using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem explosionParticle1;
    public ParticleSystem explosionParticle2;
    private GameOver gameOver;
    public GameObject gameManager;
    public GameObject playerModel;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameOver = gameManager.GetComponent<GameOver>();
        gameOver.GameOverEvent += ExplodeShip;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void ExplodeShip()
    {
        playerModel.SetActive(false);
        explosionParticle1.Play();
        explosionParticle2.Play();
    }

}
