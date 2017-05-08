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
	public float attack_speed = 1.0f;

	public float target_distance;

	protected float current_speed = 0.0f;
	protected Animator _animator;
	protected bool _fluxing = false;
	protected float _flux_speed = 1.0f;
	protected float _flux_time = 0.0f;
	protected float _flux_duration = 0.0f;

	protected override void Start()
	{
		base.Start();

		_animator = GetComponent<Animator>();

		_animator.SetInteger("health", health);

		_animator.speed = 1.0f;
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
					current_speed,
					speed,
					speed * (1.0f / idle_delay) * Time.deltaTime
				);
			}

			if (_fluxing)
			{
				_flux_time += Time.deltaTime;
				if (_flux_time <= _flux_duration)
				{
					_animator.SetFloat("flux_speed", _flux_speed);
				}
				else
				{
					_animator.SetFloat("flux_speed", 1.0f);
					_fluxing = false;
				}
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
		_animator.speed = attack_speed;
	}

	public virtual void anim_event_attack_live()
	{
		
	}

	public virtual void anim_event_attack_dead()
	{
		if (TargetValid())
		{
			Debug.Log("Damaging target");

			target.RecieveDamage(damage, element);
		}
	}

	public virtual void anim_event_attack_end()
	{
		attacking = false;
		_animator.speed = 1.0f;
	}

	public virtual void Attack()
	{
		if (!_fluxing)
		{
			attacking = true;

			_animator.SetTrigger("attack");
		}
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

	public override void RecieveDamage(int damage, element elem)
	{
		base.RecieveDamage(damage, elem);

		_animator.SetInteger("health", health);

		// Flux(-1.0f, 3.0f);
	}

	public void Flux(float speed, float duration = 1.0f)
	{
		if (!attacking)
		{
			_fluxing = true;
			_flux_speed = speed;
			_flux_duration = duration;
			_flux_time = 0.0f;
		}
	}
}
