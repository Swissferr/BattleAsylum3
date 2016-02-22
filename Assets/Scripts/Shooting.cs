using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public PlayerController controller;
    //public GameObject shotRight;
    //public GameObject shotLeft;
    public float fireRate;
    private float nextFire;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown(controller.attackButton) && controller.hasAGun)
        {
            if (Time.time > nextFire)
            {
                if (controller.facingRight)
                {
                    nextFire = Time.time + fireRate;
                    //Instantiate(shotRight, shotSpawn.position, shotSpawn.rotation);                   
                }
                else if (!controller.facingRight)
                {
                    nextFire = Time.time + fireRate;
                    //Instantiate(shotLeft, shotSpawn.position, shotSpawn.rotation);
                }
            }
        }
    }
}
