using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Everyday three different bundles are picked from the list of bundles
 */

public class DailyBundleManager : MonoBehaviour
{
	public static DailyBundleManager instance;

	[Serializable]
	public struct DailyBundleUI 
	{
		public Text price;
		public Image icon;
	}
 
	public List<DailyBundle> bundles = new List<DailyBundle>();

	[SerializeField]
	private List<DailyBundleUI> bundleUI;

	private int random;

	private void Start()
	{
		if(instance == null)
			instance= this;

		if (PlayerPrefs.GetInt("generateDailyBundle") == 0) 
		{
			random = UnityEngine.Random.Range(0, bundles.Count - 1);
			PlayerPrefs.SetInt("Bundle0Position", random);
			do
			{
				random = UnityEngine.Random.Range(0, bundles.Count - 1);
			} while (random == PlayerPrefs.GetInt("Bundle0Position"));
			PlayerPrefs.SetInt("Bundle1Position", random);

			do
			{
				random = UnityEngine.Random.Range(0, bundles.Count - 1);
			} while (random == PlayerPrefs.GetInt("Bundle0Position") || random == PlayerPrefs.GetInt("Bundle1Position"));
			PlayerPrefs.SetInt("Bundle2Position", random);

			PlayerPrefs.SetInt("generateDailyBundle", 1);
		}

		for (int i = 0; i < bundleUI.Count; i++)
		{
			bundleUI[i].price.text = bundles[PlayerPrefs.GetInt("Bundle" + i + "Position")].price.ToString() + bundleUI[i].price.text;
			bundleUI[i].icon.sprite = bundles[PlayerPrefs.GetInt("Bundle" + i + "Position")].icon;
		}
	}
}
