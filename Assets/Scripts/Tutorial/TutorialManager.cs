using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Level level;
	public GameObject tutorialManager;

	private void Awake()
	{
		if (PlayerPrefs.GetInt(level.location.ToString()) == 0)
			tutorialManager.SetActive(true);
	}
}
