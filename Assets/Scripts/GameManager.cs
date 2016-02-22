using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public PlayerController p1;
    //public PlayerController p2;

    Animator anim;
    private bool begin = true;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        //Players obviously start with disabled controls.
        //The simplest way I found was to simply disable both PlayerController scripts.
        p1.enabled = false;
        //p2.enabled = false;
        //This flag tells the game manager wether or not it has to play the countdown animation.
        begin = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (begin)
        {
            anim.Play("CountDown");
            //waits for 3 seconds then calls the enableControls() function.
            Invoke("enableControls", 3);
            begin = false;
        }
        //This checks every frame if a player is dead.
        if (p1.isDead() /*|| p2.isDead()*/)
        {           
            //Starts a coroutine that waits for 3 seconds before reloading the scene.
            StartCoroutine(reloadCoroutine());
        }
    }
    
    IEnumerator reloadCoroutine()
    {                 
        yield return new WaitForSeconds(3);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);   
    }

    void enableControls()
    {
        p1.enabled = true;
        //p2.enabled = true;
    }

}
