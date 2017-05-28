using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct Spawning
{
	public float time;
	public int row;
	public unit_troll prefab_unit;
};

public class manager_player : MonoBehaviour
{
	public static string path_level = "levels/level_01_01";

	public LayerMask layer_collectibles;
	public LayerMask layer_cells;

	public Transform grp_units;

	public List<unit_gnome> lst_prefab_gnomes = new List<unit_gnome>();
	public List<unit_troll> lst_prefab_trolls = new List<unit_troll>();

	public int spore_total = 0;

	public unit selected_prefab_unit = null;

	public List<unit_troll> lst_living_trolls = new List<unit_troll>();

	private List<Spawning> _lst_spawnings = new List<Spawning>();
	private float _game_time = 0.0f;

	private bool _game_over = false;


	void Start()
	{
		GLOBAL.manager_player = this;

		if (GLOBAL.game_hud == null)
			SceneManager.LoadSceneAsync("ui_game_hud", LoadSceneMode.Additive);

		LoadLevel();
	}

	void OnDestroy()
	{
		if (GLOBAL.manager_player == this)
			GLOBAL.manager_player = null;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (GLOBAL.game_hud != null)
		{
			GLOBAL.game_hud.txt_spores.text = spore_total.ToString();
		}

		if (selected_prefab_unit != null)
		{
			game_cell cell = null;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_cells))
			{
				cell = hit.transform.GetComponent<game_cell>();

				// Debug.Log(string.Format
				// (
				// 	"Cell '{0}' hit - pos: ({1})",
				// 	cell.name,
				// 	cell.transform.position.ToString()
				// ));

				selected_prefab_unit.transform.position = cell.transform.position;

				cell.Highlight();
			}
			else
			{
				selected_prefab_unit.transform.position = GLOBAL.pos_offscreen;
			}

			// Drop or deselect the prefab unit.
			if (Input.GetMouseButtonDown(0))
			{
				if (Input.GetAxis("Shift") > 0.0f)
				{
					if (cell != null)
					{
						unit new_unit = Instantiate(selected_prefab_unit);
						new_unit.transform.SetParent(grp_units);
						cell.AddUnit(new_unit);
						new_unit.active = true;
					}
				}
				else
				{
					if (cell != null)
					{
						selected_prefab_unit.transform.SetParent(grp_units);
						cell.AddUnit(selected_prefab_unit);
						selected_prefab_unit.active = true;
					}
					else
					{
						Destroy(selected_prefab_unit.gameObject);
					}

					selected_prefab_unit = null;
					GLOBAL.game_hud.DeselectAll();
				}
			}
		}
		else
		{
			// Left-Click to collect things.
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_collectibles))
				{
					collectible collectible = hit.transform.GetComponent<collectible>();

					collectible.Collect();
				}
			}
		}

		// ================
		// Handle spawning.
		// ================

		_game_time += Time.deltaTime;

		// Loop until there are no more units to spawn at the current time.
		while (   _lst_spawnings.Count > 0
			   && _lst_spawnings[0].time <= _game_time)
		{
			Spawning spawning = _lst_spawnings[0];

			Vector3 pos = new Vector3
			(
				GLOBAL.game_field.width,
				0,
				spawning.row
			);

			game_cell cell = GLOBAL.game_field.GetCellAtPosition(pos, true);
			// Debug.Log(string.Format("cell: [{0}]", cell));

			unit new_unit = Instantiate(spawning.prefab_unit);
			new_unit.transform.SetParent(grp_units);
			new_unit.transform.position = pos;
			// Debug.Log(string.Format("new_unit: [{0}]", new_unit));
			cell.AddUnit(new_unit);
			new_unit.active = true;
			new_unit.RegisterUnit();

			_lst_spawnings.RemoveAt(0);
		}

		// If all trolls are dead and no more are coming, player wins.
		if (   _lst_spawnings.Count == 0
			&& lst_living_trolls.Count == 0
			&& !_game_over)
		{
			_game_over = true;
			ui_level_over.Load(true);
		}
	}

	public void GameOverFailure()
	{
		if (!_game_over)
		{
			_game_over = true;
			ui_level_over.Load(false);
		}
	}

	public void SelectPrefabUnit(unit prefab_unit)
	{
		if (prefab_unit == null)
		{
			if (selected_prefab_unit != null)
			{
				Destroy(selected_prefab_unit.gameObject);
				selected_prefab_unit = null;
			}

			Debug.Log(string.Format
			(
				"Unit Destroyed"
			));
		}
		else
		{
			selected_prefab_unit = Instantiate(prefab_unit);
			selected_prefab_unit.transform.position = GLOBAL.pos_offscreen;
			selected_prefab_unit.active = false;

			Debug.Log(string.Format
			(
				"Unit Created - pos: {0}",
				selected_prefab_unit.transform.position.ToString()
			));
		}
	}

	private void LoadLevel()
	{
		Debug.Log(string.Format("Loading: '{0}'...", path_level));

		TextAsset txt = Resources.Load<TextAsset>(path_level);

		string json_text = txt.text;

		Debug.Log(json_text);

		JSONObject json_obj = new JSONObject(json_text);

		if (json_obj["spawnings"] != null)
		{
			List<JSONObject> lst_json_obj_spawnings = json_obj["spawnings"].list;

			for (int index_json = 0; index_json < lst_json_obj_spawnings.Count; ++index_json)
			{
				JSONObject json_obj_spawn = lst_json_obj_spawnings[index_json];

				Debug.Log(string.Format("spawn[{0}].time: {1}", index_json, json_obj_spawn["time"].f));
				Debug.Log(string.Format("spawn[{0}].row: {1}", index_json, json_obj_spawn["row"].i));
				Debug.Log(string.Format("spawn[{0}].unit: {1}", index_json, json_obj_spawn["unit"].str));

				Spawning spawning = new Spawning();

				spawning.time = json_obj_spawn["time"].f;
				spawning.row = (int)json_obj_spawn["row"].i;
				spawning.prefab_unit = StringToPrefabUnit(json_obj_spawn["unit"].str);

				// Figure out what index to insert the spawning at so that spawn times are in order.
				int index_spawning = 0;
				while 
				(
					   index_spawning < _lst_spawnings.Count 
					&& _lst_spawnings[index_spawning].time < spawning.time
				)
				{
					++index_spawning;
				}

				_lst_spawnings.Insert(index_spawning, spawning);
			}
		}

		Debug.Log(string.Format("Load Finished."));
	}

	private unit_troll StringToPrefabUnit(string name)
	{
		for (int index_troll = 0; index_troll < lst_prefab_trolls.Count; ++index_troll)
		{
			unit_troll prefab_unit = lst_prefab_trolls[index_troll];
			if (prefab_unit.name.Equals(name))
			{
				return prefab_unit;
			}
		}

		Debug.LogError(string.Format("Prefab unit with name '{0}' does not exist.", name));

		return null;
	}
};
