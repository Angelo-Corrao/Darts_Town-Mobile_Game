using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialStart : MonoBehaviour
{
	[SerializeField]
	private GameObject inputField;
	[SerializeField]
	private GameObject secondDialogue;
	[SerializeField]
	private GameObject thirdDialogue;

	[SerializeField]
	private GameObject logo;
	[SerializeField]
	private GameObject loadingIcon;
	[SerializeField]
	private GameObject grandmaIcon;

	private int tutorialAdvance = 0;

	private void Start()
	{
		inputField.SetActive(true);
		logo.SetActive(false);
		loadingIcon.SetActive(false);
		grandmaIcon.SetActive(true);
	}

	public void ChangeText()
	{
		tutorialAdvance++;
		switch (tutorialAdvance)
		{
			case 1:
				inputField.SetActive(false);
				secondDialogue.SetActive(true);
				break;
			case 2:
				secondDialogue.SetActive(false);
				thirdDialogue.SetActive(true);
				break;
			case 3:
				thirdDialogue.SetActive(false);
				logo.SetActive(true);
				grandmaIcon.SetActive(false);
				SceneManager.LoadScene(3);
				break;
			
		}
	}

	public void SavePlayerName(string userName)
	{
		PlayerPrefs.SetString("playerName", userName);
	}
}
