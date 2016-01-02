using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

	[SerializeField] private Transform target; //this will be the payer gameobject.transform assigned via editor slot

	public float rotSpeed = 1.5f;

	private float rotY;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		rotY = transform.eulerAngles.y; //referring to the camera transform
		offset = target.position - transform.position; //setting the offset = gameobject.position - camera.position
	}

	//called after update
	void LateUpdate () {
		float horInput = Input.GetAxis ("Horizontal");
		if (horInput != 0)
			rotY += horInput * rotSpeed; //keyboard control
		else
			rotY += Input.GetAxis ("Mouse X") * rotSpeed * 3; //mouse control

		Quaternion rotation = Quaternion.Euler(0, rotY, 0); //rotation conversion
		transform.position = target.position - (rotation * offset);
		transform.LookAt (target); //points camera at the target
	}
}
