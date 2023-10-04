using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHub : MonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField] 
	private Animator GrandmaHouseTutorial;
	[SerializeField] 
	private Animator shopTutorial;
	[SerializeField] 
	private Animator inventoryTutorial;
	[SerializeField] 
	private GameObject topUpButton;
	[SerializeField] 
	private GameObject grandmaIcon;

	public static TutorialHub instance;

	public bool hubTutorial = true;
	[SerializeField]private GameObject[] tutorialDialogues;

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
		grandmaIcon.SetActive(true);
	}

	public void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance)
		{
			case 0:
				tutorialDialogues[0].SetActive(true);
				break;
			case 1:
				tutorialDialogues[0].SetActive(false);
				tutorialDialogues[1].SetActive(true);
				break;
			case 2:
				tutorialDialogues[1].SetActive(false);
				tutorialDialogues[2].SetActive(true);
				break;
			case 3:
				tutorialDialogues[2].SetActive(false);
				GrandmaHouseTutorial.enabled = true;
				grandmaIcon.SetActive(false);
				break;
			case 4:
				tutorialDialogues[3].SetActive(true);
				break;
			case 5:
				tutorialDialogues[3].SetActive(false);
				tutorialDialogues[4].SetActive(true);
				break;
			case 6:
				shopTutorial.enabled = true;
				tutorialDialogues[4].SetActive(false);
				grandmaIcon.SetActive(false);
				break;
			case 7:
				tutorialDialogues[5].SetActive(true);
				break;
			case 8:
				tutorialDialogues[5].SetActive(false);
				inventoryTutorial.enabled = true;
				topUpButton.SetActive(false);
				grandmaIcon.SetActive(false);
				break;
		}
	}

	public void AdvanceTutorial(int dialogue) 
	{
		tutorialAdvance = dialogue;
		ChangeText();
	}
}
