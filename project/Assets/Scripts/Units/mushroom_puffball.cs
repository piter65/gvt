using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mushroom_puffball : unit_mushroom
{
	public Transform prefab_puffball;
	public Transform puffball_spawn;
	public float release_delay = 5.0f;
	public float time_since_release = 0.0f;

	private Animator _animator;
	private bool _releasing = false;

	protected override void Start()
	{
		base.Start();

		_animator = GetComponent<Animator>();
	}
	
	protected override void Update()
	{
		base.Update();

		if (active)
		{
			if (!_releasing)
			{
				time_since_release += Time.deltaTime;

				if (time_since_release >= release_delay)
				{
					time_since_release -= release_delay;
					_animator.SetTrigger("release");
					_releasing = true;
				}
			}
		}
	}

	public void anim_event_release_peak()
	{
		Transform puffball = Instantiate(prefab_puffball);
		puffball.position = puffball_spawn.position;
	}

	public void anim_event_release_end()
	{
		_releasing = false;
	}

	public override void RecieveDamage(int damage)
	{
		base.RecieveDamage(damage);

		if (dead)
			_animator.SetTrigger("die");
	}
}
