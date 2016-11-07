using UnityEngine;
using System.Collections;

public class ClickTile : MonoBehaviour {

	GameObject gameStateObject;
	GameState state;

	void Start() {
		gameStateObject = GameObject.Find("GameStateObject");
		state = gameStateObject.GetComponent<GameState>();
	}

	// Use this for initialization
	void onMouseDown() {
		Debug.Log ("Boop boop boop boop");
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit rayHit = new RaycastHit();;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out rayHit) &&  !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
				int x = (int)(rayHit.point.x / 32);
				int y = (int)((rayHit.point.y * -1) / 32);
				Debug.Log ("X: " + x + ", Y: "+ y);
				int newState = 0;
				if (state.GetPointInGrid(x, y) == 0) {
					newState = 1;
				}
				state.UpdateGrid(x, y, newState);
				Debug.Log ("Ray hit");
			}
		}
			

	}
}
