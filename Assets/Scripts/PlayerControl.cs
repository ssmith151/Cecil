﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.
    public GameObject wallGOCheck;          // GameObject to collect the reference to get transform, for walls ya know
    public GameObject levelControlGO;
    public GameObject mobileControls;
    public Collider2D triggerCollider;
    public float moveForce;          // Amount of force added to move the player left and right.
    public float maxSpeed;             // The fastest the player can travel in the x axis.
                                       //public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
    public float jumpForce;         // Amount of force added when the player jumps.
                                    //public AudioClip[] taunts;				// Array of clips for when the player taunts.
                                    //public float tauntProbability = 50f;	// Chance of a taunt happening.
                                    //public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    private MainMenuController inGameMenu;  //reference to the main menu controller on canvas
    private LevelController levelController;

    //private int tauntIndex;				// The index of the taunts array indicating the most recent taunt.         // A position marking where to check if the player is grounded.
    private Transform wallCheck;            // Position marking the front of the player to check for walls
    private Transform groundCheck;
    public bool grounded = false;           // Whether or not the player is grounded.
    public bool walled = false;             // Ya know, walls and shit
    private Rigidbody2D rb;                 // Unity 5.0 Correction reference            
    private float samePositionY;            // sets a float every fixed update to keep character falling on a wall
    private Animator anim;				    // Reference to the player's animator component.
    private float sunExposure = 100f;
    private float health = 100f;
    private bool inShade;
    private bool sunscreen;
    public float melatonin = 0.0f;
    private Image sunScreenTimer;
    private Image healthBar;
    private LayerMask shadeLayer;
    public CircleCollider2D charBottom;
    public CircleCollider2D groundSpace;
    private LayerMask groundLayer;
    private bool groundClose;
    public bool groundthrust;
    private bool groundedLastFrame;
    private SpriteRenderer characterRender;
    private SpriteRenderer cytosolRenderer;
    public AudioSource audSorce;
    public AudioClip[] audClips;
    public bool playerCanMove;
    //[HideInInspector]
    public bool enemyBounce;
    private bool invulnerable;
    private bool alive;
    private float h = 0.0f;
    private float v = 0.0f;
    private Button leftButton;
    private Button rightButton;
    private bool fallingThrough;

    void Awake()
    {

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        mobileControls.SetActive(false);
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        mobileControls.SetActive(true);
        leftButton = GameObject.Find("MobileLeftButton").GetComponent<Button>();
        rightButton = GameObject.Find("MobileRightButton").GetComponent<Button>();
#endif

        fallingThrough = false;
        enemyBounce = false;
        alive = true;
        characterRender = GameObject.Find("PlayerBody").GetComponent<SpriteRenderer>();
        cytosolRenderer = GameObject.Find("Cytosol").GetComponent<SpriteRenderer>();
        playerCanMove = true;
        inShade = false;
        sunscreen = false;
        groundedLastFrame = false;
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        sunScreenTimer = GameObject.Find("SunscreenTimer").GetComponent<Image>();
        shadeLayer = LayerMask.GetMask("Shade");
        groundLayer = LayerMask.GetMask("Ground");
        //charBottom = gameObject.GetComponent<CircleCollider2D>();
        // Setting up references.
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        inGameMenu = canvas.GetComponent<MainMenuController>();
        levelController = levelControlGO.GetComponent<LevelController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        //characterRender = gameObject.GetComponent<SpriteRenderer>();
        wallCheck = wallGOCheck.transform;
        anim = GetComponent<Animator>();
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
            }
            else
                inGameMenu.OnInGameMenuOpen();
        }
        if (rb.velocity.y < -10)
        {
            groundthrust = false;
        }
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        groundClose = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        walled = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;

    }
    public void Jump()
    {
        if (grounded)
            jump = true;
    }
    public void Left()
    {
        h = -1;
    }
    public void Right()
    {
        h = 1;
    }
    public void Still()
    {
        h = 0;
    }
    void FixedUpdate()
    {

        // Cache the horizontal input.
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
#endif
        if (groundedLastFrame)
        {
            grounded = groundSpace.IsTouchingLayers(groundLayer);
            groundedLastFrame = false;
        }
        else
        {
            grounded = groundSpace.IsTouchingLayers(groundLayer);
            groundedLastFrame = true;
        }
        if (!walled && !grounded)
        {
            // without next switch the character cannot switch directions in midair, possibly a good thing
            if (samePositionY == gameObject.transform.position.y)
                gameObject.transform.Translate(0.0f, -0.02f, 0.0f);
            //Debug.Log("stuck");
            //h = 0;
            if (groundClose && !groundthrust)
            {
                rb.AddForce(new Vector2(0.0f, rb.velocity.y * -2.0f));
                groundthrust = true;
            }
        }
        anim.SetBool("grounded", grounded);
        if (!playerCanMove)
        {
            h = 0;
        }
        if (walled && !grounded)
        {
            if (h != 0)
            {
                h = 0;
            }
        }
        anim.SetFloat("speed", Mathf.Abs(h));
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

        // If the player should jump and isn't stuck/Immovable
        if (jump && playerCanMove)
        {
            float i = Random.Range(-0.4f, -0.2f);
            audSorce.pitch = 1 + i;
            audSorce.PlayOneShot(audClips[0], 0.3f);

            // Add a vertical force to the player.
            // if (rb.velocity.y <= jumpForce)    Does not fix super jump
            rb.AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
        if (Input.GetButton("Jump") && enemyBounce)
        {
            enemyBounce = false;
            rb.AddForce(new Vector2(0f, jumpForce * 1.5f));
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
        inShade = triggerCollider.IsTouchingLayers(shadeLayer);
        if (!inShade & melatonin <= 0)
        {
            sunExposure -= 4;
            // find out how to use layer mask to make the inshade bool
        }
        else
        {
            sunExposure += 12;
        }
        sunExposure = Mathf.Clamp(sunExposure, -1, 100.0f);
        if (sunExposure <= 0)
        {
            TakeDamage(10.0f);
            StartCoroutine(DamageFromSun());
        }
        characterRender.material.color = Color.Lerp(Color.red, Color.white, (sunExposure / 100.0f));
    }
    IEnumerator DamageFromSun()
    {
        if (health <= 0)
        {
            yield break;
        }
        Color preDamageColor = cytosolRenderer.material.color;
        cytosolRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        cytosolRenderer.material.color = preDamageColor;
    }
    // try to use a image and use alpha in this from another texture
    // link the texture to an animator and use that lavvue in animation
    // OR use an overlay on the GameObject and create alpha with reduction
    void SunscreenCounter()
    {
        melatonin -= 1;
        sunScreenTimer.fillAmount = melatonin / 100.0f;
        melatonin = Mathf.Clamp(melatonin, 0, 100);
        if (melatonin == 0)
        {
            CancelInvoke("SunscreenCounter");
            sunscreen = false;
        }
    }
    public void SunscreenApply()
    {
        melatonin = 100;
        if (!sunscreen)
        {
            InvokeRepeating("SunscreenCounter", 0.0f, 0.2f);
            sunscreen = true;
        }
    }
    public void TakeDamage(float damageIn)
    {
        if (invulnerable)
            return;
        health -= damageIn;
        healthBar.fillAmount = health / 100.0f;
        Mathf.Clamp(health, 0, 100);
        // play an audioclip
        anim.SetTrigger("TookDamage");
        audSorce.pitch = Random.Range(0.8f, 1.1f);
        audSorce.PlayOneShot(audClips[5], 1.0f);
        StartCoroutine(DamageShield(0.2f));
        if (health <= 0 && alive)
        {
            alive = false;
            PlayerDeath();
            StartCoroutine(ReloadGame());
            StopPlayer(3);
        }
    }
    public void DrownDamage()
    {
        anim.SetTrigger("TookDamage");
        health -= 2;
        healthBar.fillAmount = health / 100.0f;
        Mathf.Clamp(health, 0, 100);
        // play an audioclip
        StartCoroutine(DamageShield(0.2f));
        if (health <= 0 && alive)
        {
            alive = false;
            PlayerDrown();
            StartCoroutine(ReloadGame());
            StopPlayer(3);
        }
    }
    IEnumerator DamageShield(float waitUntil)
    {
        if (health <= 0)
        {
            yield break;
        }
        Color preDamageColor = characterRender.material.color;
        characterRender.material.color = Color.red;
        invulnerable = true;
        yield return new WaitForSeconds(waitUntil);
        invulnerable = false;
        characterRender.material.color = preDamageColor;
        anim.ResetTrigger("TookDamage");
    }
    void PlayerDeath()
    {
        // play death noise
        //play death animation
        Collider2D[] playerColls = GetComponents<Collider2D>();
        foreach (Collider2D col in playerColls)
        {
            col.isTrigger = true;
        }
        SpriteRenderer[] playerSprites = GetComponents<SpriteRenderer>();
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.sortingLayerName = "UI";
        }
        Camera.main.GetComponent<CameraFollow>().enabled = false;
    }
    void PlayerDrown()
    {
        // play drown noise
        audSorce.pitch = 1.0f;
        audSorce.PlayOneShot(audClips[1], 1.5f);
        //play drown animation
        Collider2D[] playerColls = GetComponents<Collider2D>();
        foreach (Collider2D col in playerColls)
        {
            col.isTrigger = true;
        }
        rb.velocity = new Vector2(0.0f, 0.0f);
        SpriteRenderer[] playerSprites = GetComponents<SpriteRenderer>();
        foreach (SpriteRenderer spr in playerSprites)
        {
            spr.sortingLayerName = "UI";
        }
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        StartCoroutine(DrownMove());
    }
    IEnumerator DrownMove()
    {
        rb.gravityScale = 0.00f;
        float startY = transform.position.y;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, startY + Mathf.Sin(Time.time),
                transform.position.z), 0.01f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void AddHealth(float healthIn)
    {
        health += healthIn;
        healthBar.fillAmount = health / 100.0f;
        Mathf.Clamp(health, 0, 100);
        // play an audioclip
        audSorce.pitch = 1.0f;
        audSorce.PlayOneShot(audClips[2], 0.10f);
    }
    IEnumerator ReloadGame()
    {
        // ... pause briefly and play gameover noise
        audSorce.pitch = 1.0f;
        audSorce.PlayOneShot(audClips[3], 2.0f);
        yield return new WaitForSeconds(3);
        // ... and then reload the level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
    }
    public void StopPlayer(int stopTime)
    {
        if (playerCanMove)
        {
            StartCoroutine(StopPlayerCo(stopTime));
        }
    }
    IEnumerator StopPlayerCo(int stopTime)
    {
        playerCanMove = false;
        yield return new WaitForSeconds(stopTime);
        playerCanMove = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (v == -1 && !fallingThrough)
        {
            if (other.CompareTag("Platform"))
            {
                fallingThrough = true;
                anim.SetBool("fallThrough", true);
                foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
                {
                    col.isTrigger = true;
                }
            }
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (fallingThrough && other.CompareTag("Platform"))
        {
            fallingThrough = false;
            anim.SetBool("fallThrough", false);
            foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
            {
                if (col != groundSpace) 
                    col.isTrigger = false;
            }
        }
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
