using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ai_gnome_ranged : ai
{
	protected unit_gnome _gnome;

	protected override void Start()
	{
		base.Start();

		_gnome = GetComponent<unit_gnome>();
	}
	
	protected override void Update()
	{
		base.Update();

		if (_gnome.active)
		{
			if (!_gnome.attacking)
				ScanForTargets();
		}
	}

	protected override void ScanForTargets()
	{
		game_cell cell = _gnome.cell;

		_gnome.target = null;

		// Debug.Log("== Scanning for targets ==");

		while (cell != null)
		{
			// Debug.Log("Scan cell - pos: " + cell.transform.position);

			if (cell.lst_unit_trolls.Count > 0)
			{
				_gnome.target = cell.lst_unit_trolls[0];
				break;
			}

			// Check the next cell
			cell = cell.Right;
		}

		if (_gnome.target != null)
			Debug.Log("Gnome found target");

		// Attack the target if it's a valid target
		if (_gnome.TargetValid())
		{
			_gnome.Attack();
		}
	}
}
