using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Manages the dart during a match
 */

public class DartGenerator : MonoBehaviour
{
	public static DartGenerator instance;

	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private Transform spawner;

	public bool canDrag = true;

	public List<GameObject> dartsGenerated = new List<GameObject>();

	
	public RectTransform image;
	
	public Sprite dragPhaseSprite;
	
	public Sprite swipePhaseSprite;

	public bool pauseGame = false;


	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void OnEnable()
	{
		GameMode.onTimerExpire += TurnTimeElapsed;
		GameManager.onGamePause += DisableDart;
		GameManager.onGameResume += EnableDart;
	}

	private void OnDisable()
	{
		GameMode.onTimerExpire -= TurnTimeElapsed;
		GameManager.onGamePause -= DisableDart;
		GameManager.onGameResume -= EnableDart;
	}

	void Start()
	{
		GenerateNewDart();
		float aimScale = Mathf.Lerp(0.9f, 0.2f, Player.instance.playerData.currentUpgrade.precision / 10);
		image.localScale = new Vector3(aimScale, aimScale, 0);

	}

	public void GenerateNewDart()
	{
		GameObject dart = Instantiate(Player.instance.playerData.currentDart, spawner.position - new Vector3(0, 0.4f, 0), Quaternion.identity);
		dartsGenerated.Add(dart);
		dart.transform.SetParent(spawner);
		canDrag = false;
		if(pauseGame)
			DisableDart();
	}

	public void DeleteDarts(int numberOfDarts) 
	{
		for (int i = 0; i < numberOfDarts; i++) 
		{
			Destroy(dartsGenerated[0]);
			dartsGenerated.RemoveAt(0);
		}
	}

	public void TurnTimeElapsed() 
	{
		dartsGenerated[dartsGenerated.Count - 1].SetActive(false);
		canDrag = false;
		GenerateNewDart();
	}

	public void EnableDart() 
	{
		dartsGenerated[dartsGenerated.Count - 1].SetActive(true);
		pauseGame = false;
		
	}

	public void DisableDart() 
	{
		dartsGenerated[dartsGenerated.Count - 1].SetActive(false);
		pauseGame = true;
	}

	
}
