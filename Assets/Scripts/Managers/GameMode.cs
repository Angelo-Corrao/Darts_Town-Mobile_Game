using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GameMode : MonoBehaviour
{
	public delegate void OnUIUpdate(int playerScore, int cpuScore, int playerTurnScore, int cpuTurnScore, int dartsLeft, bool isPlayerTurn);
	public static event OnUIUpdate onUIUpdate;

	public delegate void OnTimerExpire();
	public static event OnTimerExpire onTimerExpire;

	public delegate void OnGameEnd();
	public static event OnGameEnd onGameEnd;

	[SerializeField]
	protected int maxDartsPerTurn;
	[SerializeField]
	protected float turnTimer;
	[SerializeField]
	protected string[] dartboardHitSFX;
	[SerializeField]
	protected GameObject scoreTextPopUp;
	[SerializeField]
	protected GameObject darkBackgroundPanel;


	[SerializeField]
	protected GameObject userMessage;
	[SerializeField]
	protected GameObject player1Name;
	[SerializeField]
	protected GameObject player2Name;
	[SerializeField]
	protected GameObject playerTurnIndicator;
	[SerializeField]
	public GameObject endGamePanel;
	[SerializeField]
	protected Level level;
	[SerializeField]
	protected Sprite winTextSprite;
	[SerializeField]
	protected Sprite loseTextSprite;
	[SerializeField]
	protected Sprite dartsPerTurnSprite;
	[SerializeField]
	protected Sprite emptySprite;
	[SerializeField]
	protected Image timerImage;
	[SerializeField]
	protected Image endGamePanelTitle;


	[SerializeField]
	protected Image[] dartsPerTurnPlayer1;
	[SerializeField]
	protected Image[] dartsPerTurnPlayer2;
	[SerializeField]
	protected Text exitPanelText;
	[SerializeField]
	protected Text endGamePanelMessage;

	public int playerScore;
	public int cpuScore;

	protected bool pauseGame = false;
	protected bool pauseTimer = false;

	protected float timer;

	protected bool isPlayerTurn = true;
	public int startingPlayerPoints;
	public int startingCpuPoints;
	protected int playerTurnScore;
	protected int cpuTurnScore;
	protected int dartsLeft;
	protected bool isTurnTimerSFXPlaying = false;
	protected string[] wallHitSfx = new string[] { "Wall Hit 1", "Wall Hit 2", "Wall Hit 3" };
	protected Text player1Text;
	protected Text player2Text;

	protected virtual void Awake()
	{
		dartsLeft = maxDartsPerTurn;
		timer = turnTimer;
		UpdateUI();
		player1Text = player1Name.GetComponent<Text>();
		player2Text = player2Name.GetComponent<Text>();

	}

	protected void OnEnable()
	{
		Dart.onTargetHit += OnTargetHitManager;


		GameManager.onGamePause += StopTurnTimer;
		GameManager.onGameResume += StartTurnTimer;
		Tutorial.onTutorialEnd += ChangeBanner;
	}

	protected void OnDisable()
	{
		Dart.onTargetHit -= OnTargetHitManager;


		GameManager.onGamePause -= StopTurnTimer;
		GameManager.onGameResume -= StartTurnTimer;
		Tutorial.onTutorialEnd -= ChangeBanner;
	}

	protected void Start()
	{
		if (!GameManager.instance.tutorial)
			StartCoroutine(TurnChangeBanner());
		if (PlayerPrefs.GetInt(level.location.ToString()) > 0)
		{
			PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") - level.entryFee);
			PlayerPrefs.SetInt("freeCurrencySpent", PlayerPrefs.GetInt("freeCurrencySpent") + level.entryFee);
		}
	}

	protected abstract void OnTargetHitManager(Collision collision);

	protected IEnumerator DisableScoreText(GameObject score)
	{
		yield return new WaitForSeconds(1);
		Destroy(score);
		yield return null;
	}

	protected void StartTurnTimer()
	{
		pauseGame = false;

	}

	protected void StopTurnTimer()
	{
		pauseGame = true;

	}



	protected void TimerExpire()
	{
		onTimerExpire?.Invoke();
	}

	public virtual void UpdateUI()
	{
		onUIUpdate?.Invoke(playerScore, cpuScore, playerTurnScore, cpuTurnScore, dartsLeft, isPlayerTurn);
	}


	public void ChangeMechanic()
	{
		DartGenerator.instance.dartsGenerated.Last<GameObject>().GetComponent<DragGesture>().enabled = false;
		DartGenerator.instance.dartsGenerated.Last<GameObject>().GetComponent<SwipeGesture>().enabled = true;
	}

	protected virtual void EndGame()
	{
		endGamePanel.SetActive(true);
		darkBackgroundPanel.SetActive(true);
		endGamePanelTitle.sprite = isPlayerTurn ? winTextSprite : loseTextSprite;
		endGamePanelMessage.text = isPlayerTurn ? $"You won {level.prize}" : $"You lost {level.entryFee}";
		if (isPlayerTurn)
		{
			PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + level.prize);
			PlayerPrefs.SetInt("matchesWon", PlayerPrefs.GetInt("matchesWon") + 1);
			PlayerPrefs.SetInt("totalMatchesWon", PlayerPrefs.GetInt("totalMatchesWon") + 1);
			PlayerPrefs.SetInt(level.victory.ToString(), PlayerPrefs.GetInt(level.victory.ToString()) + 1);
			AudioManager.Instance.PlaySFX("Free Currency Gain");
		}

		PlayerPrefs.SetInt(level.location.ToString(), PlayerPrefs.GetInt(level.location.ToString()) + 1);
		PlayerPrefs.SetInt(level.dailyLocation.ToString(), PlayerPrefs.GetInt(level.dailyLocation.ToString()) + 1);
		gameObject.SetActive(false);

		DartGenerator.instance.DisableDart();
		pauseTimer = true;
		CallOnGameEndEvent();
		AudioManager.Instance.StopSFXSource();
		PlayerPrefs.SetInt("matchesPlayed", PlayerPrefs.GetInt("matchesPlayed") + 1);
		PlayerPrefs.SetInt("totalMatchesPlayed", PlayerPrefs.GetInt("totalMatchesPlayed") + 1);
		if (PlayerPrefs.GetInt("grandmaHouse") == 1 || PlayerPrefs.GetInt("totalMatchesPlayed") == 3)
			SceneManager.LoadScene(1);
	}

	protected void CallOnGameEndEvent()
	{
		onGameEnd?.Invoke();
	}

	public void RealoadLevel()
	{
		if (PlayerPrefs.GetInt("freeCurrency") < level.entryFee)
			userMessage.SetActive(true);
		else
		{
			PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") - level.entryFee);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		}
	}

	public void LeaveGameWarning()
	{
		if (PlayerPrefs.GetInt(level.location.ToString()) == 0)
			exitPanelText.text = $"\n\nAre you sure you want to quit?";
		else
			exitPanelText.text = $"Are you sure you want to quit?\n\nYou will lose {level.entryFee} free currency";

	}

	public void LeaveGame(int scene)
	{
		PlayerPrefs.SetInt("totalMatchesPlayed", PlayerPrefs.GetInt("totalMatchesPlayed") + 1);
		PlayerPrefs.SetInt(level.location.ToString(), PlayerPrefs.GetInt(level.location.ToString()) + 1);
		PlayerPrefs.SetInt(level.dailyLocation.ToString(), PlayerPrefs.GetInt(level.dailyLocation.ToString()) + 1);
		GameManager.instance.LoadScene(scene);
	}

	public void ChangePlayerTurnIndicator()
	{
		timer = turnTimer;
		timerImage.fillAmount = Mathf.Clamp(timer / turnTimer, 0, 1);
		player1Name.GetComponent<Animator>().enabled = isPlayerTurn;
		player2Name.GetComponent<Animator>().enabled = !isPlayerTurn;
		if (!isPlayerTurn)
			player1Text.color = new Color(player1Text.color.r, player1Text.color.g, player1Text.color.b, 1);
		else
			player2Text.color = new Color(player2Text.color.r, player2Text.color.g, player2Text.color.b, 1);
		StartCoroutine(TurnChangeBanner());
	}


	public void ChangeBanner()
	{
		StartCoroutine(TurnChangeBanner());
	}

	public IEnumerator TurnChangeBanner()
	{
		pauseTimer = true;
		playerTurnIndicator.SetActive(true);
		DartGenerator.instance.DisableDart();
		yield return new WaitForSeconds(0.8f);
		pauseTimer = false;
		playerTurnIndicator.SetActive(false);
		DartGenerator.instance.EnableDart();

	}

	protected void RefreshPlayerDarts()
	{
		if (isPlayerTurn)
		{
			for (int i = 0; i < dartsPerTurnPlayer2.Length; i++)
			{
				dartsPerTurnPlayer2[i].sprite = dartsPerTurnSprite;
			}
		}
		else
		{
			for (int i = 0; i < dartsPerTurnPlayer1.Length; i++)
			{
				dartsPerTurnPlayer1[i].sprite = dartsPerTurnSprite;
			}
		}
	}
}
