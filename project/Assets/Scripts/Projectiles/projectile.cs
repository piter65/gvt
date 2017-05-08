using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class projectile : MonoBehaviour
{
	public bool active = false;
	public float speed = 5.0f;
	public int damage = 3;
	public float impact_flux_speed = 0.0f;
	public float impact_flux_duration = 0.5f;
	public element element;
	public Vector3 pos_source = Vector3.zero;
	public unit target = null;

	protected virtual void Start()
	{
		
	}
	
	protected virtual void Update()
	{
		
	}
}
