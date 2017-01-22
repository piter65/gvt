using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gnome_crossbowman : unit_gnome
{

	public Transform bolt_slot;
	public projectile prefab_bolt;

	private projectile _bolt;

	protected override void Start()
	{
		base.Start();

		LoadBolt();
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

	public override void Attack()
	{
		base.Attack();

		_animator.SetTrigger("attack");
	}
}
