using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] players;
    public GameObject[] ais;

	/*private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/

	private void Start()
	{
		Instance= this;
	}
	public void CheckWinState()
    {
        int playerAliveCount = 0;
        int aiAliveCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeSelf) {
				playerAliveCount++;
            }
            
        }
        Debug.Log(playerAliveCount);
        foreach (GameObject ai in ais)
        {
            if (ai.activeSelf) {
				aiAliveCount++;
            }
        }
        Debug.Log(aiAliveCount);

        if (0 < playerAliveCount && playerAliveCount < 2 && aiAliveCount == 0 ) {
            Invoke(nameof(WinStage), 3f);
        }
        else if (playerAliveCount == 0)
		{
			Invoke(nameof(LoseStage), 3f);
		}
	}

    private void WinStage()
    {
        SceneManager.LoadScene(6);
    }
    private void LoseStage()
    {
        SceneManager.LoadScene(7);
    }

}
