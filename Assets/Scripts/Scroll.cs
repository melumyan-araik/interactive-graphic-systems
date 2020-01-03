using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scroll : MonoBehaviour {

	void Update() 
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && GetComponent<Camera> ().fieldOfView > 40) 
		{
			GetComponent<Camera> ().fieldOfView--;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && GetComponent<Camera> ().fieldOfView < 50) 
		{
			GetComponent<Camera> ().fieldOfView++;
		}
	}
}
