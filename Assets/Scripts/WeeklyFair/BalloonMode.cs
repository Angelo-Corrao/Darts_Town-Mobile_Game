using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BalloonMode : MonoBehaviour
{
	[System.Serializable]
	public struct ColoredBalloonSprite
	{
		public Balloon.BalloonColor color;
		public Sprite balloonSprite;
	}

	public class ColoredBalloon
	{
		public Balloon.BalloonColor color;

		public ColoredBalloon(int color)
		{
			this.color = (Balloon.BalloonColor)color;
		}
	}

	[SerializeField]
	private int dartsLeft;
	[SerializeField]
	private int weeklyFairMaxAttemps;

	[SerializeField]
	private Image currentColorObjective;
	[SerializeField]
	private Image[] dartsLeftSprite;
	[SerializeField]
	private Sprite empty;
	[SerializeField]
	protected Sprite winTextSprite;
	[SerializeField]
	protected Sprite loseTextSprite;
	[SerializeField]
	private GameObject endGamePanel;
	[SerializeField]
	private GameObject darkBackgroundPanel;
	[SerializeField]
	private GameObject userMessage;
	[SerializeField]
	private GameObject confettiVFX;
	[SerializeField]
	private Button claimButton;
	[SerializeField]
	private List<ColoredBalloonSprite> balloonSprites = new List<ColoredBalloonSprite>();
	[SerializeField]
	private LootBox[] weeklyFairRewards = new LootBox[2];

	private List<ColoredBalloon> balloonsObjectives = new List<ColoredBalloon>();
	private int playerCounter;
	private List<int> pickedColors = new List<int>();
	private int random;

	private void Awake()
	{
		playerCounter = 0;
		for (int i = 0; i < 3; i++)
		{
			do
			{
				random = Random.Range(0, balloonSprites.Count);
			} while (pickedColors.Contains(random));
			pickedColors.Add(random);
			balloonsObjectives.Add(new ColoredBalloon(random));
		}
		UpdateUI();
	}

	private void OnEnable()
	{
		Dart.onTargetHit += OnTargetHitManager;
	}

	private void OnDisable()
	{
		Dart.onTargetHit -= OnTargetHitManager;
	}

	protected void OnTargetHitManager(Collision collision)
	{
		dartsLeft--;
		dartsLeftSprite[dartsLeft].sprite = empty;

		if (collision.gameObject.GetComponent<Balloon>() != null)
		{
			if (collision.gameObject.GetComponent<Balloon>().color == balloonsObjectives[playerCounter].color)
			{
				playerCounter++;
				if (playerCounter == 3)
				{
					PlayerPrefs.SetInt("weeklyFairTimesWon", 1);
					PlayerPrefs.SetInt("weeklyFairTimesAttempted", PlayerPrefs.GetInt("weeklyFairTimesAttempted") + 1);
					endGamePanel.SetActive(true);
					darkBackgroundPanel.SetActive(true);
					endGamePanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = winTextSprite;
					DartGenerator.instance.DisableDart();
					GameManager.instance.HideGesture();

				}
				else if (playerCounter < 3)
					UpdateUI();
			}
			else
			{
				Debug.Log("wrong balloon");
				endGamePanel.SetActive(true);
				darkBackgroundPanel.SetActive(true);
				endGamePanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = loseTextSprite;
				PlayerPrefs.SetInt("weeklyFairTimesAttempted", PlayerPrefs.GetInt("weeklyFairTimesAttempted") + 1);
				DartGenerator.instance.DisableDart();
				GameManager.instance.HideGesture();
			}

			Destroy(collision.gameObject);
			confettiVFX.transform.position = collision.transform.position;
			confettiVFX.GetComponent<ParticleSystem>().Play();
			AudioManager.Instance.PlaySFX("Balloon Pop");
		}
		else
		{
			Debug.Log("balloon null");
			endGamePanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			endGamePanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = loseTextSprite;
			PlayerPrefs.SetInt("weeklyFairTimesAttempted", PlayerPrefs.GetInt("weeklyFairTimesAttempted") + 1);
			DartGenerator.instance.DisableDart();
			GameManager.instance.HideGesture();
		}

	}

	protected void UpdateUI()
	{
		currentColorObjective.sprite = balloonSprites[(int)balloonsObjectives[playerCounter].color].balloonSprite;
	}

	public void ReloadLevel()
	{
		if ((PlayerPrefs.GetInt("weeklyFairTimesWon") == 1 || PlayerPrefs.GetInt("weeklyFairTimesAttempted") == weeklyFairMaxAttemps) && PlayerPrefs.GetInt("weeklyFairPrizeClaimed") == 0)
		{
			userMessage.GetComponentInChildren<Text>().text = "Claim your reward first";
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}
		else if (PlayerPrefs.GetInt("weeklyFairTimesAttempted") < 3 && PlayerPrefs.GetInt("weeklyFairTimesWon") == 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		else
		{
			userMessage.GetComponentInChildren<Text>().text = "No attempts left";
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}
	}

	public void ClaimPrize()
	{
		if (PlayerPrefs.GetInt("weeklyFairTimesWon") == 1 && PlayerPrefs.GetInt("weeklyFairPrizeClaimed") == 0)
		{
			LootBoxManager.instance.PullLootBox(weeklyFairRewards[1]);
			PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + (int)weeklyFairRewards[1].price);
			PlayerPrefs.SetInt("weeklyFairPrizeClaimed", 1);
		}
		else if (PlayerPrefs.GetInt("weeklyFairTimesAttempted") == weeklyFairMaxAttemps && PlayerPrefs.GetInt("weeklyFairTimesWon") != 1 && PlayerPrefs.GetInt("weeklyFairPrizeClaimed") == 0)
		{
			LootBoxManager.instance.PullLootBox(weeklyFairRewards[0]);
			PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + (int)weeklyFairRewards[0].price);
			PlayerPrefs.SetInt("weeklyFairPrizeClaimed", 1);
		}
		else
		{
			userMessage.GetComponentInChildren<Text>().text = "No rewards to claim";
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}

		claimButton.interactable = false;

	}

	public void PlayButtonSound()
	{
		AudioManager.Instance.PlaySFX("Button Tap");
	}

	public void LoadScene(int scene)
	{
		if ((PlayerPrefs.GetInt("weeklyFairTimesWon") == 1 || PlayerPrefs.GetInt("weeklyFairTimesAttempted") == weeklyFairMaxAttemps) && PlayerPrefs.GetInt("weeklyFairPrizeClaimed") == 0)
		{
			userMessage.GetComponentInChildren<Text>().text = "Claim your reward first";
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}
		else
			SceneManager.LoadScene(scene);
	}

	public void LeaveGame()
	{
		PlayerPrefs.SetInt("weeklyFairTimesAttempted", PlayerPrefs.GetInt("weeklyFairTimesAttempted") + 1);
		SceneManager.LoadScene(1);
	}

	public void PauseGame() 
	{
		DartGenerator.instance.DisableDart();
	}

	public void ResumeGame() 
	{
		DartGenerator.instance.EnableDart();
	}
}