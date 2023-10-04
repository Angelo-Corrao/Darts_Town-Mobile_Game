using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContentResizer : MonoBehaviour
{
	[SerializeField] private RectTransform content;
	[SerializeField] private int targetAspectRationWidth;
	[SerializeField] private int targetAspectRationHeight;
	[SerializeField] private int screenOverflow;

	void Start()
	{
		int widthQuotient = Mathf.CeilToInt(Screen.width / targetAspectRationWidth);
		int heightQuotient = Mathf.CeilToInt(Screen.height / targetAspectRationHeight);

		int maxQuotient = Mathf.Max(widthQuotient, heightQuotient);

		int closestAspectWidth = targetAspectRationWidth * maxQuotient;
		int closestAspectHeight = targetAspectRationHeight * maxQuotient;

		float contentResizeTopBottom = closestAspectHeight - Screen.height + closestAspectHeight / screenOverflow;
		float contentResizeLeftRight = closestAspectWidth - Screen.width + closestAspectWidth / screenOverflow;

		content.offsetMin = new Vector2(-contentResizeLeftRight/2, -contentResizeTopBottom / 2);
		content.offsetMax = new Vector2(contentResizeLeftRight/2, contentResizeTopBottom/2);

	}
}
