using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {

	Vector3 distance;
	float posX;
	float posY;
	float posZ;

	void OnMouseDown()
	{
		distance = Camera.main.WorldToScreenPoint(transform.position);
		posX = Input.mousePosition.x - distance.x;
		posY = Input.mousePosition.y - distance.y;
		posZ = Input.mousePosition.z - distance.z;

	}

	void OnMouseDrag()
	{
		Vector3 mousePosition = new Vector3 (/*Input.mousePosition.x - posX*/distance.x, /*Input.mousePosition.y - posY*/distance.y, distance.z-1);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);

		transform.position = objPosition;
	}
}
