using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class DailyChallenge : ScriptableObject
{
	public enum Category
	{
		matchesWon,
		matchesPlayed,
		bullseyeScored,
		doubleScored,
		triplesScored,
		maxPointsInARound,
		grandmaHouseDaily,
		officeDaily,
		pubDaily,
		musicStoreDaily,
		arenaDaily,
		weeklyFairTimesAttempted
	}

	public string challenge;
	public int freeCurrencyReward;
	public int premiumCurrencyReward;
	public int x;
	public Category category;

	public bool CheckCompletion() 
	{
		if (PlayerPrefs.GetInt(category.ToString()) >= x)
			return true;
		return false;
	}
}
