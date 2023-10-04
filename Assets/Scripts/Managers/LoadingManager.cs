using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private GameObject tutorialManager;

	private void Start()
	{
		StartCoroutine(LoadGame());
	}

	private IEnumerator LoadGame() 
	{
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		if (PlayerPrefs.GetInt("grandmaHouse") == 0)
			tutorialManager.SetActive(true);
		else
			SceneManager.LoadScene(1);
	}
}
