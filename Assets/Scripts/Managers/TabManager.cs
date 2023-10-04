using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> tabs;
	[SerializeField] private GameObject tutorialManager;

	public void OpenTab(int index)
	{
		for (int i = 0; i < tabs.Count; i++)
		{
			if (i == index)
				tabs[i].SetActive(true);
			else
				tabs[i].SetActive(false);
		}
		
		if (tutorialManager.activeSelf)
			TutorialInventory.instance.ChangeText();
	}
}
