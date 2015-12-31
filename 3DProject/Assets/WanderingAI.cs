using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour {

	[SerializeField] private GameObject projectilePrefab;
	private GameObject projectile;

	private bool alive=true;

	public const float baseSpeed = 3.0f;
	public float speed = 3.0f;
	public float obstacleRange = 5.0f;

	void Awake(){
		Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	// Update is called once per frame
	void Update () {
		if(alive) {
			transform.Translate(0, 0, speed * Time.deltaTime); //moves forward every frame, local coordinates, Z is forward

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if(Physics.SphereCast(ray, 0.75f, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				if(hitObject.GetComponent<Player>()) {
					//instantiate projectiel and copy some settings from the robot
					projectile = Instantiate(projectilePrefab) as GameObject;
					projectile.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
					projectile.transform.rotation = transform.rotation;
				}
				else if(hit.distance < obstacleRange) {
					float angle = Random.Range(-110, 110);
					transform.Rotate(0,angle,0);
				}
			}
		}
	}

	public void SetAlive(bool alive){
		this.alive = alive;
	}

	private void OnSpeedChanged(float value){
		speed = baseSpeed * value;
	}
}
