using UnityEngine;
using System.Collections;

public class killOnHit : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            GameObject player = other.gameObject;
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.dead = true;
        }
        else {
            Destroy(other.gameObject);
        }

    }

}
