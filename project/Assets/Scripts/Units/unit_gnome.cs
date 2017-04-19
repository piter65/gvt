using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class unit_gnome : unit
{
	public bool attacking = false;

	protected Animator _animator;

	protected override void Start()
	{
		base.Start();

		_animator = GetComponent<Animator>();

		_animator.SetInteger("health", health);
	}

	protected override void OnDrawGizmos()
	{
		// Draw line to current cell
		if (cell != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine
			(
				transform.position + GLOBAL.nudge_up,
				cell.transform.position
			);
		}

		// Draw a line to current target
		if (target != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine
			(
				transform.position + GLOBAL.nudge_up * 2.0f,
				target.transform.position + GLOBAL.nudge_up * 2.0f
			);
		}
	}

	public virtual void Attack()
	{
		attacking = true;
	}

	public virtual bool TargetValid()
	{
		if (target != null)
		{
			return true;
		}

		return false;
	}

	public override void RecieveDamage(int damage, element elem)
	{
		base.RecieveDamage(damage, elem);

		_animator.SetInteger("health", health);
	}
}
