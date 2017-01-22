using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class structure_water_well : unit_gnome
{
	private bool constructed = false;

	protected override void Update()
	{
		base.Update();

		if (active)
		{
			// Trigger the construction animation.
			if (!constructed)
			{
				_animator.SetTrigger("construct");
				constructed = true;
			}
		}
	}
}
