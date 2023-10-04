using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.VisualScripting;

/*
 * Manages all the transactions in the shop
 */

public class ShopManager : MonoBehaviour
{

	[SerializeField]
	private GameObject darkBackgroundPanel;
	[SerializeField]
	private GameObject loadingScreen;
	[SerializeField]
	private GameObject loadingImage;
	[SerializeField]
	private GameObject paymentResultText;

	private LootBox selectedBox;
	private TopUp selectedTopUp;
	private DailyBundle selectedDailyBundle;
	private int bundlePosition;

	[SerializeField]
	private GameObject lootBoxPopUp;

	[SerializeField]
	private GameObject topUpPanel;
	[SerializeField]
	private GameObject bundlePanel;

	[SerializeField]
	private RectTransform screen;
	[SerializeField]
	private GameObject UserMessage;

	[SerializeField]
	private Button[] dealsOfTheDay;

	[SerializeField]
	private Animator loadingPayment;

	private void Awake()
	{
		for (int i = 0; i < dealsOfTheDay.Length; i++) 
		{
			if (!PlayerPrefs.HasKey("bundle" + i + "interactable"))
				PlayerPrefs.SetInt("bundle" + i + "interactable", 1);
		}
	}

	private void Start()
	{
		AudioManager.Instance.PlayMusic("Shop");

		for (int i = 0; i < dealsOfTheDay.Length; i++) 
		{
			dealsOfTheDay[i].interactable = PlayerPrefs.GetInt("bundle" + i + "interactable") == 1;
		}
	}

	public void LoadScene() 
	{
		SceneManager.LoadScene(PlayerPrefs.GetInt("previousScene")); 
	}

	public void SaveSelectedBundle(int position) 
	{
		selectedDailyBundle = DailyBundleManager.instance.bundles[PlayerPrefs.GetInt("Bundle" + position.ToString() + "Position")];
		bundlePosition = position;
		darkBackgroundPanel.SetActive(true);
		bundlePanel.SetActive(true);
		bundlePanel.GetComponentInChildren<Text>().text = $"Are you sure you want to purchase {selectedDailyBundle.premiumCurrency} premium currency for {selectedDailyBundle.price}$?";
	}

	public void BuyBundle() 
	{
		StartCoroutine(Payment(bundlePanel));
		PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + selectedDailyBundle.premiumCurrency);
		dealsOfTheDay[bundlePosition].interactable = false;
		PlayerPrefs.SetInt("bundle" + bundlePosition + "interactable", 0);
	}

	public void SaveSelectedLootBox(LootBox	lootBox) 
	{
		selectedBox = lootBox;
		darkBackgroundPanel.SetActive(true);
		lootBoxPopUp.SetActive(true);
		lootBoxPopUp.GetComponentInChildren<Text>().text = $"Are you sure that you want to spend {lootBox.price} premium currency to open this loot box?";
	}

	public void CheckPlayerCurrency() 
	{
		if (PlayerPrefs.GetInt("premiumCurrency") < selectedBox.price)
		{
			UserMessage.GetComponentInChildren<Text>().text = "You don't have enough resources to buy this loot box";
			UserMessage.SetActive(true);
			darkBackgroundPanel.SetActive(true);
		}
		else
			LootBoxManager.instance.PullLootBox(selectedBox);
	}

	public void SaveSelectedTopUp(TopUp topUp) 
	{
		selectedTopUp = topUp;
		darkBackgroundPanel.SetActive(true);
		topUpPanel.SetActive(true);
		topUpPanel.GetComponentInChildren<Text>().text = $"Are you sure you want to purchase {topUp.premiumCurrency} premium currency for {topUp.price}$?";
	}

	public void TopUpPremiumCurrency() 
	{
		StartCoroutine(Payment(topUpPanel));
		PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") + selectedTopUp.premiumCurrency);
	}

	public IEnumerator Payment(GameObject panel) 
	{
		panel.SetActive(false);
		loadingScreen.SetActive(true);
		yield return new WaitForSeconds(loadingPayment.GetCurrentAnimatorStateInfo(0).length);
		loadingImage.SetActive(false);
		paymentResultText.SetActive(true);
		CurrencyManager.instance.UpdateCurrency();
		yield return new WaitForSeconds(1.5f);
		loadingScreen.SetActive(false);
		loadingImage.SetActive(true);
		paymentResultText.SetActive(false);
		darkBackgroundPanel.SetActive(false);
	}
}
