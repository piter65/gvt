using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tog_unit_select : MonoBehaviour
{
	public Text txt_name;
	public Text txt_cost;

	public unit prefab_unit;

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

	public void SetPrefabUnit(unit prefab_unit)
	{
		this.prefab_unit = prefab_unit;
		txt_name.text = prefab_unit.select_name;
		txt_cost.text = "$" + prefab_unit.cost.ToString();
	}
}
