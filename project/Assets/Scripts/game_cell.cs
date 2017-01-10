using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_cell : MonoBehaviour
{
	public int cell_index = -1;
	public unit_mushroom unit_mushroom = null;
	public unit unit_gnome = null;
	public List<unit_troll> lst_unit_trolls = new List<unit_troll>();

	public game_cell Left
	{
		get { return GLOBAL.game_field.GetCellLeft(cell_index); }
	}
	public game_cell Right
	{
		get { return GLOBAL.game_field.GetCellRight(cell_index); }
	}
	public game_cell Down
	{
		get { return GLOBAL.game_field.GetCellDown(cell_index); }
	}
	public game_cell Up
	{
		get { return GLOBAL.game_field.GetCellUp(cell_index); }
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

	public void AddUnit(unit new_unit)
	{
		new_unit.cell = this;

		if (new_unit is unit_mushroom)
		{
			unit_mushroom = new_unit as unit_mushroom;
		}
		else if (new_unit is unit_gnome)
		{
			unit_gnome = new_unit as unit_gnome;
		}
		else if (new_unit is unit_troll)
		{
			lst_unit_trolls.Add(new_unit as unit_troll);
		}
	}

	public void RemoveUnit(unit old_unit)
	{
		if (old_unit.cell == this)
			old_unit.cell = null;

		if (unit_mushroom == old_unit)
		{
			unit_mushroom = null;
		}
		if (unit_gnome == old_unit)
		{
			unit_gnome = null;
		}
		else if (lst_unit_trolls.Contains(old_unit as unit_troll))
		{
			lst_unit_trolls.Remove(old_unit as unit_troll);
		}
	}

	public void Highlight()
	{

	}
}
