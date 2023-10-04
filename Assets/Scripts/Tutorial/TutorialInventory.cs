using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInventory : MonoBehaviour
{
	public Text text;
	public GameObject textBox;
	public Button upgradeTabButton;
	public Button tipsTabButton;
	public Button bodiesTabButton;
	public Button flightsTabButton;
	public GameObject upgradeTabContent;
	public GameObject flightsTabContent;
	public Button backButton;
	public Button topUpButton;
	public GameObject finalTutorialDialogue;
	public GameObject grandmaIcon;

	public static TutorialInventory instance;

	public bool inventoryTutorial = true;
	public string[] tutorial;

	[HideInInspector]
	public int tutorialAdvance = 0;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("TutorialShop") != 2 && PlayerPrefs.GetInt("TutorialInventory") != 7)
		{
			PlayerPrefs.SetInt("TutorialInventory", 0);
			textBox.SetActive(true);
			grandmaIcon.SetActive(true);
			upgradeTabContent.SetActive(false);
			backButton.interactable = false;
			topUpButton.interactable = false;
		}
		else 
		{
			textBox.SetActive(false);
			grandmaIcon.SetActive(false);
		}

		if (PlayerPrefs.GetInt("TutorialInventory") == 7) 
		{
			grandmaIcon.SetActive(true);
			finalTutorialDialogue.SetActive(true);
		}

		text.text = tutorial[PlayerPrefs.GetInt("TutorialInventory") == 7 ? 2 : 0];
		grandmaIcon.SetActive(true);
	}

	public void ChangeText()
	{
		PlayerPrefs.SetInt("TutorialInventory", PlayerPrefs.GetInt("TutorialInventory") + 1);
		switch (PlayerPrefs.GetInt("TutorialInventory"))
		{
			case 1:
				upgradeTabButton.interactable = false;
				bodiesTabButton.interactable = false;
				tipsTabButton.interactable = false;
				textBox.SetActive(false);
				grandmaIcon.SetActive(false);
				break;
			case 2:
				flightsTabButton.interactable = false;
				textBox.SetActive(false);
				break;
			case 3:
				text.text = tutorial[1];
				textBox.SetActive(true);
				grandmaIcon.SetActive(true);
				break;
			case 4:
				upgradeTabButton.interactable = true;
				flightsTabContent.SetActive(false);
				textBox.SetActive(false);
				grandmaIcon.SetActive(false);
				break;
			case 5:
				upgradeTabButton.interactable = false;
				text.text = tutorial[2];
				textBox.SetActive(true);
				grandmaIcon.SetActive(true);
				break;
			case 6:
				textBox.SetActive(false);
				grandmaIcon.SetActive(false);
				break;
			case 7:
				finalTutorialDialogue.SetActive(true);
				grandmaIcon.SetActive(true);
				break;
			case 8:
				finalTutorialDialogue.SetActive(false);
				textBox.SetActive(false); 
				grandmaIcon.SetActive(false); 
				PlayerPrefs.SetInt("TutorialShop", 2);
				backButton.interactable = true;
				topUpButton.interactable = true;
				tipsTabButton.interactable = true;
				bodiesTabButton.interactable = true;
				flightsTabButton.interactable = true;
				upgradeTabContent.SetActive(true);
				gameObject.SetActive(false);
				break;

		}
	}
}
