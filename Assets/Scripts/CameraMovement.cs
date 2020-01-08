using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour {

	Vector3 localRotation;
	bool cameraDisable = false;
	
	void LateUpdate () 
	{
		if (Input.GetMouseButton (1)) 
		{
			if (!cameraDisable)
			{
				localRotation.x += Input.GetAxis ("Mouse X") * 3;
				localRotation.y += Input.GetAxis ("Mouse Y") * -3;
				localRotation.y = Mathf.Clamp(localRotation.y, -20, 20);
			}
		}

		Quaternion qt = Quaternion.Euler (localRotation.y, localRotation.x, 0);
		transform.parent.rotation = Quaternion.Lerp (transform.parent.rotation, qt, Time.deltaTime * 5);
	}
}
