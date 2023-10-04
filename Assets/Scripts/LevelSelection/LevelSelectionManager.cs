using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Displays level info and manages level loading
 */

public class LevelSelectionManager : MonoBehaviour
{
	[SerializeField]
	private int weeklyFairMaxAttemps;
	[SerializeField]
	private GameObject LevelPopUp;
	[SerializeField]
	private GameObject FairPopUp;
	[SerializeField]
	private GameObject userMessage;
	[SerializeField]
	private GameObject levelUnlockedPanel;
	[SerializeField]
	private GameObject darkBackgroundPanel;
	[SerializeField]
	private GameObject levelLockedPanel;
	[SerializeField]
	private GameObject matchmakingLoadingPanel;
	[SerializeField]
	private GameObject matchmakingCancelButton;
	[SerializeField]
	private GameObject entryFeeFreeCurrencyIcon;
	[SerializeField]
	private Text levelLockedPanelName;
	[SerializeField]
	private Text levelLockedPanelNumberOfMatches;
	[SerializeField]
	private Text levelName;
	[SerializeField]
	private Text prize;
	[SerializeField]
	private Text entryFee;
	[SerializeField]
	private Text gameMode;
	[SerializeField]
	private Text gameModeDescription;
	[SerializeField]
	private Font daCherry;
	[SerializeField]
	private Font varela;
	[SerializeField]
	private Image gameModeInfoImage;
	[SerializeField]
	private Button playButton;
	[SerializeField]


	private Level[] levels = new Level[5];

	private void Awake()
	{

		if (!PlayerPrefs.HasKey("currentUnlockedLevelNumber"))
			PlayerPrefs.SetInt("currentUnlockedLevelNumber", 1);

		if (PlayerPrefs.GetInt("currentUnlockedLevelNumber") < levels.Length && levels[PlayerPrefs.GetInt("currentUnlockedLevelNumber")].CheckUnlocked())
		{
			levelUnlockedPanel.GetComponentInChildren<Text>().text = levels[PlayerPrefs.GetInt("currentUnlockedLevelNumber")].levelName;
			levelUnlockedPanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			PlayerPrefs.SetInt("currentUnlockedLevelNumber", PlayerPrefs.GetInt("currentUnlockedLevelNumber") + 1);
		}
	}

	private void Start()
	{
		AudioManager.Instance.PlayMusic("Main Menu");
	}

	
	public void ShowLevelInfo(Level level)
	{
		if (level.CheckUnlocked())
		{
			LevelPopUp.SetActive(true);
			levelName.text = level.levelName;
			prize.text = level.prize.ToString();
			if (PlayerPrefs.GetInt(level.location.ToString()) == 0)
			{
				entryFee.text = "Free try";
				entryFee.font = daCherry;
				entryFee.color = new Color32(92, 42, 31, 255);
				entryFee.fontSize = 50;
				entryFeeFreeCurrencyIcon.SetActive(false);
			}
			else
			{
				entryFee.text = level.entryFee.ToString();
				entryFee.font = varela;
				entryFee.color = new Color32(29, 39, 40, 255);
				entryFee.fontSize = 40;
				entryFeeFreeCurrencyIcon.SetActive(true);
			}
			gameMode.text = level.gameMode.ToString();
			gameModeDescription.text = string.Empty;
			foreach (string sentence in level.gameModeDescription.Split("."))
			{
				gameModeDescription.text += sentence + "\n\n";
			}
			gameModeInfoImage.sprite = level.gameModeInfoTargetSprite;
			playButton.onClick.AddListener(delegate () { LoadLevel(level); });
		}
		else
		{
			levelLockedPanelName.text = level.levelName;
			levelLockedPanelNumberOfMatches.text = $"Play {level.couterToUnlock - PlayerPrefs.GetInt(level.previuosLevel.location.ToString())} times\n{level.previuosLevel.levelName}\nto unlock";
			levelLockedPanel.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}
	}

	public void LoadScene(int scene)
	{
		PlayerPrefs.SetInt("previousScene", SceneManager.GetActiveScene().buildIndex);
		switch (scene)
		{
			case 2: //inventory
			case 4: //shop

				if (PlayerPrefs.GetInt("totalMatchesPlayed") < 3)
				{
					userMessage.SetActive(true);
					darkBackgroundPanel.SetActive(true);
					userMessage.GetComponentInChildren<Text>().text = "You need more experience to enter";
					return;
				}
				break;
			case 9: //fair
				AudioManager.Instance.PlayMusic("Event");
				break;
		}
		SceneManager.LoadScene(scene);
	}

	public void LoadLevel(Level level)
	{
		if (PlayerPrefs.GetInt("freeCurrency") >= level.entryFee || PlayerPrefs.GetInt(level.location.ToString()) == 0)
			StartCoroutine(StartMatchmaking(level));
		else
		{
			userMessage.SetActive(true);
			userMessage.GetComponentInChildren<Text>().text = "You don't have enough resources to play this level";
		}


	}

	public void ResetListener()
	{
		playButton.onClick.RemoveAllListeners();
	}

	private IEnumerator StartMatchmaking(Level level)
	{
		LevelPopUp.SetActive(false);
		matchmakingLoadingPanel.SetActive(true);
		yield return new WaitForSeconds(matchmakingLoadingPanel.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
		matchmakingCancelButton.SetActive(false);
		yield return new WaitForSeconds(1);
		AudioManager.Instance.PlayMusic("In Game");
		LoadScene(level.sceneId);
	}

	public void StopMatchMaking() 
	{
		StopAllCoroutines();
	}

	public void PlayLocationButtonSound()
	{
		AudioManager.Instance.PlaySFX("Location Tap");
	}

	public void LoadTownFairMode()
	{
		if (PlayerPrefs.GetInt("totalMatchesPlayed") < 5)
		{
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			userMessage.GetComponentInChildren<Text>().text = "You need more experience to enter";
			return;
		}
		else if (PlayerPrefs.GetInt("weeklyFairTimesWon") == 1 || PlayerPrefs.GetInt("weeklyFairTimesAttempted") == weeklyFairMaxAttemps)
		{
			userMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
			userMessage.GetComponentInChildren<Text>().text = "No attemps Left";
			return;
		}
		else
		{
			darkBackgroundPanel.SetActive(true);
			FairPopUp.SetActive(true);
			return;
		}
	}
}