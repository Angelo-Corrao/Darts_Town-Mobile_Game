using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialMusicShop : Tutorial
{

	[SerializeField] 
	private GameObject welcomeMessage;

	public static TutorialMusicShop instance;


	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		GameManager.instance.tutorial = true;
		scorePanel.SetActive(false);
		grandmaIcon.SetActive(true);
		welcomeMessage.SetActive(true);
		StartCoroutine(DragDelay());
	}

	public override void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance)
		{
			case 1:
				welcomeMessage.SetActive(false);
				textBox.SetActive(true);
				text.text = tutorial[0];
				break;
			case 2:
				text.text = tutorial[1];
				break;
			case 3:
				text.text = tutorial[2];
				break;
			case 4:
				GameManager.instance.tutorial = false;
				DartGenerator.instance.EnableDart();
				EndTutorial();
				textBox.SetActive(false);
				gameObject.SetActive(false);
				scorePanel.SetActive(true);
				gameMode.UpdateUI();
				break;
			default:
				textBox.SetActive(false);
				break;
		}
		CheckAdvance();
	}
}
