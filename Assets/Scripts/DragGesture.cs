using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class DragGesture : MonoBehaviour
{ 
	[SerializeField] 
	private LayerMask layerMask;
	[SerializeField] 
	private float swayMultiplier;
	[SerializeField] 
	private float maxOffset = 200f;

	private RaycastHit hitInfo;
	private Vector2 startingPosition;
	private float minDistance = 0.001f;
	private Vector2 offset;

	private void Awake()
	{
		layerMask = LayerMask.GetMask("Default","Target");
    }

	private void OnEnable()
	{
		GameManager.instance.phase = GameManager.Phase.drag;
		GameManager.instance.ShowGesture(6);
	}

	private void OnDisable()
	{
		GameManager.instance.HideGesture();
	}

	void Start()
	{
		DartGenerator.instance.image.GetComponentInChildren<Image>().sprite = DartGenerator.instance.dragPhaseSprite;
	}

	
	void Update()
	{
		if (Physics.Raycast(transform.position + Vector3.forward, Vector3.forward, out hitInfo, 50, layerMask)) 
			DartGenerator.instance.image.position = hitInfo.point-new Vector3(0,0,0.1f);

		if (Input.touchCount == 1 && DartGenerator.instance.canDrag)
		{
			GameManager.instance.HideGesture();

			if (Input.touches[0].phase == TouchPhase.Began)
			{
				startingPosition = Input.touches[0].position;
			}


			offset = Input.touches[0].position - startingPosition;

			float intensity = Mathf.Max(.1f, Mathf.Pow(Mathf.Clamp(offset.magnitude, 0f, maxOffset) / maxOffset, 1 / 1.5f));
			Vector2 scaledByDPI = Commons.ScaleByDPI(offset);

			if (scaledByDPI.sqrMagnitude < minDistance * minDistance)
				offset = Vector2.zero;

			if (((Vector2)transform.position - (Vector2)GameManager.instance.timer.position).x > (GameManager.instance.timer.rect.width / 2) && offset.x > 0 || ((Vector2)transform.position - (Vector2)GameManager.instance.timer.position).x < -(GameManager.instance.timer.rect.width / 2) && offset.x < 0)
				offset = new Vector2(0, offset.y);

			if (((Vector2)transform.position - (Vector2)GameManager.instance.timer.position).y > (GameManager.instance.timer.rect.width / 2) && offset.y > 0 || ((Vector2)transform.position - (Vector2)GameManager.instance.timer.position).y < -(GameManager.instance.timer.rect.width / 2) && offset.y < 0)
				offset = new Vector2(offset.x, 0);


			transform.position += new Vector3(offset.x, offset.y, 0).normalized * Time.deltaTime * (1 / Mathf.Pow(Player.instance.playerData.currentUpgrade.sway + 1, 1 / 1.5f) + .1f / Player.instance.playerData.currentUpgrade.sway) * intensity;

			if (Input.touches[0].phase == TouchPhase.Ended)
			{
				GetComponent<DragGesture>().enabled = false;
				GetComponent<SwipeGesture>().enabled = true;

				if (GameManager.instance.tutorial)
				{
					if (TutorialGrandmaHouse.instance != null)
						TutorialGrandmaHouse.instance.ChangeText();

					if (TutorialPub.instance != null)
						TutorialPub.instance.ChangeText();

					if (TutorialMusicShop.instance != null)
						TutorialMusicShop.instance.ChangeText();

					if (TutorialOffice.instance != null)
						TutorialOffice.instance.ChangeText();

					if (TutorialArena.instance != null)
						TutorialArena.instance.ChangeText();
				}
			}
		}

		else if (Input.touchCount == 0)
			DartGenerator.instance.canDrag = true;
	}

}

