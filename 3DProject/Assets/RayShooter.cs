using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RayShooter : MonoBehaviour {
	private Camera playerCamera;
	private GUIStyle myStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
		myStyle.fontSize = 48;
		playerCamera = GetComponent<Camera>();
		//Cursor.lockState = CursorLockMode.Locked; //lock cursor to the center of the game window
		//Cursor.visible = false; //hide the mouse cursor
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject()) { //check gui is not used
			Vector3 point = new Vector3(playerCamera.pixelWidth / 2, playerCamera.pixelHeight / 2, 0);
			Ray ray = playerCamera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {  //C# 'out' enforces pass by reference
				Debug.Log("Hit!");
				GameObject hitObject = hit.transform.gameObject; //checks if we hit a game object
				ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
				if(target != null) { //check to see if we hit our reactive target
					target.ReactToHit();
					Messenger.Broadcast(GameEvent.ENEMY_HIT);
				}
				else
					StartCoroutine(SphereIndicator(hit.point)); //basically starts a new thread for this method
			}
		}
	}

	//StartCoroutine requires IEnumerator return
	//http://twistedoakstudios.com/blog/Post83_coroutines-more-than-you-want-to-know
	IEnumerator SphereIndicator(Vector3 pos ){ //IEnumerator is similar interface to java for collections
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds(1); //pauses this thread and yields for 1 second

		Destroy(sphere);
	}

	// runs every frame after 3D scene is rendered
	void OnGUI() {
		int size = 48;
		float posX = playerCamera.pixelWidth / 2 - size / 4;
		float posY = playerCamera.pixelHeight / 2 - size / 2;
		GUI.Label(new Rect(posX, posY, size, size), "*",myStyle); //basic user interface API
	}
}
