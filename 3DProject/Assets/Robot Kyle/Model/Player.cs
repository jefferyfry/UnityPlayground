using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private int health=5;
	
	// Update is called once per frame
	public void Hurt(int damage){
		health -= damage;
		Debug.Log("Health: " + health);
	}
}
