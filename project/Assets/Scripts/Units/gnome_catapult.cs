using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gnome_catapult : unit_gnome
{
	public AudioClip audio_reload;

	public Transform projectile_slot;
	public projectile prefab_projectile;
	public float attack_speed = 1.0f;

	private projectile _projectile;

	protected override void Start()
	{
		base.Start();

		LoadProjectile();

		_animator.speed = 1.0f;
	}

	private void LoadProjectile()
	{
		if (projectile_slot != null)
		{
			_projectile = Instantiate(prefab_projectile);
			_projectile.transform.SetParent(projectile_slot);
			_projectile.transform.localPosition = Vector3.zero;
			_projectile.transform.localRotation = Quaternion.identity;
			_projectile.damage = damage;
			_projectile.element = element;
		}
	}

	public virtual void anim_event_attack_start()
	{
		_animator.speed = attack_speed;
	}

	public virtual void anim_event_attack_release()
	{
		if (_projectile != null)
		{
			_projectile.transform.SetParent(null);
			_projectile.pos_source = _projectile.transform.position;
			_projectile.target = target;
			_projectile.active = true;
		}
	}

	public virtual void anim_event_attack_reload()
	{
		LoadProjectile();

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
