using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;

    public float maxSpeed = 10f;  
    public float jumpSpeed = 10f;
    public float speed;
    public bool grounded = false;

    public int health;
    const int MAX_HEALTH = 3;
    public bool dead = false;
 
    public string jumpButton = "jump";
    public string horizontalControl = "horizontal";
    public string attackButton = "attack";

    private Transform groundCheck1;
    private Transform groundCheck2;
    private Transform wallCheckFront;
    private Transform wallCheckBack;
    public AreaEffector2D effector;
    public GameObject bullet;
    public BulletScript bulScript;

    public bool facingRight = true;
    private bool jump = false;
    private bool wallJump = false;
    private bool canWallJump = false;
    private float moveHorizontal;
    private bool shoot = false;

    // Gun stuff
    public bool hasAGun = false;
    private int bulletsRemaining = 2;    
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;
    //End of gun stuff

    Animator anime;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //important!
        anime = GetComponentInChildren<Animator>();

        groundCheck1 = transform.Find("GroundCheck1");
        groundCheck2 = transform.Find("GroundCheck2");
        wallCheckFront = transform.Find("WallCheckFront");
        wallCheckBack = transform.Find("WallCheckBack");

        health = MAX_HEALTH;
        dead = false;
        updateAnim();
    }

    // Update is called once per frame
    void Update()
    {
        //grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
        //updateAnim();

        if (Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground")))
            grounded = true;
        else
            grounded = false;

        updateAnim();

        if (Physics2D.Linecast(transform.position, wallCheckFront.position, 1 << LayerMask.NameToLayer("Wall")) && !grounded)
        {
            flip();
            canWallJump = Physics2D.Linecast(transform.position, wallCheckBack.position, 1 << LayerMask.NameToLayer("Wall"));
            updateAnim();
        }

        checkGrounded();

        //Grounded jump
        if (Input.GetButtonDown(jumpButton) && grounded)
            jump = true;

        //wall-jump
        if (Input.GetButtonDown(jumpButton) && canWallJump)
            wallJump = true;

        //Attack with sword
        if (Input.GetButtonDown(attackButton) && !hasAGun)
            attack();

        //Shoot the gun
        if (Input.GetButtonDown(attackButton) && hasAGun)
        {
            if (Time.time > nextFire && bulletsRemaining > 0)
            {
                //shoot = true;
                if (facingRight)
                {
                    nextFire = Time.time + fireRate;
                    bulScript.right = facingRight;
                    Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
                    bulletsRemaining--;
                }
                else if (!facingRight)
                {
                    nextFire = Time.time + fireRate;
                    bulScript.right = facingRight;
                    Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);                    
                    bulletsRemaining--;
                }
                //shoot = false;
            }
            else if(bulletsRemaining == 0)
            {
                hasAGun = false;
            }
        }

        //Triggers the gun animations
        if (hasAGun)
        {
            changeWeapon();
        }
        //Disables the gun animations.
        else if (!hasAGun)
        {
            changeWeapon();
        }

        if (dead)
        {
            anime.Play("PixelCharAnim_Sword_death");
            
            PlayerController pc = GetComponent<PlayerController>();
            pc.enabled = false;
            
            //The game now restarts itself by detecting that the player died.
            //See the GameManager script.       
        }
       
    }

    void FixedUpdate()
    {

        moveHorizontal = Input.GetAxis(horizontalControl);

        if (grounded && rb.velocity.y == 0)
        {
            updateAnim();
            rb.velocity = new Vector2(moveHorizontal * maxSpeed, rb.velocity.y);
        }

        if (jump && rb.velocity.y == 0)
        {
            Vector2 jumpForce = new Vector2(0.0f, 100f);
            rb.AddForce(jumpForce * jumpSpeed);
            updateAnim();
            jump = false;
        }
        else if (wallJump)
        {
            if (facingRight)
            {
                Vector2 jumpForce = new Vector2(60.0f, 125f);
                rb.AddForce(jumpForce * jumpSpeed);
                wallJump = false;
                canWallJump = false;
                updateAnim();
            }
            else if (!facingRight)
            {
                Vector2 jumpForce = new Vector2(-60.0f, 125f);
                rb.AddForce(jumpForce * jumpSpeed);                
                wallJump = false;
                canWallJump = false;
                updateAnim();
            }
        }

        if (moveHorizontal > 0 && !facingRight)
        {
            if (grounded)
                flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            if (grounded)
                flip();
        }      
    }

    //Reverses the local scale of the player to make him face the other way.
    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (facingRight)
            effector.forceAngle = 60;
        else
            effector.forceAngle = 120;
    }

    //Checks if the player is grounded. Might be deleted eventually.
    void checkGrounded()
    {
        if (grounded)
            canWallJump = false;

        updateAnim();
    }

   void attack()
    {
        //The sword hitbox is handled by the animator. GG
        anime.Play("PixelCharAnim_Sword_mediumAtk");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //The player loses 1 health if he gets hit by a sword.
        if (other.gameObject.CompareTag("SwordHitbox"))
        {
            if (health > 0)
                health--;
            else
            {
                //should never happen
            }

            //If his health is at 0, he dies, which is not good.
            if (health == 0)
            {
                dead = true;
                anime.SetBool("DED", dead);
            }

            //The player instantly dies if he hits a stage hazard.
            //if (other.gameObject.CompareTag("Hazard"))
            //    dead = true;
        }
        else if (other.gameObject.CompareTag("Gun"))
        {
            hasAGun = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (health > 0)
                health--;
            else
            {
                //should never happen
            }

            //If his health is at 0, he dies, which is not good.
            if (health == 0)
            {
                dead = true;
                anime.SetBool("DED", dead);
            }
            Destroy(other);
        }
    }

    //Updates the animator.
    void updateAnim()
    {
        anime.SetBool("Ground", grounded);
        anime.SetBool("WallRide", canWallJump);
        anime.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        anime.SetBool("DED", dead);
    }

    void changeWeapon()
    {
        if (hasAGun)
        {
            anime.SetLayerWeight(0, 0f);
            anime.SetLayerWeight(1, 1f);
        }
        //Disables the gun animations.
        else if (!hasAGun)
        {
            anime.SetLayerWeight(0, 1f);
            anime.SetLayerWeight(1, 0f);
        }
    }

    public bool isDead()
    {
        return dead;
    }
}
