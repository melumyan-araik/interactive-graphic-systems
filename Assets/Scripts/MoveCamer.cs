using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamer : MonoBehaviour {
	
	public float sensitiveX = 3f;
	public float sensitiveY = 3f;


	public float minX = -360; 
	public float maxX = 360; 
	public float minY = -60;
	public float maxY = 60;

	private Quaternion originalRot;
	private float rotX = 0;
	private float rotY = 0;

	void Start () {

	}

	void FixedUpdate () {
		if (Input.GetMouseButton (0)) {
			Quaternion rotationY = Quaternion.AngleAxis (1, Vector3.up);
			transform.rotation *= rotationY;
		}
}

}