using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;
	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;

	private float rotationX = 0;

	public enum RotationAxes {
		MouseXAndY = 0,
		MouseX = 1,
		MouseY =2
	}

	public RotationAxes axes = RotationAxes.MouseXAndY;
	
	// Update is called once per frame
	void Update () {
		if (axes == RotationAxes.MouseX) {
			transform.Rotate(0,Input.GetAxis("Mouse X")*sensitivityHor,0);
		} else if (axes == RotationAxes.MouseY) {
			rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
			rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
			float rotationY = transform.localEulerAngles.y;
			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		} else {
			rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
			rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
			float delta = Input.GetAxis ("Mouse X") * sensitivityHor;
			float rotationY = transform.localEulerAngles.y + delta;
			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		}
	}
}
