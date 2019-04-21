using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LookAndFly))]

public class ToggleCameraMode : MonoBehaviour {

	LookAndFly _lookAndFly;
	Transform _transform;

	Vector3 fixedPosition;
	Vector3 fixedRotation;

	public GUIText infoText;
	public KeyCode toggleKey;
	
	void Awake() {

		_lookAndFly = GetComponent<LookAndFly>();
		_transform = GetComponent<Transform>();

		fixedPosition = _transform.position;
		fixedRotation = _transform.eulerAngles;

		infoText.text = "Press '" + toggleKey.ToString() + "' to toggle camera mode.";
	}

	void Update() {

		if(Input.GetKeyDown(toggleKey)) {

			if(_lookAndFly.enabled) {

				_lookAndFly.enabled = false;
				_transform.position = fixedPosition;
				_transform.eulerAngles = fixedRotation;
			}
			else {

				_lookAndFly.enabled = true;
			}
		}
	}
}
