using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	[SerializeField] private GameObject enemyPrefab; //SerializedField makes variable visbible in editor, but read only with private
	private GameObject enemy;
	
	// Update is called once per frame
	void Update () {
		if(enemy == null) {
			enemy = Instantiate(enemyPrefab) as GameObject;
			enemy.transform.position = new Vector3(0, 1, 0);
			float angle = Random.Range(0, 360);
			enemy.transform.Rotate(0, angle, 0);
		}
	}
}
