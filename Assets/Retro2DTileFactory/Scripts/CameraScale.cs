using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class VirtualResolution {

	public int width = 400;
	public int height = 240;	
}

public class CameraScale : MonoBehaviour {

	public VirtualResolution virtualResolution;

	// used to detect screen size changes
	private int width;
	private int height;

	//http://answers.unity3d.com/questions/32229/how-do-i-set-the-aspect-ratio-of-the-viewport.html
	//http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
	void Start () 
	{
		width = Screen.width;
		height = Screen.height;

		CalculateCameraRect ();
	}

	void Update ()
	{
		if (ScreenResized()) {
			CalculateCameraRect();
		}
	}

	bool ScreenResized()
	{
		if (width != Screen.width || height != Screen.height) {
			width = Screen.width;
			height = Screen.height;

			return true;
		}

		return false;
	}

	void CalculateCameraRect()
	{	
		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		//float targetaspect = 16.0f / 9.0f;
		float targetaspect = (float)virtualResolution.width / (float)virtualResolution.height;
		
		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;
		
		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;
		
		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();
		
		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;
			
			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;
			
			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;
			
			Rect rect = camera.rect;
			
			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;
			
			camera.rect = rect;
		}
	}
}
