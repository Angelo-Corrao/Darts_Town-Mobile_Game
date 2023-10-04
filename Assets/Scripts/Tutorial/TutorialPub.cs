using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPub : Tutorial
{
	public GameObject welcomeMessage;

	public static TutorialPub instance;

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
				gameMode.playerScore = 100;
				gameMode.cpuScore = 100;
				gameMode.startingPlayerPoints = 100;
				gameMode.startingCpuPoints = 100;
				gameMode.UpdateUI();
				break;
			default:
				textBox.SetActive(false);
				break;
		}
		CheckAdvance();
	}
}
