using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Manages the game with Belgian Darts rules
 */

public class MusicShopMode : GameMode
{
	[SerializeField]
	private int MusicStoreTurn = 5;

	protected override void Awake()
	{
		base.Awake();
		dartboardHitSFX = new string[] { "Darts Vinyl 1", "Darts Vinyl 2", "Darts Vinyl 3" };
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
					if (MusicStoreTurn == 0 && !isPlayerTurn)
						EndGame();

					if (isPlayerTurn)
						ChangeTurn();

					else
						ChangeTurn();
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
		{
			GameModeManagement(ref playerScore, collision);
		}

		else
		{
			GameModeManagement(ref cpuScore, collision);
		}
	}

	private void GameModeManagement(ref int score, Collision collision)
	{
		if (collision.gameObject.GetComponent<TargetArea>() != null)
		{
			AudioManager.Instance.StopSFXSource();
			AudioManager.Instance.PlaySFX(dartboardHitSFX[Random.Range(0, dartboardHitSFX.Length)]);

			GameObject scoreUI = Instantiate(scoreTextPopUp, collision.GetContact(0).point - new Vector3(0, -0.2f, 1), Quaternion.identity);
			scoreUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = collision.gameObject.GetComponent<TargetArea>().score.ToString();
			StartCoroutine(DisableScoreText(scoreUI));
			score += collision.gameObject.GetComponent<TargetArea>().score;
			if (collision.gameObject.GetComponent<TargetArea>().section == TargetArea.DartboardSection.Bullseye && isPlayerTurn)
				PlayerPrefs.SetInt("bullseyeScored", PlayerPrefs.GetInt("bullseyeScored") + 1);
		}
		else
			AudioManager.Instance.PlaySFX(wallHitSfx[Random.Range(0, wallHitSfx.Length)]);

		if (isPlayerTurn)
			dartsPerTurnPlayer1[dartsPerTurnPlayer1.Length - dartsLeft].sprite = emptySprite;
		else
			dartsPerTurnPlayer2[dartsPerTurnPlayer2.Length - dartsLeft].sprite = emptySprite;
		dartsLeft--;

		if (dartsLeft == 0)
		{
			if (MusicStoreTurn == 0 && !isPlayerTurn)
				EndGame();

			ChangeTurn();

		}

		UpdateUI();
	}

	protected override void EndGame()
	{





		if (playerScore > cpuScore)


		{
			endGamePanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			endGamePanelTitle.sprite = winTextSprite;
			endGamePanelMessage.text = $"You won {level.prize}";
			PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + level.prize);
			PlayerPrefs.SetInt("matchesWon", PlayerPrefs.GetInt("matchesWon") + 1);
			PlayerPrefs.SetInt("totalMatchesWon", PlayerPrefs.GetInt("totalMatchesWon") + 1);
			PlayerPrefs.SetInt(level.victory.ToString(), PlayerPrefs.GetInt(level.victory.ToString()) + 1);
			AudioManager.Instance.PlaySFX("Free Currency Gain");




		}
		else if (cpuScore > playerScore)
		{
			endGamePanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			endGamePanelTitle.sprite = loseTextSprite;
			endGamePanelMessage.text = $"You lost {level.entryFee}";


		}
		else
		{
			endGamePanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			endGamePanelTitle.sprite = winTextSprite;
			endGamePanelMessage.text = $"You tied {level.prize / 2}";
			PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + level.prize / 2);
			PlayerPrefs.SetInt("matchesWon", PlayerPrefs.GetInt("matchesWon") + 1);
			PlayerPrefs.SetInt("totalMatchesWon", PlayerPrefs.GetInt("totalMatchesWon") + 1);
			PlayerPrefs.SetInt(level.victory.ToString(), PlayerPrefs.GetInt(level.victory.ToString()) + 1);
			AudioManager.Instance.PlaySFX("Free Currency Gain");


		}

		PlayerPrefs.SetInt(level.location.ToString(), PlayerPrefs.GetInt(level.location.ToString()) + 1);
		PlayerPrefs.SetInt(level.dailyLocation.ToString(), PlayerPrefs.GetInt(level.dailyLocation.ToString()) + 1);
		pauseTimer = true;
		gameObject.SetActive(false);
		DartGenerator.instance.DisableDart();
		CallOnGameEndEvent();
		PlayerPrefs.SetInt("matchesPlayed", PlayerPrefs.GetInt("matchesPlayed") + 1);
		PlayerPrefs.SetInt("totalMatchesPlayed", PlayerPrefs.GetInt("totalMatchesPlayed") + 1);
		AudioManager.Instance.StopSFXSource();

	}

	private void ChangeTurn()
	{
		DartGenerator.instance.DeleteDarts(maxDartsPerTurn);
		AudioManager.Instance.PlaySFX("Darts Take Back");
		dartsLeft = maxDartsPerTurn;
		RefreshPlayerDarts();
		isPlayerTurn = !isPlayerTurn;
		ChangePlayerTurnIndicator();
		if (isPlayerTurn)
			MusicStoreTurn--;
	}

}