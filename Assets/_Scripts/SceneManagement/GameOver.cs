using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameOver : MonoBehaviour
{
    public TMP_Text scoreText;
    public static bool gameIsOver = false;

    //Sets game over panel and shows score.
    public void Setup(int score)
    {
        try
        {
            gameIsOver = true;
            gameObject.SetActive(true);
            scoreText.text = score.ToString() + " POINTS";
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Restart button function, restarts the game, resets time scale.
    public void RestartButton()
    {
        try
        {
            AudioManager.instance.PlaySFX("Click");
            gameIsOver = false;
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Menu button function, takes you to menu, resets time scale.
    public void MenuButton()
    {
        try
        {
            AudioManager.instance.PlaySFX("Click");
            AudioManager.instance.PlayMusic("MainMenu");
            gameIsOver = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }
}
