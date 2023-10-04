using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	[SerializeField]
	private Text matchesPlayed;
	[SerializeField]
	private Text winRate;
	[SerializeField]
	private Text favouriteLocation;
	[SerializeField]
	private Level[] levels;

	private int max;
	private string favouriteLocationName;

	private void Awake()
	{
		max = 0;
		favouriteLocationName = levels[0].name;

		for (int i = 0; i < levels.Length; i++)
		{
			if (PlayerPrefs.GetInt(levels[i].location.ToString()) > max)
			{
				favouriteLocationName = levels[i].name;
				max = PlayerPrefs.GetInt(levels[i].location.ToString());
			}
		}

		matchesPlayed.text = PlayerPrefs.GetInt("totalMatchesPlayed").ToString();

		winRate.text = ((int)((float)(PlayerPrefs.GetInt("totalMatchesWon")/ (float)PlayerPrefs.GetInt("totalMatchesPlayed"))* 100)).ToString() + "%";

		favouriteLocation.text = favouriteLocationName;

	}

	public void OpenTOS() 
	{
		Application.OpenURL("https://drive.google.com/file/d/1U3rncH45L4pqAcPFNnnIA3FA9S8nymvp/view?usp=sharing");
	}
}
