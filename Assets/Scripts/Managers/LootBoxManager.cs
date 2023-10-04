using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Manages lootbox opening.
 * Objects are pulled from a list with different chance of appearing
 * The number of object pulled is determined by the tyep of lootbox
 */

public class LootBoxManager : MonoBehaviour
{
	public static LootBoxManager instance;
	[SerializeField]
	private List<DropList> dropPool;
	[SerializeField]
	private GameObject darkBackgroundPanel;
	[SerializeField]
	private RectTransform LootResultPanel;
	[SerializeField]
	private GameObject LootRowPanel;
	[SerializeField]
	private GameObject LootInfoPanel;
	[SerializeField]
	private GameObject LootTablePanel;
	[SerializeField]
	private List<ScriptableObject> tutorialPool;

	private List<GameObject> rows = new List<GameObject>();

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}

	public void PullLootBox(LootBox lootBox)
	{

		LootTablePanel.SetActive(true);
		darkBackgroundPanel.SetActive(true);
		GameObject rowPanel = null;
		PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") - (int)lootBox.price);
		PlayerPrefs.SetInt("premiumCurrencySpent", PlayerPrefs.GetInt("premiumCurrencySpent") + (int)lootBox.price);
		for (int j = 0; j < lootBox.numberOfDrops; j++)
		{
			int random = Random.Range(0, 100);

			if (j % 3 == 0)
			{
				rowPanel = Instantiate(LootRowPanel, LootResultPanel);
				rows.Add(rowPanel);
			}

			int sum = 0;
			for (int i = 0; i < dropPool.Count; i++)
			{
				sum += dropPool[i].dropRate;
				if (random < sum)
				{
					ScriptableObject droppedItem = dropPool[i].drop[UnityEngine.Random.Range(0, dropPool[i].drop.Count)];
					if (droppedItem is CurrencyBag)
						PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + (droppedItem as CurrencyBag).freeCurrency);
					else if (droppedItem is Skin)
					{
						if (PlayerPrefs.GetInt((droppedItem as Skin).name) == 1)
							PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + (droppedItem as Skin).returnedCurrency);
						else
							PlayerPrefs.SetInt((droppedItem as Skin).name, 1);
					}

					GameObject lootInfo = Instantiate(LootInfoPanel, rowPanel.transform);
					StartCoroutine(lootInfo.GetComponent<LootDisplayAnimation>().DelayAnimation(j));
					lootInfo.transform.GetChild(0).GetComponentInChildren<Image>().sprite = droppedItem is CurrencyBag ? (droppedItem as CurrencyBag).icon : (droppedItem as Skin).icon;
					lootInfo.GetComponentInChildren<Text>().text = droppedItem is CurrencyBag ? (droppedItem as CurrencyBag).freeCurrency.ToString() : (droppedItem as Skin).tierStar;
					break;
				}
			}
		}
		if(SceneManager.GetActiveScene().buildIndex != 9)
			CurrencyManager.instance.UpdateCurrency();
	}

	public void DestroyPull()
	{
		foreach (GameObject row in rows)
			Destroy(row);
	}

	public void PullTutorialLootBox(LootBox lootBox) 
	{
		LootTablePanel.SetActive(true);
		darkBackgroundPanel.SetActive(true);
		GameObject rowPanel = null;
		PlayerPrefs.SetInt("premiumCurrency", PlayerPrefs.GetInt("premiumCurrency") - (int)lootBox.price);
		for (int j = 0; j < lootBox.numberOfDrops; j++)
		{
			if (j % 3 == 0)
			{
				rowPanel = Instantiate(LootRowPanel, LootResultPanel);
				rows.Add(rowPanel);
			}

			ScriptableObject droppedItem = tutorialPool[j];
			if (droppedItem is CurrencyBag)
				PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + (droppedItem as CurrencyBag).freeCurrency);
			else if (droppedItem is Skin)
			{
				if (PlayerPrefs.GetInt((droppedItem as Skin).name) == 1)
					PlayerPrefs.SetInt("freeCurrency", PlayerPrefs.GetInt("freeCurrency") + (droppedItem as Skin).returnedCurrency);
				else
					PlayerPrefs.SetInt((droppedItem as Skin).name, 1);
			}

			GameObject lootInfo = Instantiate(LootInfoPanel, rowPanel.transform);
			StartCoroutine(lootInfo.GetComponent<LootDisplayAnimation>().DelayAnimation(j));
			lootInfo.transform.GetChild(0).GetComponentInChildren<Image>().sprite = droppedItem is CurrencyBag ? (droppedItem as CurrencyBag).icon : (droppedItem as Skin).icon;
			lootInfo.GetComponentInChildren<Text>().text = droppedItem is CurrencyBag ? (droppedItem as CurrencyBag).freeCurrency.ToString() : (droppedItem as Skin).tierStar;
		
		}
		CurrencyManager.instance.UpdateCurrency();
		PlayerPrefs.SetInt("TutorialShop", 1);
		TutorialShop.instance.ChangeText();
	}
}
