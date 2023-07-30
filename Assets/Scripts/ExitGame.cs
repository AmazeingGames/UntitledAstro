using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void QuitGame()
    {
        Debug.Log("quit"); 
        Application.Quit();
        
    }
}
