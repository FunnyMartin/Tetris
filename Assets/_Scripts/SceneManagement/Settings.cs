using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    
    //Loads scene "MainMenu".
    public void MainMenuButton()
    {
        try
        {
            AudioManager.instance.PlaySFX("Click");
            SceneManager.LoadScene("MainMenu");
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Checks every frame for input, opens MainMenu.
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenuButton();
        }
    }

    //Sets the volume of music by slider and saves it to PlayerPref "MusicVolume".
    public void MusicVolume()
    {
        try
        {
            AudioManager.instance.MusicVolume(musicSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //Sets the volume of sfx by slider and saves it to PlayerPref "SFXVolume".
    public void SFXVolume()
    {
        try
        {
            AudioManager.instance.SFXVolume(sfxSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }

    //On start loads the value of sliders by PlayerPrefs.
    private void Start()
    {
        try
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
    }
}
