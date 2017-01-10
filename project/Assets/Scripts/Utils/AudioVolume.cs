using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioVolume : MonoBehaviour
{
	public enum AudioType
	{
		Music,
		SoundEffect
	}

	public AudioType type = AudioType.SoundEffect;
	[Range(0.0f, 1.0f)]
	public float max_volume = 1.0f;
	private float _relative_volume = 1.0f;
	public float relative_volume
	{
		get { return _relative_volume; }
		set
		{
			_relative_volume = Mathf.Clamp(value, 0.0f, 1.0f);

			UpdateVolume();
		}
	}

	private AudioSource _audio;

	void Start()
	{
		_audio = GetComponent<AudioSource>();

		GLOBAL.RegisterAudio(this);

		UpdateVolume();
	}

	void OnDestroy()
	{
		GLOBAL.UnregisterAudio(this);
	}
	
	public void UpdateVolume()
	{
		if (type == AudioType.Music)
			_audio.volume = _relative_volume * GLOBAL.volume_master * max_volume * GLOBAL.volume_music;
		else if (type == AudioType.SoundEffect)
			_audio.volume = _relative_volume * GLOBAL.volume_master * max_volume * GLOBAL.volume_sound_effects;
	}
}