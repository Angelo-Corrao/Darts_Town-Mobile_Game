using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using JetBrains.Annotations;
using UnityEngine.Rendering;

/*
 * Manages saving parameters and equips the correct skin  and upgrade the player last used
 */

public class SavingDataManager : MonoBehaviour
{
	[Serializable]
	public struct SkinList 
	{
		public List<Skin> skins;
	}

	[Serializable]
	public struct TierList 
	{
		public List<SkinList> tiers;
	}

	private string savingPath;

	[SerializeField]
	private GameObject dartPrefab;
	[SerializeField]
	private List<TierList> skins;
	[SerializeField]
	private List<Upgrade> upgrades;

	private Renderer bodyRenderer;
	private Renderer tipRenderer;
	private Renderer flightRenderer;

	private void Start()
	{

		savingPath = Application.persistentDataPath + "/DailyChallenges.txt";
		bodyRenderer = dartPrefab.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
		tipRenderer = dartPrefab.transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
		flightRenderer = dartPrefab.transform.GetChild(0).GetChild(2).GetComponent<Renderer>();

		if (!File.Exists(savingPath))
			File.Create(savingPath);

		if (!PlayerPrefs.HasKey("firstLogin"))
		{
			PlayerPrefs.SetString("firstLogin", DateTime.Today.ToString());
			PlayerPrefs.SetInt("freeCurrency", 0);
			PlayerPrefs.SetInt("premiumCurrency", 0);
			PlayerPrefs.SetInt("freeCurrencySpent", 0);
			PlayerPrefs.SetInt("premiumCurrencySpent", 0);
			PlayerPrefs.SetInt("matchesWon", 0);
			PlayerPrefs.SetInt("matchesPlayed", 0);
			PlayerPrefs.SetInt("bullseyeScored", 0);
			PlayerPrefs.SetInt("doubleScored", 0);
			PlayerPrefs.SetInt("triplesScored", 0);
			PlayerPrefs.SetInt("maxPointsInARound", 0);
			PlayerPrefs.SetInt("weeklyFairTimesAttempted", 0);
			PlayerPrefs.SetInt("weeklyFairTimesWon", 0);
			PlayerPrefs.SetInt("weeklyFairPrizeClaimed", 0);
			PlayerPrefs.SetInt("grandmaHouse", 0);
			PlayerPrefs.SetInt("office", 0);
			PlayerPrefs.SetInt("pub", 0);
			PlayerPrefs.SetInt("musicStore", 0);
			PlayerPrefs.SetInt("arena", 0);
			PlayerPrefs.SetInt("grandmaHouseVictories", 0);
			PlayerPrefs.SetInt("officeVictories", 0);
			PlayerPrefs.SetInt("pubVictories", 0);
			PlayerPrefs.SetInt("musicStoreVictories", 0);
			PlayerPrefs.SetInt("arenaVictories", 0);
			PlayerPrefs.SetInt("totalMatchesWon", 0);
			PlayerPrefs.SetInt("totalMatchesPlayed", 0);
			PlayerPrefs.SetInt("grandmaHouseDaily", 0);
			PlayerPrefs.SetInt("officeDaily", 0);
			PlayerPrefs.SetInt("pubDaily", 0);
			PlayerPrefs.SetInt("musicStoreDaily", 0);
			PlayerPrefs.SetInt("arenaDaily", 0);
			PlayerPrefs.SetInt("TutorialShop", 0);
			PlayerPrefs.SetInt("TutorialInventory", 0);
			PlayerPrefs.SetString("equippedBody", "BT10");
			PlayerPrefs.SetString("equippedTip", "TT10");
			PlayerPrefs.SetString("equippedFlight", "FT10");
			PlayerPrefs.SetString("equippedUpgrade", "Base");

			for (int i = 0; i < skins.Count; i++) 
			{
				for (int j = 0; j<skins[i].tiers.Count; j++) 
				{
					for (int k = 0; k < skins[i].tiers[j].skins.Count; k++) 
					{
						PlayerPrefs.SetInt(skins[i].tiers[j].skins[k].name, 0);
					}
				}
			}

			PlayerPrefs.SetInt("BT10", 1);
			PlayerPrefs.SetInt("TT10", 1);
			PlayerPrefs.SetInt("FT10", 1);

			for (int i = 0; i < upgrades.Count; i++) 
			{
				PlayerPrefs.SetInt(upgrades[i].name, 0);
			}

			PlayerPrefs.SetInt(upgrades[0].name, 1);
			Player.instance.playerData.currentUpgrade = upgrades[0];

		}

		EquipSkin(skins, PlayerPrefs.GetString("equippedBody"));
		EquipSkin(skins, PlayerPrefs.GetString("equippedTip"));
		EquipSkin(skins, PlayerPrefs.GetString("equippedFlight"));

		for (int i = 0; i < upgrades.Count; i++) 
		{
			if (upgrades[i].name.Equals(PlayerPrefs.GetString("equippedUpgrade")))
			{
				Player.instance.playerData.currentUpgrade = upgrades[i];
				break;
			}
		}

	}

	private void EquipSkin(List<TierList> skins, string skinID) 
	{
		string dartPartString = skinID.Substring(0, 1);
		Skin.DartPart dartPart = Skin.DartPart.Body;
		switch (dartPartString) 
		{
			case "B":
				dartPart = Skin.DartPart.Body;
				break;
			case "T":
				dartPart = Skin.DartPart.Tip;
				break;
			case "F":
				dartPart = Skin.DartPart.Flight;
				break;
		}
		int skinTier = int.Parse(skinID.Substring(2, 1));
		int skinPosition = int.Parse(skinID.Substring(3));

		Skin skin = skins[(int)dartPart].tiers[skinTier - 1].skins[skinTier == 1 ? skinPosition : skinPosition - 1];

		dartPrefab.transform.GetChild(0).GetChild((int)skin.dartPart).GetComponent<Renderer>().sharedMaterial = skin.material;
		dartPrefab.transform.GetChild(0).GetChild((int)skin.dartPart).GetComponent<MeshFilter>().sharedMesh = skin.mesh;

		switch (skin.dartPart)
		{
			case Skin.DartPart.Body:
				SetShaderColor(ref bodyRenderer, skin);
				break;
			case Skin.DartPart.Tip:
				SetShaderColor(ref tipRenderer, skin);
				break;
			case Skin.DartPart.Flight:
				SetShaderColor(ref flightRenderer, skin);
				break;
		}
	}

	private void SetShaderColor(ref Renderer dartPart, Skin skin)
	{
		ShaderScript.instance.SetColorInShader("_PrimaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._PrimaryColor));
		ShaderScript.instance.SetColorInShader("_SecondaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._SecondaryColor));
		ShaderScript.instance.SetColorInShader("_TertiaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._TertiaryColor));
	}

	public void PlayButtonSound()
	{
		AudioManager.Instance.PlaySFX("Button Tap");
	}

}
