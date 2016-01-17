using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.
    public GameObject wallGOCheck;          // GameObject to collect the reference to get transform, for walls ya know
    public GameObject levelControlGO;
    public Collider2D triggerCollider;
    public float moveForce = 365f;          // Amount of force added to move the player left and right.
    public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
                                            //public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
    public float jumpForce = 1000f;         // Amount of force added when the player jumps.
                                            //public AudioClip[] taunts;				// Array of clips for when the player taunts.
                                            //public float tauntProbability = 50f;	// Chance of a taunt happening.
                                            //public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    private MainMenuController inGameMenu;  //reference to the main menu controller on canvas
    private LevelController levelController;

    //private int tauntIndex;				// The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private Transform wallCheck;            // Position marking the front of the player to check for walls
    public bool grounded = false;           // Whether or not the player is grounded.
    public bool walled = false;             // Ya know, walls and shit
    private Rigidbody2D rb;                 // Unity 5.0 Correction reference            
    private float samePositionY;            // sets a float every fixed update to keep character falling on a wall
    //private Animator anim;				// Reference to the player's animator component.
    private float sunExposure = 100f;
    private bool inShade;
    public float melatonin = 0.0f;
    public Image sunBar;

    void Awake()
    {
        inShade = false;
        // Setting up references.
        Canvas canvas = FindObjectOfType<Canvas>();
        inGameMenu = canvas.GetComponent<MainMenuController>();
        levelController = levelControlGO.GetComponent<LevelController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        wallCheck = wallGOCheck.transform;
        //anim = GetComponent<Animator>();
        if (levelController.sunny)
            InvokeRepeating("SunDamage", 5, 1);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGameMenu.menuOpen)
            {
                inGameMenu.OnInGameMenuClose();
            } else
                inGameMenu.OnInGameMenuOpen();
        }

        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        walled = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
    }

    void FixedUpdate()
    {
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");

        if (!walled && !grounded)
        {
            // without next switch the character cannot switch directions in midair, possibly a good thing
            if (samePositionY == gameObject.transform.position.y)
                h = 0;
        }

        if (walled && !grounded)
        {
            if (h != 0)
            {
                h = 0;
            }
        }

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        //anim.SetFloat("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * rb.velocity.x < maxSpeed)
            // ... add a force to the player.
            rb.AddForce(Vector2.right * h * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);


        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && facingRight)
            // ... flip the player.
            Flip();

        // If the player should jump...
        if (jump)
        {
            // Set the Jump animator trigger parameter.
            //anim.SetTrigger("Jump");

            // Play a random jump audio clip.
            //int i = Random.Range(0, jumpClips.Length);
            //AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            // Add a vertical force to the player.
            rb.AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
        samePositionY = gameObject.transform.position.y;
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void SunDamage()
    {
        Debug.Log("Checking Sun");
        inShade = triggerCollider.IsTouchingLayers(1 << 13);
        if (!inShade || melatonin > 0)
        {
            Debug.Log("Sun Damage");
            sunExposure -= 4;
            // find out how to use layer mask to make the inshade bool
        }
        sunBar.fillAmount = sunExposure / 100.0f;
    }

}
//	public IEnumerator Taunt()
//	{
//		// Check the random chance of taunting.
//		float tauntChance = Random.Range(0f, 100f);
//		if(tauntChance > tauntProbability)
//		{
//			// Wait for tauntDelay number of seconds.
//			yield return new WaitForSeconds(tauntDelay);

//			// If there is no clip currently playing.
//			if(!GetComponent<AudioSource>().isPlaying)
//			{
//				// Choose a random, but different taunt.
//				tauntIndex = TauntRandom();

//				// Play the new taunt.
//				GetComponent<AudioSource>().clip = taunts[tauntIndex];
//				GetComponent<AudioSource>().Play();
//			}
//		}
//	}


//	int TauntRandom()
//	{
//		// Choose a random index of the taunts array.
//		int i = Random.Range(0, taunts.Length);

//		// If it's the same as the previous taunt...
//		if(i == tauntIndex)
//			// ... try another random taunt.
//			return TauntRandom();
//		else
//			// Otherwise return this index.
//			return i;
//	}
//}
