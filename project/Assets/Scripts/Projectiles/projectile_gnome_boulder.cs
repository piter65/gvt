using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class projectile_gnome_boulder : projectile
{
	public AnimationCurve curve_height;

	private float _time_active = 0.0f;
	private Vector3 _pos_target = Vector3.zero;

	protected override void Start()
	{
		base.Start();
	}
	
	protected override void Update()
	{
		base.Update();

		if (active)
		{
			_time_active += Time.deltaTime;

			if (target != null)
			{
				_pos_target = target.transform.position;
			}

			Vector3 offset_to_target = _pos_target - pos_source;
			float distance_target = offset_to_target.magnitude;

			float interp = _time_active * speed / distance_target;
			interp = Mathf.Clamp(interp, 0.0f, 1.0f);

			transform.position = 
				  pos_source
				+ offset_to_target * interp
				+ new Vector3(0.0f, curve_height.Evaluate(interp), 0.0f);

			if (interp == 1.0f)
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

				// The projectile is no longer active.
				active = false;
			}
		}
	}
}
