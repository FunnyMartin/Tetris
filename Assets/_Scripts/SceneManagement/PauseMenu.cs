using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Image img;
    
    //Checks every frame for input and resumes or pauses game.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOver.gameIsOver)
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //Disables pause menu UI, sets time scale to 1, bool gameIsPaused is set to false.
    public void Resume()
    {
        try
        {
            img.enabled = true;
            AudioManager.instance.PlaySFX("Click");
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Enables pause menu UI, sets time scale to 0, bool gameIsPaused is set to true.
    public void Pause()
    {
        try
        {
            img.enabled = false;
            AudioManager.instance.PlaySFX("Click");
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }  
    }

    //Resumes the time scale and loads scene "MainMenu".
    public void loadMenu()
    {
        try
        {
            Resume();
            AudioManager.instance.PlayMusic("MainMenu");
            SceneManager.LoadScene("MainMenu");
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }   
    }

    //Quits the application.
    public void ExitGame()
    {
        Application.Quit();
    }
}
