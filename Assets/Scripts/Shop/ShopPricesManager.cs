using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPricesManager : MonoBehaviour
{
	[SerializeField]
	private List<Text> priceTags;
	[SerializeField]
	private List<ShopItem> shopItems;

	private void Start()
	{
		for (int i = 0; i < priceTags.Count; i++)
		{
			priceTags[i].text = shopItems[i].price.ToString() + priceTags[i].text;
		}
	}
}
