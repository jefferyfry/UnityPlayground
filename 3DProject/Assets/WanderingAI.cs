using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour {

	private bool alive=true;

	public float speed = 3.0f;
	public float obstacleRange = 5.0f;
	
	// Update is called once per frame
	void Update () {
		if(alive) {
			transform.Translate(0, 0, speed * Time.deltaTime); //moves forward every frame, local coordinates, Z is forward

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if(Physics.SphereCast(ray, 0.75f, out hit)) {
				if(hit.distance < obstacleRange) {
					float angle = Random.Range(-110, 110);
					transform.Rotate(0,angle,0);
				}
			}
		}
	}

	public void SetAlive(bool alive){
		this.alive = alive;
	}
}
