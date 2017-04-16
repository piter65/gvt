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
	public int thickskin = 0;		// some trolls have thick skin...
	public float death_timeout = 2.0f;

	public bool active = false;
	public bool dead = false;
	public game_cell cell;
	public unit target = null;

	public AudioClip audio_death;

	protected float _dead_time = 0.0f;
	protected AudioSource _audio_source;

	protected virtual void Start()
	{
		_audio_source = GetComponent<AudioSource>();

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

				// Notify the cell that unit no longer occupies it.
				if (cell != null)
					cell.RemoveUnit(this);
			}
		}

		if (dead)
		{
			_dead_time += Time.deltaTime;

			if (_dead_time > death_timeout)
				Destroy(this.gameObject);
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
		int nowdamage;
		nowdamage = damage - thickskin;		/// peter was here....
		if (nowdamage<1) nowdamage=1;		// always some damage..


		health -= nowdamage;
		health = Mathf.Max(health, 0);
		
		if (health <= 0)   // peter changed from ==   - Just me being paranoid....
		{
			if (dead != true)
			{
				_audio_source.clip = audio_death;
				_audio_source.Play();
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
