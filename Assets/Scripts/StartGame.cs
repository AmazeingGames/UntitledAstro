using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    GameObject mainMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas = GameObject.Find("MainMenuCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenuCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mainMenuCanvas.SetActive(false);
            }
        }


        
    }
}
