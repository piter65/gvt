using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gnome_crossbowman : unit_gnome
{
	public AudioClip audio_reload;

	public Transform bolt_slot;
	public projectile prefab_bolt;
	public float attack_speed = 1.0f;

	private projectile _bolt;

	protected override void Start()
	{
		base.Start();

		LoadBolt();

		_animator.speed = 1.0f;
	}

	private void LoadBolt()
	{
		if (bolt_slot != null)
		{
			_bolt = Instantiate(prefab_bolt);
			_bolt.transform.SetParent(bolt_slot);
			_bolt.transform.localPosition = Vector3.zero;
			_bolt.transform.localRotation = Quaternion.identity;
			_bolt.damage = damage;
			_bolt.element = element;
		}
	}

	public virtual void anim_event_attack_start()
	{
		_animator.speed = attack_speed;
	}

	public virtual void anim_event_attack_release()
	{
		if (_bolt != null)
		{
			_bolt.transform.SetParent(null);
			_bolt.active = true;
		}
	}

	public virtual void anim_event_attack_reload()
	{
		LoadBolt();

		_audio_source.clip = audio_reload;
		_audio_source.Play();
	}

	public virtual void anim_event_attack_end()
	{
		attacking = false;

		_animator.speed = 1.0f;
	}

	public override void Attack()
	{
		base.Attack();

		_animator.SetTrigger("attack");
	}
}
