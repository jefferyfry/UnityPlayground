using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	public const float baseSpeed = 3.0f;
	public float speed = 3.0f;

	[SerializeField] private GameObject enemyPrefab; //SerializedField makes variable visbible in editor, but read only with private
	private GameObject enemy;

	void Awake(){
		Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}
	
	// Update is called once per frame
	void Update () {
		if(enemy == null) {
			enemy = Instantiate(enemyPrefab) as GameObject;
			enemy.transform.position = new Vector3(0, 0, 0);
			float angle = Random.Range(0, 360);
			enemy.transform.Rotate(0, angle, 0);
			enemy.GetComponent<WanderingAI>().speed = speed;
		}
	}

	private void OnSpeedChanged(float value){
		speed = baseSpeed * value;
	}
}
