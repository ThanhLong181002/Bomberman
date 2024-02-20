using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject pauseMenuUI;
    public GameObject Audio;
    public GameObject BtnPlaySound;
    public GameObject BtnStopSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPause = false;
	}
    public void Pause()
	{
		pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPause= true;
	}

	public void Restart()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		GameIsPause = false;
	}

	public void StopAudio()
	{
		Audio.SetActive(false);
        BtnStopSound.SetActive(false);
        BtnPlaySound.SetActive(true);
	}
    public void StartAudio()
	{
		Audio.SetActive(true);
        BtnStopSound.SetActive(true);
		BtnPlaySound.SetActive(false);
	}

    public void GoHome()
    {
		SceneManager.LoadScene(0);
	}
	public void QuitGame()
    {
        Debug.Log("Quitting ....");
    }
}
