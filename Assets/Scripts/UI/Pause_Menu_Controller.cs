using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu_Controller : MonoBehaviour
{

    private bool paused = false;
    public GameObject pauseMenuPanel; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
                paused = true;
            else
                paused = false;
        }

        if (paused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }




    public void resumeGame()
    {

        paused = false;

    }



}
