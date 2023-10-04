using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages the game with Fives rules
 */

public class GrandmaHouseMode : GameMode
{
	protected override void Awake()
	{
		base.Awake();
		dartboardHitSFX = new string[] { "Darts Doily 1", "Darts Doily 2", "Darts Doily 3" };
	}

	private void Update()
	{
		if (!GameManager.instance.tutorial && !pauseTimer)
		{
			timer -= Time.deltaTime;
			timerImage.fillAmount = Mathf.Clamp(timer / turnTimer, 0, 1);

			if (timer <= 10 && !isTurnTimerSFXPlaying)
			{
				AudioManager.Instance.PlaySFX("Turn Timer");
				isTurnTimerSFXPlaying = true;
			}

			if (timer <= 0)
			{
				isTurnTimerSFXPlaying = false;

				if (isPlayerTurn)
					dartsPerTurnPlayer1[dartsPerTurnPlayer1.Length - dartsLeft].sprite = emptySprite;
				else
					dartsPerTurnPlayer2[dartsPerTurnPlayer2.Length - dartsLeft].sprite = emptySprite;
				dartsLeft -= 1;
				TimerExpire();
				if (dartsLeft == 0)
				{
					if (isPlayerTurn)
						ChangeTurn(ref playerScore, ref startingPlayerPoints, ref playerTurnScore);

					else
						ChangeTurn(ref cpuScore, ref startingCpuPoints, ref cpuTurnScore);
				}
				UpdateUI();
				timer = turnTimer;
			}
		}
	}

	protected override void OnTargetHitManager(Collision collision)
	{
		timer = turnTimer;
		isTurnTimerSFXPlaying = false;

		if (isPlayerTurn)
			GameModeManagement(ref playerScore, ref playerTurnScore, ref startingPlayerPoints, collision);
		else
			GameModeManagement(ref cpuScore, ref cpuTurnScore, ref startingCpuPoints, collision);
	}

	private void GameModeManagement(ref int score, ref int turnScore, ref int startingPoints, Collision collision)
	{
		if (collision.gameObject.GetComponent<TargetArea>() != null)
		{
			AudioManager.Instance.StopSFXSource();
			AudioManager.Instance.PlaySFX(dartboardHitSFX[Random.Range(0, dartboardHitSFX.Length)]);
			GameObject scoreUI = Instantiate(scoreTextPopUp, collision.GetContact(0).point - new Vector3(0, -0.1f, 0.5f), Quaternion.identity);
			scoreUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = collision.gameObject.GetComponent<TargetArea>().score.ToString();
			StartCoroutine(DisableScoreText(scoreUI));
			turnScore += collision.gameObject.GetComponent<TargetArea>().score;
			if (isPlayerTurn)
			{
				switch (collision.gameObject.GetComponent<TargetArea>().section)
				{
					case TargetArea.DartboardSection.Bullseye:
						PlayerPrefs.SetInt("bullseyeScored", PlayerPrefs.GetInt("bullseyeScored") + 1);
						break;
					case TargetArea.DartboardSection.Double:
						PlayerPrefs.SetInt("doubleScored", PlayerPrefs.GetInt("doubleScored") + 1);
						Debug.Log(PlayerPrefs.GetInt("doubleScored"));
						break;
					case TargetArea.DartboardSection.Triple:
						PlayerPrefs.SetInt("triplesScored", PlayerPrefs.GetInt("triplesScored") + 1);
						break;
				}

				if (turnScore == 180)
					PlayerPrefs.SetInt("maxPointsInARound", PlayerPrefs.GetInt("maxPointsInARound") + 1);
			}
		}
		else
			AudioManager.Instance.PlaySFX(wallHitSfx[Random.Range(0, wallHitSfx.Length)]);

		// Check if the game is ended
		if (score - turnScore <= 0)
			EndGame();



		if (isPlayerTurn)
			dartsPerTurnPlayer1[dartsPerTurnPlayer1.Length - dartsLeft].sprite = emptySprite;
		else
			dartsPerTurnPlayer2[dartsPerTurnPlayer2.Length - dartsLeft].sprite = emptySprite;
		dartsLeft--;

		if (dartsLeft == 0)
			ChangeTurn(ref score, ref startingPoints, ref turnScore);

		UpdateUI();
	}

	private void ChangeTurn(ref int score, ref int startingPoints, ref int turnScore)
	{
		score -= turnScore;
		turnScore = 0;
		startingPoints = score;
		RefreshPlayerDarts();
		isPlayerTurn = !isPlayerTurn;
		ChangePlayerTurnIndicator();
		DartGenerator.instance.DeleteDarts(maxDartsPerTurn);
		AudioManager.Instance.PlaySFX("Darts Take Back");
		dartsLeft = maxDartsPerTurn;
	}
}

