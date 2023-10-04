using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialShop : MonoBehaviour
{
	public Text text;
	public GameObject textBox;
	public GameObject grandmaIcon;

	public static TutorialShop instance;

	public bool shopTutorial = true;
	public string[] tutorial;

	[HideInInspector]
	public int tutorialAdvance = 0;

	public Button[] shopButtons;
	public Button backButton;
	public Button topUpButton;
	public Button tutorialLootBoxButton;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		textBox.SetActive(true);
		text.text = tutorial[0];
		grandmaIcon.SetActive(true);
	}

	public void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance) 
		{
			case 1:
				PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + 50);
				CurrencyManager.instance.UpdateCurrency();
				AudioManager.Instance.PlaySFX("Premium Currency Gain");
				for (int i = 0; i < shopButtons.Length; i++) 
				{
					shopButtons[i].interactable = false;
				}
				backButton.interactable = false;
				topUpButton.interactable = false;
				textBox.SetActive(false);
				grandmaIcon.SetActive(false);
				break;
			case 2:
				text.text = tutorial[1];
				textBox.SetActive(true);
				grandmaIcon.SetActive(true);
				break;
			case 3:
				textBox.SetActive(false);
				//PlayerPrefs.SetInt("TutorialShop", 1);
				tutorialLootBoxButton.interactable= false;
				backButton.interactable = true;
				grandmaIcon.SetActive(false);
				break;
		}

	}
}
