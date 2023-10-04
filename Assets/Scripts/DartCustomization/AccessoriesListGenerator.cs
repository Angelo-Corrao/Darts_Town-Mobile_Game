using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AccessoriesListGenerator : MonoBehaviour
{
	[SerializeField]
	protected List<Accessories> accessories;
	[SerializeField]
	protected GameObject panelPrefab, buttonPrefab;
	[SerializeField]
	protected RectTransform scrollPanel;
	[SerializeField]
	protected int buttonsPerRow;
	[SerializeField]
	protected GameObject ownedAccessoriesSeparator;
	[SerializeField]
	private GameObject resetUpgradeButton;
	[SerializeField]
	private Upgrade baseUpgrade;

	private List<GameObject> buttons = new List<GameObject>();
	private List<Upgrade> upgrades = new List<Upgrade>();

	private void Awake()
	{
		CreateAccessoryTab(accessories);
		Instantiate(ownedAccessoriesSeparator, scrollPanel);
		GameObject resetUpgradePanel = Instantiate(resetUpgradeButton, scrollPanel);
		resetUpgradePanel.GetComponentInChildren<Button>().onClick.AddListener(delegate () { DartCustomizationManager.instance.PutUpgrade(baseUpgrade, resetUpgradePanel.transform.GetChild(0).gameObject); });
		scrollPanel.offsetMin += new Vector2(0, -(resetUpgradeButton.GetComponent<RectTransform>().rect.height));
	}

	//Creates a list of accessories dividing them in rows with buttonPerRow elements in each row 
	protected void CreateAccessoryTab(List<Accessories> accessories, params Sprite[] icon)
	{
		double panelToCreate = System.Math.Ceiling((double)accessories.Count / buttonsPerRow);
		
		int counter = 0;

		for (int i = 0; i < panelToCreate; i++)
		{
			GameObject panel = Instantiate(panelPrefab, scrollPanel);
			scrollPanel.offsetMin += new Vector2(0, -panel.GetComponent<RectTransform>().rect.height);
			for (int j = 0; j < buttonsPerRow && counter < panelToCreate*buttonsPerRow; j++, counter++)
			{
				GameObject button = Instantiate(buttonPrefab, panel.transform);
				if (counter < accessories.Count)
				{
					buttons.Add(button);
					if (accessories[counter] is Skin)
					{
						button.GetComponent<Button>().onClick.AddListener(delegate () { DartCustomizationManager.instance.ChangeSkin(accessories[buttons.IndexOf(button)] as Skin); });
						button.GetComponent<Image>().sprite = icon[0];
						button.transform.GetChild(0).GetComponent<Image>().sprite = (accessories[counter] as Skin).icon;
					}
					else if (accessories[counter] is Upgrade)
					{
						upgrades.Add(accessories[counter] as Upgrade);
						button.GetComponent<Button>().onClick.AddListener(delegate () { DartCustomizationManager.instance.PutUpgrade(accessories[buttons.IndexOf(button)] as Upgrade, button); });
						button.transform.GetChild(0).GetComponent<Image>().sprite = PlayerPrefs.GetInt((accessories[counter] as Upgrade).name) == 1 ? (accessories[counter] as Upgrade).icon : (accessories[counter] as Upgrade).notOwnedIcon;
						button.GetComponent<Outline>().effectColor = PlayerPrefs.GetInt((accessories[counter] as Upgrade).name) == 1 ? new Color32(235, 154, 137,255) : new Color32(217, 217, 217, 255);
						if ((accessories[counter] as Upgrade).name.Equals(Player.instance.playerData.currentUpgrade.name)) 
						{
							button.GetComponent<Outline>().effectColor = new Color32(235, 154, 137, 255);
							button.GetComponent<Outline>().effectDistance = new Vector2(12, 12);
						}
					}
				}
				else
					button.transform.localScale = Vector3.zero;

				
			}
		}
	}


	public void ChangeButtonState(GameObject button) 
	{
		for(int i =0;i<buttons.Count;i++)
		{
			buttons[i].GetComponent<Outline>().effectDistance = buttons[i].Equals(button) ? new Vector2(12, 12) : new Vector2(8,8);
			if (buttons[i].Equals(button))
			{
				buttons[i].GetComponent<Outline>().effectColor = new Color32(237, 109, 62, 255);
			}
			else 
			{
				buttons[i].GetComponent<Outline>().effectColor = PlayerPrefs.GetInt(upgrades[i].name) == 1 ? new Color32(235, 154, 137, 255) : new Color32(217, 217, 217, 255);
			}
		}
	}
	
}
