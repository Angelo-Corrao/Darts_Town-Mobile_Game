using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
 * Everyday three new challenges are generated, one from each category.
 * Rerolling a challenge swaps a challenge with a different one from the same category
 */

public class DailyChallengeManager : MonoBehaviour
{
	[Serializable]
	public struct DailyChallengeUIBanner
	{
		public GameObject rerollButton;
		public Button claimButton;
		public Text challengeDescription;
		public Text rewardValue;
		public Image rewardIcon;
		public Image background;
		public Player.CurrencyType rewardType;
	}

	[Serializable]
	public class Challenge
	{
		public int position;
		public int rerollCounter;
		public bool claimed;

		public Challenge(int position, int rerollCounter, bool claimed)
		{
			this.position = position;
			this.rerollCounter = rerollCounter;
			this.claimed = claimed;
		}
	}

	[Serializable]
	public class Challenges
	{
		public List<Challenge> challenges = new List<Challenge>();
	}

	[Serializable]
	public struct ChallengeList
	{
		public List<DailyChallenge> list;
	}


	[SerializeField]
	private List<DailyChallengeUIBanner> challengeBanner;
	[SerializeField]
	private List<ChallengeList> challengeTypes; //list of challenges divided by category

	[SerializeField]
	private Sprite greyFreeCurrency;
	[SerializeField]
	private Sprite greyPremiumCurrency;
	[SerializeField]
	private Sprite coloredFreeCurrency;
	[SerializeField]
	private Sprite coloredPremiumCurrency;
	[SerializeField]
	private Sprite incompletePanelBackground;
	[SerializeField]
	private Sprite completePanelBackground;

	private Challenges savedChallenges = new Challenges();
	private DailyChallenge[] dailyChallenges = new DailyChallenge[3];

	private string savingPath;
	private int random;

	void OnEnable()
	{
		savingPath = Application.persistentDataPath + "/DailyChallenges.txt";

		if (!PlayerPrefs.HasKey("lastLoginDate"))
		{
			ResetDailyData();

			for (int i = 0; i < dailyChallenges.Length; i++)
			{
				GenerateNewChallenge(i);
			}

			WriteOnFile();

		}
		else if (!string.Equals(PlayerPrefs.GetString("lastLoginDate"), DateTime.Today.ToString()))
		{
			ResetDailyData();

			for (int i = 0; i < 3; i++)
			{
				PlayerPrefs.SetInt("bundle" + i + "interactable", 1);
			}

			for (int i = 0; i < dailyChallenges.Length; i++)
			{
				GenerateNewChallenge(i);
			}

			WriteOnFile();
		}



	}

	private void Start()
	{
		ReadFromFile();


		for (int i = 0; i < dailyChallenges.Length; i++)
		{
			LoadSavedChallenges(i);
		}
	}

	public void RerollDailyChallenge(int position)
	{
		PlayerPrefs.SetInt(dailyChallenges[position].category.ToString(), 0);
		do
		{
			random = UnityEngine.Random.Range(0, challengeTypes[position].list.Count);
		} while (random == savedChallenges.challenges[position].position);
		dailyChallenges[position] = challengeTypes[position].list[random];
		challengeBanner[position].challengeDescription.text = UpdateChallengeDescription(dailyChallenges[position].challenge, dailyChallenges[position].x - PlayerPrefs.GetInt(dailyChallenges[position].category.ToString()));
		savedChallenges.challenges[position].position = random;
		savedChallenges.challenges[position].rerollCounter = 0;
		savedChallenges.challenges[position].claimed = false;
		challengeBanner[position].rerollButton.SetActive(false);
		challengeBanner[position].rewardIcon.sprite = greyFreeCurrency;
		challengeBanner[position].background.sprite = incompletePanelBackground;

		WriteOnFile();



	}

	public void ClaimDailyChallengeReward(int position)
	{
		switch (challengeBanner[position].rewardType) 
		{
			case Player.CurrencyType.freeCurrency:
				PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + dailyChallenges[position].freeCurrencyReward);
				break;
			case Player.CurrencyType.premiumCurrency:
				PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + dailyChallenges[position].premiumCurrencyReward);
				break;
		}

		savedChallenges.challenges[position].claimed = true;
		challengeBanner[position].claimButton.interactable = !savedChallenges.challenges[position].claimed;
		challengeBanner[position].rewardIcon.sprite = challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency ? greyFreeCurrency : greyPremiumCurrency;
		challengeBanner[position].background.transform.localScale = Vector3.one;

		if (challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency)
		{
			challengeBanner[position].rerollButton.SetActive(false);
			AudioManager.Instance.PlaySFX("Free Currency Gain");
		}
		else
			AudioManager.Instance.PlaySFX("Premium Currency Gain");

		WriteOnFile();

		CurrencyManager.instance.UpdateCurrency();
	}

	public void GenerateNewChallenge(int position)
	{
		random = UnityEngine.Random.Range(0, challengeTypes[position].list.Count);
		dailyChallenges[position] = challengeTypes[position].list[random];
		challengeBanner[position].challengeDescription.text = UpdateChallengeDescription(dailyChallenges[position].challenge, dailyChallenges[position].x - PlayerPrefs.GetInt(dailyChallenges[position].category.ToString()));
		savedChallenges.challenges.Add(new Challenge(random, challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency ? 1 : 0, false));


	}

	public void LoadSavedChallenges(int position)
	{
		dailyChallenges[position] = challengeTypes[position].list[savedChallenges.challenges[position].position];
		challengeBanner[position].challengeDescription.text = dailyChallenges[position].CheckCompletion() ? UpdateChallengeDescription(challengeTypes[position].list[savedChallenges.challenges[position].position].challenge, challengeTypes[position].list[savedChallenges.challenges[position].position].x) : UpdateChallengeDescription(challengeTypes[position].list[savedChallenges.challenges[position].position].challenge, challengeTypes[position].list[savedChallenges.challenges[position].position].x - PlayerPrefs.GetInt(challengeTypes[position].list[savedChallenges.challenges[position].position].category.ToString()));
		if (challengeBanner[position].rerollButton != null)
			challengeBanner[position].rerollButton.SetActive(savedChallenges.challenges[position].rerollCounter == 1 && !dailyChallenges[position].CheckCompletion());
		challengeBanner[position].claimButton.interactable = dailyChallenges[position].CheckCompletion() && !savedChallenges.challenges[position].claimed;
		challengeBanner[position].rewardValue.text = challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency ? dailyChallenges[position].freeCurrencyReward.ToString() : dailyChallenges[position].premiumCurrencyReward.ToString(); ;
		if (dailyChallenges[position].CheckCompletion() && !savedChallenges.challenges[position].claimed)
			challengeBanner[position].rewardIcon.sprite = challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency ? coloredFreeCurrency : coloredPremiumCurrency;
		else
			challengeBanner[position].rewardIcon.sprite = challengeBanner[position].rewardType == Player.CurrencyType.freeCurrency ? greyFreeCurrency : greyPremiumCurrency;
		challengeBanner[position].background.sprite = dailyChallenges[position].CheckCompletion() ? completePanelBackground : incompletePanelBackground;
		challengeBanner[position].background.transform.localScale = dailyChallenges[position].CheckCompletion() && !savedChallenges.challenges[position].claimed ? new Vector3(1.2f, 1.2f, 1) : Vector3.one;

	}

	private void ResetDailyData()
	{
		PlayerPrefs.SetString("lastLoginDate", DateTime.Today.ToString());
		PlayerPrefs.SetInt("matchesWon", 0);
		PlayerPrefs.SetInt("matchesPlayed", 0);
		PlayerPrefs.SetInt("bullseyeScored", 0);
		PlayerPrefs.SetInt("doubleScored", 0);
		PlayerPrefs.SetInt("triplesScored", 0);
		PlayerPrefs.SetInt("maxPointsInARound", 0);
		PlayerPrefs.SetInt("grandmaHouseDaily", 0);
		PlayerPrefs.SetInt("officeDaily", 0);
		PlayerPrefs.SetInt("pubDaily", 0);
		PlayerPrefs.SetInt("musicStoreDaily", 0);
		PlayerPrefs.SetInt("arenaDaily", 0);
		PlayerPrefs.SetInt("weeklyFairTimesAttempted", 0);
		PlayerPrefs.SetInt("weeklyFairTimesWon", 0);
		PlayerPrefs.SetInt("weeklyFairPrizeClaimed", 0);
		PlayerPrefs.SetInt("generateDailyBundle", 0);
	}

	private void ReadFromFile()
	{
		string json = File.ReadAllText(savingPath);
		savedChallenges = JsonUtility.FromJson<Challenges>(json);
	}

	private void WriteOnFile()
	{
		string output = JsonUtility.ToJson(savedChallenges);
		File.WriteAllText(savingPath, output);
	}

	private string UpdateChallengeDescription(string stringToReplace, int replacement)
	{
		string[] words = stringToReplace.Split(' ');
		string newString = string.Empty;
		for (int i = 0; i < words.Length; i++)
		{
			if (words[i].Equals("x"))
				words[i] = replacement.ToString();

			newString += words[i] + " ";
		}

		return newString;

	}

}