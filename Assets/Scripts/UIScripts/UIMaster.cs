using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMaster : MonoBehaviour
{
    //Timer 
    public TextMeshProUGUI timerText;
    public float currentTime = 0f;
    public int finalTime;

    //HighScore
    public TextMeshProUGUI highscore;

    //Pause Menu
    private GameObject pausemenu;
    bool isGamePaused = false;


    void Start()
    {
        pausemenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pausemenu.SetActive(false);
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timerText.SetText(currentTime.ToString("F2"));

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (isGamePaused)
            {
                PauseDeactivate();
            }
            else
            {
                PauseActivate();
            }

        }
        
      


    }


    void PauseActivate()
    {

            isGamePaused = true;
            pausemenu.SetActive(true);
            Time.timeScale = 0;
       
       
    }

    void PauseDeactivate()
    {

            isGamePaused = false;
            pausemenu.SetActive(false);
            Time.timeScale = 1;

        

    }

    void QuitGame()
    {
        Application.Quit();
    }
}
