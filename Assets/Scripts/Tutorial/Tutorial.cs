using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]

public abstract class Tutorial : MonoBehaviour
{
	public delegate void OnTutorialEnd();
	public static event OnTutorialEnd onTutorialEnd;

	[SerializeField]
	protected string[] tutorial;
	[SerializeField]
	protected Text text;
	[SerializeField]
	protected GameObject textBox;
	[SerializeField]
	protected GameObject scorePanel;
	[SerializeField]
	protected GameObject backButton;
	[SerializeField]
	protected GameObject grandmaIcon;
	[SerializeField]
	protected GameMode gameMode;
	[SerializeField]
	protected Level level;

	[HideInInspector]
	public int tutorialAdvance = 0;

	public abstract void ChangeText();

	public void CheckAdvance()
	{
		GameManager.instance.tutorialCheck = tutorialAdvance;
	}

	public IEnumerator DragDelay()
	{
		yield return new WaitForSeconds(4.2f);
		DartGenerator.instance.DisableDart();
	}

	protected void EndTutorial() 
	{
		onTutorialEnd?.Invoke();
	}

}
