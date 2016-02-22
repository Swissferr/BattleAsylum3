using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class UserInput {
	public float horizontalInput;
	public float verticalInput;
	public bool jump;
	public bool fire;
}

[Serializable]
public class PlayerState {	
	public bool facingRight;
	public bool isGrounded;
	public bool isJumping;
	public bool isCrouched;
	public bool isOnPlatform;
	public float speedX;
	public float speedY;
}

[Serializable]
public class MovementParameters {
	public float maxSpeed;
	public float moveForce;
	public float jumpForce;
}

[Serializable]
public class PlayerFx {
	public AudioClip jumpFx;
	public AudioClip dieFx;	
}

public class PlayerControl : MonoBehaviour {

	public UserInput UserInput;
	public PlayerState PlayerState;

	public MovementParameters MovementParameters;
	public PlayerFx PlayerFx;

	private Animator animator;
	private Collider2D groundCollider;
	private Transform groundCheck;

	private Vector3 initialPosition;

	void Start() {
		animator = GetComponent<Animator> ();
		groundCheck = transform.Find ("GroundCheck");

		initialPosition = transform.position;
	}
	
	void Update () {

		// Get current user input
		UserInput.horizontalInput = Input.GetAxis ("Horizontal");
		UserInput.verticalInput = Input.GetAxis ("Vertical");
		UserInput.jump = Input.GetButton ("Jump");
		UserInput.fire = Input.GetButton ("Fire1");  // http://docs.unity3d.com/ScriptReference/Input.GetButton.html
	
		bool wasJumping = PlayerState.isJumping;

		if (PlayerState.isJumping) {
			PlayerState.isJumping = false;
			PlayerState.isGrounded = false;
		} 
		else {
			PlayerState.isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		}

		PlayerState.speedX = Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x);
		PlayerState.speedY = Mathf.Abs (GetComponent<Rigidbody2D>().velocity.y);
		PlayerState.isCrouched = (PlayerState.isGrounded
		                          && (UserInput.verticalInput < 0));


		PlayerState.isJumping = !PlayerState.isJumping
								&& PlayerState.isGrounded 
								&& !PlayerState.isCrouched 
								&& UserInput.jump;
		
		animator.SetBool ("Grounded", PlayerState.isGrounded);
		animator.SetBool ("Crouched", PlayerState.isCrouched);
		animator.SetBool ("Jump", PlayerState.isJumping);
		animator.SetFloat ("Speed", PlayerState.speedX);
	}

	void FixedUpdate () {

		if( (PlayerState.isGrounded && !PlayerState.isCrouched) ||
		    PlayerState.isJumping ||
		   !PlayerState.isGrounded )
		{
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(UserInput.horizontalInput * GetComponent<Rigidbody2D>().velocity.x < MovementParameters.maxSpeed)
				// ... add a force to the player.
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * UserInput.horizontalInput * MovementParameters.moveForce);
			
			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > MovementParameters.maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * MovementParameters.maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}

		if (PlayerState.isJumping) {

			// http://feedback.unity3d.com/suggestions/instant-animation-state-transition
			// might require Unity Pro to get around manually setting the animation state
			animator.Play("SmokingDudeJump"); // fixes bug with leftover frames

			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, MovementParameters.jumpForce));
		}
		
		// If the input is moving the player right and the player is facing left...
		if(UserInput.horizontalInput > 0 && !PlayerState.facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(UserInput.horizontalInput < 0 && PlayerState.facingRight)
			// ... flip the player.
			Flip();

	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		PlayerState.facingRight = !PlayerState.facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Jump()
	{
		if(PlayerFx.jumpFx.isReadyToPlay)
			AudioSource.PlayClipAtPoint (PlayerFx.jumpFx, transform.position);
	}

	void Die()
	{
		if(PlayerFx.dieFx.isReadyToPlay)
			AudioSource.PlayClipAtPoint (PlayerFx.dieFx, transform.position);
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		// Once your collision logic grows more you would want it in a different class
		if (collision.gameObject.tag == "Enemy") {
			Die ();
			ResetPosition();
		}
	}

	void ResetPosition() 
	{
		transform.position = initialPosition;
	}
}
