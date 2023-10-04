using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages the game with Around the Clock rules
 */

public class OfficeMode : GameMode
{
	[SerializeField]
	private GameObject player1Bullseye;
	[SerializeField]
	private GameObject player2Bullseye;
	[SerializeField]
	private GameObject player1Score;
	[SerializeField]
	private GameObject player2Score;

	public bool isCenterEnabled = false;

	protected override void Awake()
	{
		base.Awake();
		dartboardHitSFX = new string[] { "Darts Paper 1", "Darts Paper 2", "Darts Paper 3" };
	}

	private void Update()
	{
		if (!GameManager.instance.tutorial)
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
					ChangeTurn();

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
			GameModeManagement(ref playerScore, ref player1Score, ref player1Bullseye, collision);
		else
			GameModeManagement(ref cpuScore, ref player2Score, ref player2Bullseye, collision);

	}

	private void GameModeManagement(ref int score, ref GameObject playerScoreText, ref GameObject playerBullseye, Collision collision)
	{
		if (collision.gameObject.GetComponent<TargetArea>() != null)
		{
			AudioManager.Instance.StopSFXSource();
			AudioManager.Instance.PlaySFX(dartboardHitSFX[Random.Range(0, dartboardHitSFX.Length)]);
			GameObject scoreUI = Instantiate(scoreTextPopUp, collision.GetContact(0).point - new Vector3(0, -0.2f, 1), Quaternion.identity);
			scoreUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = collision.gameObject.GetComponent<TargetArea>().score.ToString();
			StartCoroutine(DisableScoreText(scoreUI));

			if (score == collision.gameObject.GetComponent<TargetArea>().score)
			{
				score++;
			}

			if (score == 13)
			{
				playerScoreText.SetActive(false);
				playerBullseye.SetActive(true);
			}

			if (score == (isCenterEnabled ? 14 : 13))
				EndGame();

		}
		else
			AudioManager.Instance.PlaySFX(wallHitSfx[Random.Range(0, wallHitSfx.Length)]);


		if (isPlayerTurn)
			dartsPerTurnPlayer1[dartsPerTurnPlayer1.Length - dartsLeft].sprite = emptySprite;
		else
			dartsPerTurnPlayer2[dartsPerTurnPlayer2.Length - dartsLeft].sprite = emptySprite;
		dartsLeft--;

		if (dartsLeft == 0)
			ChangeTurn();

		UpdateUI();
	}

	IEnumerator DeleteDarts()
	{
		yield return new WaitForSeconds(1);
		AudioManager.Instance.PlaySFX("Darts Take Back");
		DartGenerator.instance.DeleteDarts(maxDartsPerTurn);
	}

	private void ChangeTurn()
	{
		dartsLeft = maxDartsPerTurn;
		RefreshPlayerDarts();
		isPlayerTurn = !isPlayerTurn;
		StartCoroutine(DeleteDarts());
		ChangePlayerTurnIndicator();
	}
}
