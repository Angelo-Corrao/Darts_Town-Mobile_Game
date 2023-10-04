using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CurrencyManager : MonoBehaviour
{
    [SerializeField] 
	private Text freeCurrencyText;
    [SerializeField] 
	private Text premiumCurrencyText;

	public static CurrencyManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	void Start()
	{
		UpdateCurrency();
	}

	public void UpdateCurrency() 
	{
		freeCurrencyText.text = PlayerPrefs.GetInt("freeCurrency").ToString();
		premiumCurrencyText.text = PlayerPrefs.GetInt("premiumCurrency").ToString();
	}

	public void PlayButtonSound()
	{
		AudioManager.Instance.PlaySFX("Button Tap");
	}

}
