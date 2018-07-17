using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ui_level_select : MonoBehaviour
{
	public level_tile prefab_level_tile;
	public Button btn_back;
	public Transform grp_level_tiles;

	private List<level_tile> _lst_level_tiles = new List<level_tile>();

	void Start()
	{
		GLOBAL.level_select = this;

		LoadLevelTiles();
	}

	void OnDestroy()
	{
		if (GLOBAL.level_select == this)
			GLOBAL.level_select = null;
	}

	private void LoadLevelTiles()
	{
		// Clear existing level items.
		_lst_level_tiles.Clear();
		for (int index_level = grp_level_tiles.childCount - 1; index_level > -1; --index_level)
		{
			Transform child = grp_level_tiles.GetChild(index_level);
			Destroy(child.gameObject);
		}

		string path_level_list = "level_list";

		// Debug.Log(string.Format("Loading: '{0}'...", path_level_list));

		TextAsset txt = Resources.Load<TextAsset>(path_level_list);

		string json_text = txt.text;

		// Debug.Log(json_text);

		JSONObject jobj = new JSONObject(json_text);

		if (jobj["level_list"] != null)
		{
			List<JSONObject> lst_jobj_level_list = jobj["level_list"].list;

			for (int index_json = 0; index_json < lst_jobj_level_list.Count; ++index_json)
			{
				JSONObject jobj_level = lst_jobj_level_list[index_json];

				// Debug.Log(string.Format("spawn[{0}].time: {1}", index_json, jobj_level["time"].f));
				// Debug.Log(string.Format("spawn[{0}].row: {1}", index_json, jobj_level["row"].i));
				// Debug.Log(string.Format("spawn[{0}].unit: {1}", index_json, jobj_level["unit"].str));

				level_tile level = Instantiate(prefab_level_tile);
				level.interactable = true;
				level.data.name = jobj_level["name"].str;
				level.data.path_data = jobj_level["data"].str;
				level.data.medal = jobj_level["test_medal"].str;

				level.transform.SetParent(grp_level_tiles, false);

				_lst_level_tiles.Add(level);
			}
		}

		// Debug.Log(string.Format("Load Finished."));

		// Setup the back button navigation.
		if (_lst_level_tiles.Count > 0)
		{
			Navigation nav = new Navigation();
			nav.mode = Navigation.Mode.Explicit;
			nav.selectOnDown = _lst_level_tiles[0];
			// nav.selectOnUp = _lst_level_tiles[_lst_level_tiles.Count -1];
			btn_back.navigation = nav;

			_lst_level_tiles[0].Select();
		}

	}

	public void btn_back_Click()
	{
		SceneManager.LoadScene("ui_title_screen", LoadSceneMode.Single);
	}

	public void LoadLevel(level_tile level)
	{
		Debug.Log("DING! - "+level.data.path_data);

		// Set the level to load.
		manager_player.path_level = level.data.path_data;

		// Begin the level.
		SceneManager.LoadScene("battle_grid", LoadSceneMode.Single);
	}
};