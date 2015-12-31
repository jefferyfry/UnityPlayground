using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))] //enforces requirement
[AddComponentMenu("Control Script/FPS Input")] //adds script for quick add

public class FPSInput : MonoBehaviour {

	public float gravity = -9.8f;
	public float speed = 6.0f;
	private CharacterController characterController;

	public const float baseSpeed = 3.0f;

	void Awake(){
		Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = Input.GetAxis ("Horizontal") * speed;
		float deltaZ = Input.GetAxis ("Vertical") * speed;
		Vector3 movement = new Vector3 (deltaX, 0, deltaZ); //Vector3 is 3D vector
		movement = Vector3.ClampMagnitude (movement, speed);  //limit movement speed
		movement.y = gravity;
		movement *= Time.deltaTime; //compensates for frame speed
		movement = transform.TransformDirection (movement); //transforms the vector from local coordinate space (character) to global space
		characterController.Move (movement); //instead of using transform.Translate use CharacterController so that we can get collision detection
	}

	private void OnSpeedChanged(float value){
		speed = baseSpeed * value;
	}
}
