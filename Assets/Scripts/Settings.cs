using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public static Settings instance;
	public AudioMixer mainMixer;
	public Slider musicSlider;
	public Slider sfxSlider;

	private void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start() {
		LoadSettings();
	}

	public void SetMusicVolume(float value) {
		mainMixer.SetFloat("music", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("musicVolume", value);
	}

	public void SetSFXVolume(float value) {
		mainMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("sfxVolume", value);
	}

	public void LoadSettings() {
		if (PlayerPrefs.HasKey("musicVolume")) {
			musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
		}

		if (PlayerPrefs.HasKey("sfxVolume")) {
			sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
		}
	}
}
