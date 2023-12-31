using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private System.Random random = new System.Random();
    string[] array = { "TypeA", "TypeB" };

    //Loads scene "Game".
    public void PlayGame()
    {
        try
        {
            AudioManager.instance.PlaySFX("Click");
            AudioManager.instance.musicSource.Stop();
            playRandomTrack();
            SceneManager.LoadScene("Game");
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Picks random track from array and plays it.
    private void playRandomTrack()
    {
        try
        {
            int index = random.Next(array.Length);
            string picked = array[index];
            AudioManager.instance.PlayMusic(picked);
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

    //Loads scene "Settings".
    public void Settings()
    {
        try
        {
            AudioManager.instance.PlaySFX("Click");
            SceneManager.LoadScene("Settings");
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }  
    }
}
