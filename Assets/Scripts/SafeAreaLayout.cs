using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using S = System;


public class SafeAreaLayout : UIBehaviour, ILayoutSelfController
{
	protected override void Start()
	{
		SetLayoutHorizontal();
		SetLayoutVertical();
	}

	public void SetLayoutHorizontal()
	{
		RectTransform rectTransform = transform as RectTransform;
		rectTransform.anchorMin = new Vector2(
			Screen.safeArea.x / Screen.width,
			rectTransform.anchorMin.y
		);
		rectTransform.anchorMax = new Vector2(
			(Screen.safeArea.x + Screen.safeArea.width) / Screen.width,
			rectTransform.anchorMax.y
		);
	}
	public void SetLayoutVertical()
	{
		RectTransform rectTransform = transform as RectTransform;
		rectTransform.anchorMin = new Vector2(
			rectTransform.anchorMin.x,
			Screen.safeArea.y / Screen.height
		);
		rectTransform.anchorMax = new Vector2(
			rectTransform.anchorMax.x,
			(Screen.safeArea.y + Screen.safeArea.height) / Screen.height
		);
	}
}
