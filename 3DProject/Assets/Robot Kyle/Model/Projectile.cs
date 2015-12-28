using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float speed=10.0f;
	public int damage=1;

	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, 0, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		Player player = other.GetComponent<Player>();
		if(player != null) {
			player.Hurt(damage);
		}
		Destroy(this.gameObject);
	}
}
