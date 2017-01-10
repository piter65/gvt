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

				if (cell != null)
					cell.RemoveUnit(this);
			}
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
		if (health <= 0)
		{
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
