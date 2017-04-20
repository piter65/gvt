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
