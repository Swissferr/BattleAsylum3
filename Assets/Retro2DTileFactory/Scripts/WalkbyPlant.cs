using UnityEngine;
using System.Collections;

// Script that toggles the sprite when you walk by a plant
// Idea borrowed from 1001 Spikes
public class WalkbyPlant : MonoBehaviour {

	public Sprite plantMoved;

	private SpriteRenderer spriteRenderer;
	private Sprite initialSprite;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();

		initialSprite = spriteRenderer.sprite;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		spriteRenderer.sprite = plantMoved;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		spriteRenderer.sprite = initialSprite;
	}
}
