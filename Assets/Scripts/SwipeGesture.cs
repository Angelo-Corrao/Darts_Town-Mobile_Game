using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SwipeGesture : MonoBehaviour
{

	public delegate void OnSwipe(float verticalOffest, float horizontalOffset);
	public static event OnSwipe onSwipe;

	[SerializeField] private float swipeAngle;

	private float minSwipeDistanceCm;
	private float swipeStartTime;
	private Vector2 swipeStartPosition;
	private bool swiping = false;

	void Start()
	{
		minSwipeDistanceCm = Commons.ScaleByDPI(new Vector2(0, Screen.height / 10)).y;
		DartGenerator.instance.image.GetComponentInChildren<Image>().sprite = DartGenerator.instance.swipePhaseSprite;
		GameManager.instance.phase = GameManager.Phase.swipe;
		GameManager.instance.ShowGesture(6);

	}

	private void Update()
	{
		Swipe();
	}

	private void Swipe() {

		if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
			StartSwipe(Input.touches[0]);
		else if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
			EndSwipe(Input.touches[0]);
		else if (
				swiping &&
				Input.touchCount > 1
			)
			CancelSwipe();
	}

	private void StartSwipe(Touch touch)
	{
		swipeStartPosition = touch.position;
		swipeStartTime = Time.unscaledTime;
		swiping = true;
		GameManager.instance.HideGesture();
	}

	private void EndSwipe(Touch touch)
	{
		swiping = false;

		Vector2 swipeOffset = touch.position - swipeStartPosition;
		Vector2 scaled = Commons.ScaleByDPI(swipeOffset);

		if (scaled.sqrMagnitude < minSwipeDistanceCm * minSwipeDistanceCm) 
		{
			GameManager.instance.ShowGesture(6);
			return;
		}

		swipeOffset = new Vector2(swipeOffset.x, Mathf.Clamp(swipeOffset.y, Screen.height / 10, Screen.height / 2));

		if (Vector3.Dot(Vector3.up, swipeOffset.normalized) >= Mathf.Cos(Mathf.Deg2Rad * swipeAngle))
		{
			GameManager.instance.HideGesture();
			onSwipe?.Invoke(Mathf.Lerp(-DartGenerator.instance.image.localScale.x / 2, DartGenerator.instance.image.localScale.y, Mathf.Abs(swipeOffset.y - Screen.height / 10) / (Screen.height / 2)), swipeOffset.x);
		}
	}

	private void CancelSwipe()
	{
		swiping = false;
		GameManager.instance.ShowGesture(6);
	}
}
