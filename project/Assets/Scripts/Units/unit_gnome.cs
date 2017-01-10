using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class unit_gnome : unit
{
	public bool attacking = false;

	public Transform bolt_slot;
	public projectile prefab_bolt;

	private projectile _bolt;

	protected Animator _animator;

	protected override void Start()
	{
		base.Start();

		_animator = GetComponent<Animator>();

		LoadBolt();
	}

	protected override void Update()
	{
		base.Update();

		if (active)
		{

		}
	}

	protected override void OnDrawGizmos()
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
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine
			(
				transform.position + GLOBAL.nudge_up * 2.0f,
				target.transform.position + GLOBAL.nudge_up * 2.0f
			);
		}
	}

	private void LoadBolt()
	{
		_bolt = Instantiate(prefab_bolt);
		_bolt.transform.SetParent(bolt_slot);
		_bolt.transform.localPosition = Vector3.zero;
		_bolt.transform.localRotation = Quaternion.identity;
	}

	public virtual void anim_event_attack_start()
	{
		
	}

	public virtual void anim_event_attack_release()
	{
		_bolt.transform.SetParent(null);
		_bolt.active = true;
	}

	public virtual void anim_event_attack_reload()
	{
		LoadBolt();
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
			return true;
		}

		return false;
	}

	public override void RecieveDamage(int damage)
	{
		base.RecieveDamage(damage);

		if (dead)
			_animator.SetTrigger("die");
	}
}
