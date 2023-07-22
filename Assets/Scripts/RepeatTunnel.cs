using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatTunnel : MonoBehaviour
{
    public float speed = 5;
    private Vector3 startPos;
    private GameOver GameOverScript;
    void Start()
    {
        startPos = transform.position;
        GameOverScript = GameObject.Find("Player").GetComponent<GameOver>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverScript.gameOver == false)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);

            if (transform.position.z > startPos.z + 5)
            {
                transform.position = startPos;
            }
        }
    }
}
