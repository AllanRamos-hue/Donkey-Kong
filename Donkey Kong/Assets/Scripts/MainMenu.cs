using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip startingMusic;

    private void Start()
    {
        AudioManager.PlayBGS(startingMusic, false);
    }
  
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);       
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
