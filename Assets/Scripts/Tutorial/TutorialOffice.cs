using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialOffice : Tutorial
{
	public static TutorialOffice instance;

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
		textBox.SetActive(true);
		scorePanel.SetActive(false);
		grandmaIcon.SetActive(true);
		text.text = tutorial[0];
		StartCoroutine(DragDelay());
	}

	public override void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance)
		{
			case 1:
				text.text = tutorial[1];
				break;
			case 2:
				text.text = tutorial[2];
				break;
			case 3:
				text.text = tutorial[3];
				break;
			case 4:
				text.text = tutorial[4];
				break;
			case 5:
				GameManager.instance.tutorial = false;
				DartGenerator.instance.EnableDart();
				EndTutorial();
				textBox.SetActive(false);
				gameObject.SetActive(false);
				scorePanel.SetActive(true);
				gameMode.playerScore = 6;
				gameMode.cpuScore = 6;
				gameMode.UpdateUI();
				break;
			default:

				textBox.SetActive(false);
				break;
		}
		CheckAdvance();
	}
}
