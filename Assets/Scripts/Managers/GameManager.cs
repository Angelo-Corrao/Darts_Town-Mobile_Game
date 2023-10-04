using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Phase 
    {
        drag,
        swipe
    }

    public delegate void OnGamePause();
    public static event OnGamePause onGamePause;

    public delegate void OnGameResume();
    public static event OnGameResume onGameResume;

    public static GameManager instance;
    public Transform target;
    public RectTransform timer;
    public RectTransform scorepanel;
    public GameObject dragGesture;
    public GameObject swipeGesture;
    private GameObject spawnedGesture;
    [HideInInspector]
    public Phase phase;
    public bool isCoroutineRunning;

    [HideInInspector]
    public bool tutorial = false;
    [HideInInspector]
    public int tutorialCheck;
    
	private void Awake()
	{
        if(instance==null)
		    instance = this;
        else
            Destroy(gameObject);
	}

	private void OnEnable()
	{
        GameMode.onGameEnd += PauseGame;
        GameMode.onTimerExpire += HideGesture;
	}

	private void OnDisable()
	{
		GameMode.onGameEnd -= PauseGame;
		GameMode.onTimerExpire -= HideGesture;
	}

	public void LoadScene(int scene) 
    {
        AudioManager.Instance.StopSFXSource();
        SceneManager.LoadScene(scene);
		
	}

    public void PauseGame() 
    {
		onGamePause?.Invoke();
    }

    public void ResumeGame() 
    {
        onGameResume?.Invoke();
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlaySFX("Button Tap");
    }

    public void ShowGesture(float seconds) 
    {
        if(!isCoroutineRunning)
            StartCoroutine(SpawnGesture(seconds));
    }

    public IEnumerator SpawnGesture(float seconds) 
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(seconds);
        spawnedGesture = Instantiate(phase == Phase.drag ? dragGesture : swipeGesture, scorepanel);
    }

    public void HideGesture() 
    {
        StopAllCoroutines();
		isCoroutineRunning = false;
		if (spawnedGesture != null)
            Destroy(spawnedGesture);
    }

}
