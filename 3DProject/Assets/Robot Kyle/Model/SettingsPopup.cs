using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour {

	[SerializeField] private Slider speedSlider;
	[SerializeField] private InputField nameInput;

	void Start(){
		speedSlider.value = PlayerPrefs.GetFloat("speed");
		nameInput.text = PlayerPrefs.GetString("name");
	}

	public void Open(){
		this.gameObject.SetActive(true);
	}
	
	public void Close(){
		this.gameObject.SetActive(false);
	}

	public void OnSubmitName(string name){
		PlayerPrefs.SetString("name", name);
	}

	public void OnSpeedValue(float speed){
		PlayerPrefs.SetFloat("speed", speed);
		Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
	}
}
