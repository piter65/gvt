using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class projectile_gnome_bolt : projectile
{
	protected override void Start()
	{
		base.Start();
	}
	
	protected override void Update()
	{
		base.Update();

		if (active)
		{
			game_cell cell = GLOBAL.game_field.GetCellAtPosition(transform.position);
			if (cell != null)
			{
				float closest_distance = 100.0f;
				unit_troll closest_troll = null;

				// Loop thru the cell's list of trolls and find the closest one.
				for (int index_troll = 0; index_troll < cell.lst_unit_trolls.Count; ++index_troll)
				{
					unit_troll troll = cell.lst_unit_trolls[index_troll];
					float distance = troll.transform.position.x - transform.position.x;
					if (distance < closest_distance)
					{
						closest_distance = distance;
						closest_troll = troll;
					}
				}

				if (closest_troll != null)
				{
					// Damage the closest troll.
					closest_troll.RecieveDamage(damage, element);

					// Flux the troll.
					closest_troll.Flux(impact_flux_speed, impact_flux_duration);

					// Destroy the projectile after dealing damage.
					Destroy(gameObject);
					return;
				}
			}
			else
			{
				// Destroy the projectile if it's off the field.
				Destroy(gameObject);
				return;
			}

			// Move the projectile.
			transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
		}
	}
}
