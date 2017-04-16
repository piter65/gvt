using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class unit : MonoBehaviour
{
	public string select_name = "[Unamed]";
	public int health = 1;
	public int max_health = 1;
	public int damage = 1;
	public int cost = 25;
	public int deadtime = 0;		// peter was here.   Was gonna try to add timeout....

	public bool active = false;
	public bool dead = false;
	public game_cell cell;
	public unit target = null;

	protected virtual void Start()
	{
		health = max_health;
	}
	
	protected virtual void Update()
	{
		if (active)
		{
			if (!dead)
			{
				GLOBAL.game_field.UpdateUnitCell(this);
			}
			else
			{
				active = false;

// Brent - why this no workie?

				if (cell != null)
					cell.RemoveUnit(this);
			}
		}

		if (dead) // Peter was here....
		{
//		Debug.Log("DEAD!  time"+deadtime);

				if (++deadtime > 120)
					Destroy(this.gameObject);		// peter got this from a forum....
		}


	}

	protected virtual void OnDrawGizmos()
	{
		if (cell != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine
			(
				transform.position + GLOBAL.nudge_up,
				cell.transform.position
			);
		}

		if (target != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine
			(
				transform.position + GLOBAL.nudge_up,
				target.transform.position + GLOBAL.nudge_up
			);
		}
	}

	public virtual void RecieveDamage(int damage)
	{
		health -= damage;
		health = Mathf.Max(health, 0);
		
		if (health <= 0)   // peter changed from ==   - Just me being paranoid....
		{

			if (dead != true)
				{
					// Brent-  play death scream here....


				}


			dead = true;
		}

		Debug.Log(string.Format
		(
			"Unit '{0}' recieved damage - damage: {1}, health: {2}, dead: {3}",
			name,
			damage,
			health,
			dead
		));
	}
}
