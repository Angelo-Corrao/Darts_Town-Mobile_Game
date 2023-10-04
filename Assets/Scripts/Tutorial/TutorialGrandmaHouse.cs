using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialGrandmaHouse : Tutorial
{
	[SerializeField] 
	private GameObject swipeGestureImage;
	[SerializeField] 
	private GameObject dragGestureImage;

	public static TutorialGrandmaHouse instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void OnEnable()
	{
		GameManager.instance.tutorial = true;
	}

	private void Start()
	{
		scorePanel.SetActive(false);
		StartCoroutine(DragDelay());
		grandmaIcon.SetActive(true);
		textBox.SetActive(true);
		text.text = tutorial[0];
		dragGestureImage.SetActive(true);
		AudioManager.Instance.PlayMusic("In Game");
	}

	public override void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance)
		{
			case 1:
				DartGenerator.instance.EnableDart();
				textBox.SetActive(false);
				GameManager.instance.HideGesture();
				dragGestureImage.SetActive(false);
				break;
			case 2:
				text.text = tutorial[1];
				textBox.SetActive(true);
				GameManager.instance.phase = GameManager.Phase.swipe;
				GameManager.instance.ShowGesture(0.1f);
				break;
			case 3:
				textBox.SetActive(false);
				GameManager.instance.HideGesture();
				break;
			case 4:
				text.text = tutorial[2];
				textBox.SetActive(true);
				break;
			case 5:
				DartGenerator.instance.DisableDart();
				text.text = tutorial[3];
				break;
			case 6:
				text.text = tutorial[4];
				break;
			case 7:
				text.text = tutorial[5];
				break;
			case 8:
				GameManager.instance.tutorial = false;
				DartGenerator.instance.EnableDart();
				textBox.SetActive(false);
				gameObject.SetActive(false);
				scorePanel.SetActive(true);
				backButton.SetActive(false);
				gameMode.playerScore = 100;
				gameMode.cpuScore = 100;
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
