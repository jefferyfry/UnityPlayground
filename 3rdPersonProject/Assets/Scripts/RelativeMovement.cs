using UnityEngine;
using System.Collections;

/**
 * Rotates the player object to camera-relative based on arrow key input.
 */
[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform orbitCamera; //the camera object

	public float rotSpeed = 15.0f;
	public float moveSpeed = 6.0f;

	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -10.0f;
	public float minFall = -1.5f;

	private float vertSpeed;

	private CharacterController characterController; //character controller provides movement input and collisioning handling

	private ControllerColliderHit contact;

	private Animator animator;

	void Start(){
		vertSpeed = minFall;
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() {

		// start with zero and add movement components progressively
		Vector3 movement = Vector3.zero;

		// x z movement transformed relative to target
		float horInput = Input.GetAxis("Horizontal");
		float vertInput = Input.GetAxis("Vertical");
		if (horInput != 0 || vertInput != 0) {
			movement.x = horInput * moveSpeed;
			movement.z = vertInput * moveSpeed;
			movement = Vector3.ClampMagnitude(movement, moveSpeed);

			Quaternion tmp = orbitCamera.rotation;
			orbitCamera.eulerAngles = new Vector3(0, orbitCamera.eulerAngles.y, 0);
			movement = orbitCamera.TransformDirection(movement);
			orbitCamera.rotation = tmp;

			// face movement direction
			//transform.rotation = Quaternion.LookRotation(movement);
			Quaternion direction = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation,
			                                     direction, rotSpeed * Time.deltaTime);
		}

		animator.SetFloat("Speed", movement.sqrMagnitude);

		//jumping
		//raycasting to check if on ground
		bool hitGround = false;
		RaycastHit hit;
		if(vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
			float check = (characterController.height + characterController.radius) / 1.9f;
			hitGround = hit.distance <= check;
		}

		if(hitGround) { //changed from characterController.isGrounded to use the raycasting results
			if(Input.GetButtonDown("Jump"))
				vertSpeed = jumpSpeed;
			else {
				vertSpeed = -0.1f;
				animator.SetBool("Jumping", false);
			}
		} else {
			vertSpeed += gravity * 5 * Time.deltaTime;
			if(vertSpeed < terminalVelocity)
				vertSpeed = terminalVelocity;
			if(contact!=null)
				animator.SetBool("Jumping", true);
			if(characterController.isGrounded) {
				if(Vector3.Dot(movement, contact.normal) < 0)
					movement = contact.normal * moveSpeed;
				else
					movement += contact.normal * moveSpeed;
			}
		}
		movement.y = vertSpeed;

		movement *= Time.deltaTime;
		characterController.Move(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		contact = hit;
	}
}
