using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages Map upgrades ulockment based on specific parameters
 */

public class MapUpgradeManager : MonoBehaviour
{
	[Serializable]
	public struct MapUpgrades 
	{
		public List<GameObject> levelUpgrades;
	}

	[SerializeField] 
	private GameObject tutorialEndUpgrades;
	[SerializeField] 
	private GameObject pubSignUpgrade;
	[SerializeField] 
	private List<GameObject> shopInventoryUpgrades;
	[SerializeField] 
	private List<GameObject> fairUpgrades;
	[SerializeField] 
	private Image fair;
	[SerializeField] 
	private Image[] levelSprites = new Image[5];
	[SerializeField] 
	private Image[] inventoryStore;
	[SerializeField] 
	private Sprite unlockedFair;
	[SerializeField] 
	private Sprite[] inventoryStoreSprite;
	[SerializeField] 
	private Level[] levels= new Level[5];
	[SerializeField] 
	private List<MapUpgrades> upgrades;

	private int upgradeToUnlock;

	private void Awake()
	{
		for (int i = 0; i < levels.Length; i++) 
		{
			upgradeToUnlock = levels[i].UnlockMapUpgrade();

			for (int j = 0; j <= upgradeToUnlock; j++) 
			{
				upgrades[i].levelUpgrades[j].SetActive(true);
			}

			levelSprites[i].sprite = levels[i].ChangeSprite();
		}

		if (PlayerPrefs.GetInt("totalMatchesPlayed") >= 3) 
		{
			for (int i = 0; i < inventoryStore.Length; i++) 
			{
				inventoryStore[i].sprite = inventoryStoreSprite[i];
			}
		}

		if (PlayerPrefs.GetInt("freeCurrencySpent") > 5000)
			shopInventoryUpgrades[0].SetActive(true);
		if(PlayerPrefs.GetInt("premiumCurrencySpent") > 1000)
			shopInventoryUpgrades[1].SetActive(true);

		if (PlayerPrefs.GetInt("TutorialShop") == 2) 
				tutorialEndUpgrades.SetActive(true);

		if (PlayerPrefs.GetInt("totalMatchesPlayed") >= 5)
		{
			fair.sprite = unlockedFair;

			foreach (GameObject upgrade in fairUpgrades) 
			{
				upgrade.SetActive(true);
			}
		}

		if (levels[1].CheckUnlocked())
			pubSignUpgrade.SetActive(true);
	}
}
