using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public void PauseGame(bool mainMenu)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        GameObject[] tapCircles = GameObject.FindGameObjectsWithTag("Tap Circle");

        foreach (GameObject tapCircle in tapCircles)
        {
            Destroy(tapCircle);
        }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;

            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.UnPause();
            }
        }
        else
        {
            Time.timeScale = 0;

            foreach (AudioSource audioS in allAudioSources)
            {
                if (audioS.name != "Upgrades Manager")
                {
                    audioS.Pause();
                }
            }
        }

        if (mainMenu)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
