using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ai_troll : ai
{
	protected unit_troll _troll;

	protected override void Start()
	{
		base.Start();

		_troll = GetComponent<unit_troll>();
	}
	
	protected override void Update()
	{
		base.Update();

		if (_troll.active)
		{
			if (!_troll.attacking)
				ScanForTargets();
		}
	}

	protected override void ScanForTargets()
	{
		game_cell cell = _troll.cell;

		_troll.target = null;

		// Debug.Log("== Scanning for targets ==");

		while (cell != null)
		{
			// Debug.Log("Scan cell - pos: " + cell.transform.position);

			if (cell.lst_unit_gnomes.Count > 0)
			{
				_troll.target = cell.lst_unit_gnomes[0];
				break;
			}

			// Check the next cell
			cell = cell.Left;
		}

		// Attack the target if it's a valid target
		if (_troll.TargetValid())
		{
			_troll.Attack();
		}
	}
}
