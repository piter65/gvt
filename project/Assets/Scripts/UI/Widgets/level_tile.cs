using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// [ExecuteInEditMode]
public class level_tile : Selectable, ISubmitHandler
{
	public struct level_data
	{
		public static readonly level_data init = new level_data()
		{
			name = "",
			path_data = "",
			medal = ""
		};

		public string name;
		public string path_data;
		public string medal;
	}
	public level_data data = level_data.init;

	public TMP_Text txt_name;
	public GameObject medal_none;
	public GameObject medal_gold;
	public GameObject medal_silver;
	public GameObject medal_bronze;

	[Range(0.0f, 1.0f)]
	public float fill = 0.0f;
	public float fill_speed = 1.0f;
	public float target_interp = 0.0f;
	public AnimationCurve curve_fill;

	private Animator _animator;
	private float _interp = 0.0f;
	private bool _submitting = false;

	private BaseEventData _event_data;

	void Start()
	{
		bool blah = data.medal.Equals("none");
		txt_name.text = data.name;
		medal_none.SetActive(blah);
		medal_gold.SetActive(data.medal.Equals("gold"));
		medal_silver.SetActive(data.medal.Equals("silver"));
		medal_bronze.SetActive(data.medal.Equals("bronze"));

		_animator = GetComponent<Animator>();

		fill = curve_fill.Evaluate(_interp);
		_animator.SetFloat("Fill", fill);
	}
	
	void Update()
	{
		// Handle whether or not we're submitting.
		if (_submitting)
		{
			// If we're still selected...
			if (IsHighlighted(_event_data))
			{
				// We're still submitting if 'Submit' is still triggered.
				_submitting = Input.GetAxis("Submit") != 0.0f;

				if (_submitting)
					DoStateTransition(SelectionState.Pressed, true);
				else
					DoStateTransition(SelectionState.Highlighted, true);
			}
			else
			{
				_submitting = false;

				DoStateTransition(SelectionState.Normal, true);
			}
		}

		// Handle if we need to change the fill amount.
		if (_interp != target_interp)
		{
			_interp = Mathf.MoveTowards(_interp, target_interp, Time.deltaTime * fill_speed);
			fill = curve_fill.Evaluate(_interp);

			_animator.SetFloat("Fill", fill);

			if (fill >= 1.0f)
				OnFill();
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		_submitting = true;
		// _animator.SetTrigger("Pressed");
		DoStateTransition(SelectionState.Pressed, true);
	}

	private void OnFill()
	{
		GLOBAL.level_select.LoadLevel(this);
	}
};