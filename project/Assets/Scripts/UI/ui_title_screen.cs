using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ui_title_screen : MonoBehaviour
{
	void Start()
	{
		
	}

	public void btn_continue_Click()
	{
		SceneManager.LoadScene("ui_level_select", LoadSceneMode.Single);
	}

	public void btn_replay_Click()
	{
		SceneManager.LoadScene("ui_level_select", LoadSceneMode.Single);
	}
};