using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public AudioClip collectCoinFx;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (collectCoinFx != null && collectCoinFx.isReadyToPlay) {
			AudioSource.PlayClipAtPoint(collectCoinFx, this.transform.position);		
		}

		this.gameObject.SetActive (false);
	}
}
