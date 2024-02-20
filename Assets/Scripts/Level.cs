using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	public GameObject modeMenuLv1;
	public GameObject modeMenuLv2;
	public int selectedLevel = 0;

	public void ChooseLevel(int level)
	{
		selectedLevel = level;
		switch (selectedLevel)
		{
			case 1:
				
				modeMenuLv1.SetActive(true);
				modeMenuLv2.SetActive(false);
				break;
			case 2:
				
				modeMenuLv1.SetActive(false);
				modeMenuLv2.SetActive(true);
				break;
		}


	}

	public void ChooseMode(int mode)
	{
		int sceneIndex = (selectedLevel - 1) * 2 + mode + 1;
		SceneManager.LoadScene(sceneIndex);
		modeMenuLv1.SetActive(false);
		modeMenuLv2.SetActive(false);
	}
}