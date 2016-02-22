using UnityEngine;
using System.Collections;

public class PlayerFall : MonoBehaviour {

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10f)
			transform.position = initialPosition;
	}
}
