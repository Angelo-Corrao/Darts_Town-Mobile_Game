using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialArena : Tutorial
{

	[SerializeField]
	private GameObject welcomeMessage;

	public static TutorialArena instance;


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
				break;
			case 2:
				GameManager.instance.tutorial = false;
				DartGenerator.instance.EnableDart();
				textBox.SetActive(false);
				gameObject.SetActive(false);
				scorePanel.SetActive(true);
				gameMode.playerScore = 100;
				gameMode.cpuScore = 100;
				gameMode.startingPlayerPoints = 100;
				gameMode.startingCpuPoints = 100;
				gameMode.UpdateUI();
				EndTutorial();
				break;
			default:
				textBox.SetActive(false);
				break;
		}
		CheckAdvance();
	}
}