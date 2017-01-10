using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager_player : MonoBehaviour
{
	public LayerMask layer_collectibles;
	public LayerMask layer_cells;

	public Transform grp_units;

	public List<unit_mushroom> lst_prefab_mushrooms = new List<unit_mushroom>();
	public List<unit_gnome> lst_prefab_gnomes = new List<unit_gnome>();
	public List<unit_troll> lst_prefab_trolls = new List<unit_troll>();

	public int spore_total = 0;

	public unit selected_prefab_unit = null;


	void Start()
	{
		GLOBAL.manager_player = this;

		if (GLOBAL.game_hud == null)
			SceneManager.LoadSceneAsync("ui_game_hud", LoadSceneMode.Additive);
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
}
