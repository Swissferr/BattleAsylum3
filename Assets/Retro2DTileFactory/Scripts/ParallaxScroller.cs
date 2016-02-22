using UnityEngine;
using System.Collections;

// Use this to make inidividual game objects scroll faster or slower than foreground objects
public class ParallaxScroller : MonoBehaviour {

	/*
	 * Set scrollSpeed to control how fast the object moves with respect to the camera.
	 * 0f - object doesn't move which is the same as not using this script
	 * 1f - Tracks camera
	 * 0.05 - Slowly follows camera
	 * -0.05 - Slowly moves away from camera
	 */
	public float backgroundScrollSpeed = 0f;
												
	private Vector2 startingPosition;
	private float startingCameraXPosition;
	
	// Use this for initialization
	void Start () {
		startingPosition = gameObject.transform.position;
		startingCameraXPosition = Camera.main.transform.position.x;
		
		Debug.Log ("camera start positions: " + startingCameraXPosition);
	}
	
	// Update is called once per frame
	void Update () {

		float changeX = Camera.main.transform.position.x - startingCameraXPosition;
		float newX = startingPosition.x + changeX * backgroundScrollSpeed;
		
		gameObject.transform.position = new Vector3 (newX, transform.position.y, gameObject.transform.position.z);
		
	}
}

