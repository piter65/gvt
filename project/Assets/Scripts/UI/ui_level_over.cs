﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ui_level_over : MonoBehaviour
{
	public TMP_Text txt_success;
	public TMP_Text txt_failure;

	private static bool _success;

	public static void Load(bool success)
	{
		_success = success;
		SceneManager.LoadScene("ui_level_over", LoadSceneMode.Additive);
	}

	void Start()
	{
		txt_success.gameObject.SetActive(_success);
		txt_failure.gameObject.SetActive(!_success);
	}
	
	public void overlay_Click()
	{
		// GLOBAL.ExitLevel();
	}

	public void btn_level_select_Click()
	{
		SceneManager.LoadScene("ui_level_select", LoadSceneMode.Single);
	}

	public void btn_retry_Click()
	{
		SceneManager.LoadScene("battle_grid", LoadSceneMode.Single);
	}
}
