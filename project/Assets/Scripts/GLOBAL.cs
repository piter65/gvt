using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public static class GLOBAL
{
	public static readonly Vector3 pos_offscreen = new Vector3(-5.0f, 0.0f, 0.0f);
	public static readonly Vector3 nudge_up = new Vector3(0.0f, 0.1f, 0.0f);

	public static bool is_paused = false;
	public static float volume_master = 0.25f;
	public static float volume_music = 0.5f;
	public static float volume_sound_effects = 1.0f;

	//public static ui_menu menu_title = null;
	//public static ui_menu menu_options = null;
	//public static ui_menu menu_pause = null;

	public static game_field game_field = null;
	public static manager_player manager_player = null;

	public static ui_game_hud game_hud = null;

	private static List<AudioVolume> _lstAudio = new List<AudioVolume>();

	public static void Pause()
	{
		is_paused = true;
	}

	public static void Unpause()
	{
		is_paused = false;
	}

	public static void LoadMenus()
	{
		//if (menu_title == null)
		//	SceneManager.LoadSceneAsync("ui_menu_title", LoadSceneMode.Additive);
		//if (menu_options == null)
		//	SceneManager.LoadSceneAsync("ui_menu_options", LoadSceneMode.Additive);
		//if (menu_pause == null)
		//	SceneManager.LoadSceneAsync("ui_menu_pause", LoadSceneMode.Additive);
	}

	public static void RegisterAudio(AudioVolume audio)
	{
		_lstAudio.Add(audio);
	}

	public static void UnregisterAudio(AudioVolume audio)
	{
		_lstAudio.Remove(audio);
	}

	public static void VolumeUpdated()
	{
		foreach (AudioVolume audio in _lstAudio)
			audio.UpdateVolume();
	}

	public static void ExitLevel()
	{
		SceneManager.LoadScene("game", LoadSceneMode.Single);
	}
}