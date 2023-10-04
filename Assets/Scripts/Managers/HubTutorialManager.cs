using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTutorialManager : MonoBehaviour
{
	[SerializeField]
	private GameObject tutorialManager;
	

	private void Awake()
	{
		if (PlayerPrefs.GetInt("grandmaHouse") == 1)
		{
			tutorialManager.SetActive(true);
			TutorialHub.instance.AdvanceTutorial(-1);
		}
		else if (PlayerPrefs.GetInt("totalMatchesPlayed") >= 3 && PlayerPrefs.GetInt("TutorialShop") == 0)
		{
			tutorialManager.SetActive(true);
			TutorialHub.instance.AdvanceTutorial(3);
		}
		else if (PlayerPrefs.GetInt("TutorialShop") == 1)
		{
			tutorialManager.SetActive(true);
			TutorialHub.instance.AdvanceTutorial(6);
		}
	}
}
