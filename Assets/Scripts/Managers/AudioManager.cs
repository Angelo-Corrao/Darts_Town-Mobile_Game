using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager Instance {
		get; private set;
	}
	public AudioSource musicSource;
	public AudioSource sfxSource;
	public Sound[] musicSounds;
	public Sound[] sfxSounds;

	private bool isSourcePlayingMultipleClip;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void PlayMusic(string clipName) {
		AudioSource source = AudioSourceToUse(clipName);
		if (source == null)
			source = musicSource;
		Sound sound = Array.Find(musicSounds, x => x.name == clipName);

		if (sound != null) {
			source.clip = sound.clip;
			source.Play();
		}
	}

	public void PlaySFX(string clipName) {
		AudioSource source = AudioSourceToUse(clipName);
		Sound sound = Array.Find(sfxSounds, x => x.name == clipName);

		if (sound != null) {
			if (source != null) {
				if (isSourcePlayingMultipleClip)
					source.PlayOneShot(sound.clip);
				else {
					source.clip = sound.clip;
					source.Play();
				}
			}
			else {
				sfxSource.PlayOneShot(sound.clip);
			}
		}
	}

	public AudioSource AudioSourceToUse(string clipName) {
		AudioSource source = null;

		return source;
	}

	public void StopAllSources() {
		musicSource.Stop();
		sfxSource.Stop();
	}

	public void StopSFXSource() 
	{
		sfxSource.Stop();
	}
}
