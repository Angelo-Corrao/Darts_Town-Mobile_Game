using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/*
 * Manages skins and upgrades purchasing and equipping
 */

public class DartCustomizationManager : MonoBehaviour
{
	[SerializeField]
	private GameObject dartPrefab;
	[SerializeField]
	private GameObject dartInScene;
	[SerializeField]
	private GameObject shopPanel;
	[SerializeField]
	private GameObject upgradePanel;
	[SerializeField]
	private GameObject upgradePurchasePanel;
	[SerializeField]
	private GameObject upgradePurchasePanelFreeCurrency;
	[SerializeField]
	private GameObject upgradePurchasePanelPremiumCurrency;
	[SerializeField]
	private GameObject darkBackgroundPanel;
	[SerializeField]
	private GameObject userMessage;
	[SerializeField]
	private GameObject tutorialManager;
	[SerializeField]
	private GameObject topUpMessage;
	[SerializeField]
	private Text precisionStat;
	[SerializeField]
	private Text swayStat;
	[SerializeField]
	private AccessoriesListGenerator generator;

	public static DartCustomizationManager instance;

	private Upgrade selectedUpgrade;
	private Player.CurrencyType selectedPaymentType;

	private Renderer bodyRendererPrefab;
	private Renderer tipRendererPrefab;
	private Renderer flightRendererPrefab;
	private Renderer bodyRendererScene;
	private Renderer tipRendererScene;
	private Renderer flightRendererScene;
	private MeshFilter bodyMeshPrefab;
	private MeshFilter tipMeshPrefab;
	private MeshFilter flightMeshPrefab;
	private MeshFilter bodyMeshScene;
	private MeshFilter tipMeshScene;
	private MeshFilter flightMeshScene;


	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		bodyRendererPrefab = dartPrefab.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
		tipRendererPrefab = dartPrefab.transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
		flightRendererPrefab = dartPrefab.transform.GetChild(0).GetChild(2).GetComponent<Renderer>();
		bodyRendererScene = dartInScene.transform.GetChild(0).GetComponent<Renderer>();
		tipRendererScene = dartInScene.transform.GetChild(1).GetComponent<Renderer>();
		flightRendererScene = dartInScene.transform.GetChild(2).GetComponent<Renderer>();

		bodyMeshPrefab = dartPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>();
		tipMeshPrefab = dartPrefab.transform.GetChild(0).GetChild(1).GetComponent<MeshFilter>();
		flightMeshPrefab = dartPrefab.transform.GetChild(0).GetChild(2).GetComponent<MeshFilter>();
		bodyMeshScene = dartInScene.transform.GetChild(0).GetComponent<MeshFilter>();
		tipMeshScene = dartInScene.transform.GetChild(1).GetComponent<MeshFilter>();
		flightMeshScene = dartInScene.transform.GetChild(2).GetComponent<MeshFilter>();
	}
	void Start()
	{
		UpdateDartStats();

		bodyRendererScene.sharedMaterial = bodyRendererPrefab.sharedMaterial;
		tipRendererScene.sharedMaterial = tipRendererPrefab.sharedMaterial;
		flightRendererScene.sharedMaterial = flightRendererPrefab.sharedMaterial;
		bodyMeshScene.mesh = bodyMeshPrefab.sharedMesh;
		tipMeshScene.mesh = tipMeshPrefab.sharedMesh;
		flightMeshScene.mesh = flightMeshPrefab.sharedMesh;
		AudioManager.Instance.PlayMusic("Inventory");
	}

	public void LoadGameScene(int scene)
	{
		PlayerPrefs.SetInt("previousScene", SceneManager.GetActiveScene().buildIndex);
		SceneManager.LoadScene(scene);
	}

	public void ChangeSkin(Skin skin)
	{
		//checks if the player owns the skin
		if (PlayerPrefs.GetInt(skin.name) == 1)
		{

			switch (skin.dartPart)
			{
				case Skin.DartPart.Body:
					bodyRendererPrefab.sharedMaterial = skin.material;
					bodyRendererScene.sharedMaterial = skin.material;
					bodyMeshPrefab.sharedMesh = skin.mesh;
					bodyMeshScene.sharedMesh = skin.mesh;
					SetShaderColor(ref bodyRendererPrefab, skin);
					PlayerPrefs.SetString("equippedBody", skin.name);
					break;
				case Skin.DartPart.Tip:
					tipRendererPrefab.sharedMaterial = skin.material;
					tipRendererScene.sharedMaterial = skin.material;
					tipMeshPrefab.sharedMesh = skin.mesh;
					tipMeshScene.sharedMesh = skin.mesh;
					SetShaderColor(ref tipRendererPrefab, skin);
					PlayerPrefs.SetString("equippedTip", skin.name);
					break;
				case Skin.DartPart.Flight:
					flightRendererPrefab.sharedMaterial = skin.material;
					flightRendererScene.sharedMaterial = skin.material;
					flightMeshPrefab.sharedMesh = skin.mesh;
					flightMeshScene.sharedMesh = skin.mesh;
					SetShaderColor(ref flightRendererPrefab, skin);
					PlayerPrefs.SetString("equippedFlight", skin.name);
					break;
			}

			if (tutorialManager.activeSelf)
				TutorialInventory.instance.ChangeText();
		}
		else
		{
			if (tutorialManager.activeSelf)
			{
				userMessage.GetComponentInChildren<Text>().text = "Firstly equip the skin you just found";
				userMessage.SetActive(true);
			}
			else
				shopPanel.SetActive(true);

			darkBackgroundPanel.SetActive(true);
		}
	}

	public void PutUpgrade(Upgrade upgrade, GameObject button)
	{
		//checks if the player owns the upgrade
		if (PlayerPrefs.GetInt(upgrade.name) == 1)
		{
			Player.instance.playerData.currentUpgrade = upgrade;
			PlayerPrefs.SetString("equippedUpgrade", upgrade.name);
			generator.ChangeButtonState(button);
			UpdateDartStats();
		}
		else
		{
			darkBackgroundPanel.SetActive(true);
			upgradePanel.SetActive(true);
			upgradePanel.GetComponentInChildren<Text>().text = $"Do you want to buy \n {upgrade.name} \n Precision: {upgrade.precision} \n Sway: {upgrade.sway}";
			upgradePurchasePanelFreeCurrency.GetComponentInChildren<Text>().text = upgrade.priceCoins.ToString();
			upgradePurchasePanelPremiumCurrency.GetComponentInChildren<Text>().text = upgrade.pricePremium.ToString();
			selectedUpgrade = upgrade;
		}
	}

	public void SetPaymentType(int paymentType)
	{
		selectedPaymentType = (Player.CurrencyType)paymentType;
		upgradePurchasePanel.GetComponentInChildren<Text>().text = $"Do you want to buy {selectedUpgrade.name} for {(selectedPaymentType == Player.CurrencyType.freeCurrency ? selectedUpgrade.priceCoins : selectedUpgrade.pricePremium)} {(selectedPaymentType == Player.CurrencyType.freeCurrency ? "free currency" : "premium currency")}";
	}


	public void BuyUpgrade()
	{
		if (PlayerPrefs.GetInt(selectedPaymentType.ToString()) >= (selectedPaymentType == Player.CurrencyType.freeCurrency ? selectedUpgrade.priceCoins : selectedUpgrade.pricePremium))
		{
			PlayerPrefs.SetInt(selectedUpgrade.name, 1);
			PlayerPrefs.SetInt(selectedPaymentType.ToString(), PlayerPrefs.GetInt(selectedPaymentType.ToString()) - (selectedPaymentType == Player.CurrencyType.freeCurrency ? selectedUpgrade.priceCoins : selectedUpgrade.pricePremium));
			PlayerPrefs.SetInt(selectedPaymentType.ToString() + "Spent", PlayerPrefs.GetInt(selectedPaymentType.ToString() + "Spent") + (selectedPaymentType == Player.CurrencyType.freeCurrency ? selectedUpgrade.priceCoins : selectedUpgrade.pricePremium));
			SceneManager.LoadScene(2);
			upgradePurchasePanel.SetActive(false);
			darkBackgroundPanel.SetActive(false);
			if (tutorialManager.activeSelf)
				TutorialInventory.instance.ChangeText();
		}
		else
		{
			if (selectedPaymentType == Player.CurrencyType.premiumCurrency)
			{
				topUpMessage.SetActive(true);
			}
			else
			{
				userMessage.GetComponentInChildren<Text>().text = "You don't have enough resources";
				userMessage.SetActive(true);
			}
		}
	}

	private void UpdateDartStats()
	{
		precisionStat.text = Player.instance.playerData.currentUpgrade.precision.ToString();
		swayStat.text = Player.instance.playerData.currentUpgrade.sway.ToString();
	}

	private void SetShaderColor(ref Renderer dartPart, Skin skin)
	{
		ShaderScript.instance.SetColorInShader("_PrimaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._PrimaryColor));
		ShaderScript.instance.SetColorInShader("_SecondaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._SecondaryColor));
		ShaderScript.instance.SetColorInShader("_TertiaryColor", ref dartPart, ShaderScript.instance.HexToColor(skin._TertiaryColor));
	}

}