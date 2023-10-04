using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public enum CurrencyType
	{
		freeCurrency,
		premiumCurrency
	}

	public static Player instance;
	public PlayerData playerData;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

}
