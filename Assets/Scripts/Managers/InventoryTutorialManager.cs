using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTutorialManager : MonoBehaviour
{
	[SerializeField]
	private GameObject tutorialManager;


	public void Start()
	{
		if(PlayerPrefs.GetInt("TutorialShop") == 1)
			tutorialManager.SetActive(true);
	}
}
