﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_field : MonoBehaviour
{
	public game_cell prefab_game_cell;

	public int width = 9;
	public int height = 5;
	public int cell_count;

	public game_cell[] _arr_field_cells;

	void Start()
	{
		GLOBAL.game_field = this;

		for (int index_cell = transform.childCount - 1; index_cell > -1; --index_cell)
		{
			Destroy(transform.GetChild(index_cell).gameObject);
		}

		cell_count = width * height;

		_arr_field_cells = new game_cell[cell_count];

		for (int index_width = 0; index_width < width; ++index_width)
		{
			for (int index_height = 0; index_height < height; ++index_height)
			{
				int index = (index_width * height) + index_height;

				game_cell cell = Instantiate(prefab_game_cell);
				cell.cell_index = index;

				cell.transform.parent = transform;
				cell.transform.position = 
					new Vector3(index_width, 0.0f, index_height);
				_arr_field_cells[index] = cell;
			}
		}
	}

	void OnDestroy()
	{
		if (GLOBAL.game_field == this)
			GLOBAL.game_field = null;
	}
	
	void Update()
	{
		
	}

	public game_cell GetCellAtPosition(Vector3 pos)
	{
		int x = (int)Mathf.Round(pos.x);
		int z = (int)Mathf.Round(pos.z);

		if (   x >= 0
			&& x < width
			&& z >= 0
			&& z < height)
		{
			int index_cell = x * height + z;
			return _arr_field_cells[index_cell];
		}
		else
		{
			return null;
		}
	}

	public game_cell GetCellLeft(int cell_index)
	{
		if (cell_index <= height)
			return null;

		cell_index -= height;
		return _arr_field_cells[cell_index];
	}

	public game_cell GetCellRight(int cell_index)
	{
		if (cell_index >= cell_count - height)
			return null;

		cell_index += height;
		return _arr_field_cells[cell_index];
	}

	public game_cell GetCellDown(int cell_index)
	{
		if (cell_index % height == 0)
			return null;

		cell_index -= 1;
		return _arr_field_cells[cell_index];
	}

	public game_cell GetCellUp(int cell_index)
	{
		if (cell_index % height == height - 1)
			return null;

		cell_index += 1;
		return _arr_field_cells[cell_index];
	}

	public void UpdateUnitCell(unit unit)
	{
		if (unit.cell == null)
		{
			game_cell cell = GetCellAtPosition(unit.transform.position);

			if (cell != null)
			{
				cell.AddUnit(unit);
			}
			else
			{
				Debug.Log(string.Format
				(
					"Unit '{0}' out of bounds! - pos: {1}",
					unit.name,
					unit.transform.position.ToString()
				));
			}
		}
		else if (!unit.cell.GetComponent<Collider>().bounds.Contains(unit.transform.position))
		{
			unit.cell.RemoveUnit(unit);

			game_cell cell = GetCellAtPosition(unit.transform.position);

			if (cell != null)
			{
				cell.AddUnit(unit);
			}
			else
			{
				Debug.Log(string.Format
				(
					"Unit '{0}' out of bounds! - pos: {1}",
					unit.name,
					unit.transform.position.ToString()
				));
			}
		}
		else
		{
			// No change needed.
		}
	}
}