using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinListGenerator : AccessoriesListGenerator
{
	[SerializeField]
	protected Sprite ownedSprite;
	[SerializeField]
	protected Sprite notOwnedSprite;

	private List<Accessories> owned;
	private List<Accessories> notOwned;

	private void Awake()
	{
		owned = new List<Accessories>();
		notOwned = new List<Accessories>();
		GenerateTabs();
	}

	private void GenerateTabs()
	{
		SeparateAccessories();
		CreateAccessoryTab(owned,ownedSprite);
		Instantiate(ownedAccessoriesSeparator, scrollPanel);
		scrollPanel.offsetMin += new Vector2(0, -ownedAccessoriesSeparator.GetComponent<RectTransform>().rect.height);
		CreateAccessoryTab(notOwned,notOwnedSprite);
	}

	private void SeparateAccessories()
	{
		owned.Clear();
		notOwned.Clear();

		for (int i = 0; i < accessories.Count; i++)
		{
			if (PlayerPrefs.GetInt(accessories[i].name) == 1)
				owned.Add(accessories[i]);
			else
				notOwned.Add(accessories[i]);
		}
	}

}
