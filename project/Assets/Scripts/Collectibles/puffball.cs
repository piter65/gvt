using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class puffball : collectible
{
	public int spore_value = 25;
	public float final_height_offset = 2.0f;
	public float rise_duration = 3.0f;
	public AnimationCurve height_over_time;
	public float drift_speed = 0.25f;
	public float rise_time = 0.0f;

	private float _base_height;

	void Start()
	{
		_base_height = transform.position.y;
		rise_time = 0.0f;
	}
	
	void Update()
	{
		rise_time += Time.deltaTime;

		float drift_angle = UnityEngine.Random.Range(0.0f, 360.0f);
		float height_offset = height_over_time.Evaluate(Mathf.Clamp(rise_time / rise_duration, 0.0f, 1.0f)) * final_height_offset;
		
		Vector3 new_pos = transform.position;
		new_pos.y = _base_height + height_offset;

		Vector3 drift_direction = Quaternion.AngleAxis(drift_angle, Vector3.up) * Vector3.forward;
		if (drift_direction.x < 0.0f)
			drift_direction.x *= 0.5f;
		Vector3 drift_offset = drift_direction * (drift_speed * Time.deltaTime);
		new_pos += drift_offset;
		
		transform.position = new_pos;
	}

	public override void Collect()
	{
		GLOBAL.manager_player.spore_total += spore_value;

		Debug.Log(name + " collected.");
		Destroy(gameObject);
	}
}
