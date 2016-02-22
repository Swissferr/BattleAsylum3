using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;

	public float smoothDampTime = 0.25f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;

	private Vector3 _smoothDampVelocity;

	void Awake()
	{
		transform = gameObject.transform;
	}
	
	void LateUpdate()
	{
		UpdateCameraPosition ();
	}

	void UpdateCameraPosition()
	{
		transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
		return;
	}
	
}
