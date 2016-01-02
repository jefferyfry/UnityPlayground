using UnityEngine;
using System.Collections;

/**
 * Rotates the player object to camera-relative based on arrow key input.
 */
public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform camera; //the camera object

	public float rotSpeed = 15.0f;
	
	// Update is called once per frame
	void Update() {

		// start with zero and add movement components progressively
		Vector3 movement = Vector3.zero;

		// x z movement transformed relative to target
		float horInput = Input.GetAxis("Horizontal");
		float vertInput = Input.GetAxis("Vertical");
		if (horInput != 0 || vertInput != 0) {
			movement.x = horInput;
			movement.z = vertInput;

			Quaternion tmp = camera.rotation;
			camera.eulerAngles = new Vector3(0, camera.eulerAngles.y, 0);
			movement = camera.TransformDirection(movement);
			camera.rotation = tmp;

			// face movement direction
			//transform.rotation = Quaternion.LookRotation(movement);
			Quaternion direction = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation,
			                                     direction, rotSpeed * Time.deltaTime);
		}
	}
}
