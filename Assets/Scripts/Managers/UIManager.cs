using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Manages the UI during a match
 */

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Text playerScoreText;
	[SerializeField]
	private Text cpuScoreText;
	[SerializeField]
	private Text playerTurnText;
	[SerializeField]
	private Text playerTurnScoreText;
	[SerializeField]
	private Text cpuTurnScoreText;

	private string player1turn = "Your turn";
	private string player2turn = "Andrea's turn";


	private void OnEnable()
	{
		GameMode.onUIUpdate += UpdateUI;
	}

	private void OnDisable()
	{
		GameMode.onUIUpdate -= UpdateUI;
	}

	private void UpdateUI(int playerScore, int cpuScore, int playerTurnScore, int cpuTurnScore, int dartsLeft, bool isPlayerTurn) 
	{
		playerScoreText.text = playerScore.ToString();
		cpuScoreText.text = cpuScore.ToString();
		playerTurnScoreText.text = playerTurnScore.ToString();
		cpuTurnScoreText.text = cpuTurnScore.ToString();
		playerTurnText.text = isPlayerTurn ? player1turn : player2turn;
	}
}
