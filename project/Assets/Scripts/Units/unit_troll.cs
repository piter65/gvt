using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class unit_troll : unit
{
	public float speed = 0.5f;
	public float idle_delay = 0.5f;
	public bool attacking = false;
	public float attack_reach = 1.0f;

	public float target_distance;

	protected float current_speed = 0.0f;
	protected Animator _animator;

	protected override void Start()
	{
		base.Start();

		_animator = GetComponent<Animator>();

		_animator.SetInteger("health", health);
	}
	
	protected override void Update()
	{
		base.Update();

		if (active)
		{
			if (attacking)
			{
				current_speed = 0.0f;
			}
			else
			{
				current_speed = Mathf.MoveTowards
				(
					current_speed, speed,
					speed * (1.0f / idle_delay) * Time.deltaTime
				);
			}

			_animator.SetFloat("speed", current_speed);
		}
		else
		{
			current_speed = 0.0f;
			_animator.SetFloat("speed", current_speed);
		}
	}

	public virtual void anim_event_attack_start()
	{
		
	}

	public virtual void anim_event_attack_live()
	{
		
	}

	public virtual void anim_event_attack_dead()
	{
		if (TargetValid())
		{
			Debug.Log("Damaging target");

			target.RecieveDamage(damage);
		}
	}

	public virtual void anim_event_attack_end()
	{
		attacking = false;
	}

	public virtual void Attack()
	{
		attacking = true;

		_animator.SetTrigger("attack");
	}

	public virtual bool TargetValid()
	{
		if (target != null)
		{
			target_distance = this.transform.position.x - target.transform.position.x;

			// return
			// 	   target.cell == this.cell
			// 	|| (   target_distance >= 0
			// 		&& target_distance <= attack_reach);
			return (   target_distance >= 0
					&& target_distance <= attack_reach);
		}

		return false;
	}

	public override void RecieveDamage(int damage)
	{
		base.RecieveDamage(damage);

		_animator.SetInteger("health", health);
	}
}
