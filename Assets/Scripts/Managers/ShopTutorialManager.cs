using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorialManager : MonoBehaviour
{
	[SerializeField] private GameObject tutorialManager;
	[SerializeField] private GameObject tutorialLootboxButton;

	private void Awake()
	{
		if (PlayerPrefs.GetInt("TutorialShop") == 0) 
		{
			tutorialLootboxButton.SetActive(true);
			tutorialManager.SetActive(true);
		}

	}
}
