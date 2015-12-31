using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIController : MonoBehaviour {

	[SerializeField] private Text scoreLabel;
	[SerializeField] private SettingsPopup settingsPopup;
	private int score=0;

	void Awake(){
		Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
	}

	void OnDestroy() {
		Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
	}

	void Start(){
		score = 0;
		settingsPopup.Close();
	}
	
	// Update is called once per frame
	void Update () {
		scoreLabel.text = Time.realtimeSinceStartup.ToString();
		scoreLabel.text = score.ToString();
	}

	public void OnOpenSettings(){
		settingsPopup.Open();
	}

	public void OnPointerDown(){
		Debug.Log("Pointer Down");
	}

	private void OnEnemyHit(){
		score += 1;
		scoreLabel.text = score.ToString();
	}
}
