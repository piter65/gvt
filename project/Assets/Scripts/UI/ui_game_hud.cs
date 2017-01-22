using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ui_game_hud : MonoBehaviour
{
	public Text txt_spores;
	public ToggleGroup grp_gnomes;
	public ToggleGroup grp_trolls;

	public tog_unit_select prefab_tog_unit_select;

	void Start()
	{
		GLOBAL.game_hud = this;

		EmptyToggleGroups();

		FillToggleGroups();
	}

	void OnDestroy()
	{
		if (GLOBAL.game_hud == this)
			GLOBAL.game_hud = null;
	}
	
	void Update()
	{
		
	}

	private void EmptyToggleGroups()
	{
		for (int index_tog = grp_gnomes.transform.childCount - 1; index_tog > -1; --index_tog)
		{
			Toggle tog = grp_gnomes.transform.GetChild(index_tog).GetComponent<Toggle>();
			grp_gnomes.UnregisterToggle(tog);
			Destroy(tog.gameObject);
		}
		for (int index_tog = grp_trolls.transform.childCount - 1; index_tog > -1; --index_tog)
		{
			Toggle tog = grp_trolls.transform.GetChild(index_tog).GetComponent<Toggle>();
			grp_trolls.UnregisterToggle(tog);
			Destroy(tog.gameObject);
		}
	}

	private void FillToggleGroups()
	{
		for (int index_unit = 0; index_unit < GLOBAL.manager_player.lst_prefab_gnomes.Count; ++index_unit)
		{
			unit prefab_unit = GLOBAL.manager_player.lst_prefab_gnomes[index_unit];

			tog_unit_select tog_unit_select = Instantiate(prefab_tog_unit_select);
			tog_unit_select.SetPrefabUnit(prefab_unit);
			tog_unit_select.transform.SetParent(grp_gnomes.transform);

			Toggle tog = tog_unit_select.GetComponent<Toggle>();
			grp_gnomes.RegisterToggle(tog);
			tog.onValueChanged.AddListener(tog_gnome_changed);
		}
		for (int index_unit = 0; index_unit < GLOBAL.manager_player.lst_prefab_trolls.Count; ++index_unit)
		{
			unit prefab_unit = GLOBAL.manager_player.lst_prefab_trolls[index_unit];

			tog_unit_select tog_unit_select = Instantiate(prefab_tog_unit_select);
			tog_unit_select.SetPrefabUnit(prefab_unit);
			tog_unit_select.transform.SetParent(grp_trolls.transform);

			Toggle tog = tog_unit_select.GetComponent<Toggle>();
			grp_trolls.RegisterToggle(tog);
			tog.onValueChanged.AddListener(tog_troll_changed);
		}
	}

	public void DeselectAll()
	{
		grp_gnomes.SetAllTogglesOff();
		grp_trolls.SetAllTogglesOff();
	}

	public void tog_gnome_changed(bool value)
	{
		unit prefab_unit = null;

		Toggle tog_active = grp_gnomes.ActiveToggles().FirstOrDefault();

		if (tog_active != null)
		{
			prefab_unit = tog_active.GetComponent<tog_unit_select>().prefab_unit;
		}
		
		GLOBAL.manager_player.SelectPrefabUnit(prefab_unit);

		// Debug.Log(string.Format
		// (
		// 	"tog_gnome_changed() - prefab_unit: {0}", 
		// 	prefab_unit == null ? "[none]" : prefab_unit.name
		// ));
	}

	public void tog_troll_changed(bool value)
	{
		unit prefab_unit = null;

		Toggle tog_active = grp_trolls.ActiveToggles().FirstOrDefault();

		if (tog_active != null)
		{
			prefab_unit = tog_active.GetComponent<tog_unit_select>().prefab_unit;
		}
		
		GLOBAL.manager_player.SelectPrefabUnit(prefab_unit);

		// Debug.Log(string.Format
		// (
		// 	"tog_troll_changed() - prefab_unit: {0}", 
		// 	prefab_unit == null ? "[none]" : prefab_unit.name
		// ));
	}
}
