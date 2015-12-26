using UnityEngine;
using System.Collections;

public class FPSInput : MonoBehaviour {

	public float speed = 6.0f;
	private CharacterController characterController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = Input.GetAxis ("Horizontal") * speed;
		float deltaZ = Input.GetAxis ("Vertical") * speed;
		Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude (movement, speed);
		movement *= Time.deltaTime; //compensates for frame speed
		movement = transform.TransformDirection (movement); //transforms the vector from local coordinate space (character) to global space
		characterController.Move (movement); //instead of using transform.Translate use CharacterController so that we can get collision detection
	}
}
