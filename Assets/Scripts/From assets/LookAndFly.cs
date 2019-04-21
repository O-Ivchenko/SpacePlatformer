using UnityEngine;
using System.Collections;

public class LookAndFly : MonoBehaviour {

	Transform _transform;

	public float sensitivityX = 15f;
	public float sensitivityY = 15f;

	public float minY = -60f;
	public float maxY = 60f;
	
	float rotationX = 0f;
	float rotationY = 0f;

	void Awake() {

		_transform = GetComponent<Transform>();
	}

	void Update() {	

		rotationX = _transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp(rotationY, minY, maxY);
		
		_transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);

		_transform.Translate(Input.GetAxis("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? 20f : 1f), 0f, Input.GetAxis("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? 20f : 1f)); 
	}
}