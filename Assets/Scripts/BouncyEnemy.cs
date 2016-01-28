using UnityEngine;
using System.Collections;

public class BouncyEnemy : MonoBehaviour
{

    public float jumpHeight;
    public float playerRadius;
    public float bounceWait;
    public float paceArchWait;
    public float paceArchForce;
    public float chaseDelay;
    public float chaseWait;
    public float chaseArchWait;

    private Rigidbody2D rb;
    private BoxCollider2D bocCol;
    private Vector3 startPosition;
    private float startOffset;
    private float currentArchDir;
    private bool facingRight;
    private bool chasing;
    private bool dying;

    // Use this for initialization
    void Awake()
    {
        dying = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPosition = gameObject.transform.position;
        InvokeRepeating("PaceBounce", 0.0f, bounceWait);
    }

    void PaceBounce()
    {
        Vector2 bounceDir = new Vector2(0.0f, jumpHeight / 3);
        startOffset = gameObject.transform.position.x - startPosition.x;
        if (startOffset <= 0.0f)
        {
            rb.AddForce(bounceDir);
            currentArchDir = paceArchForce;
            if (!facingRight)
                Flip();
        }
        else
        {
            bounceDir = new Vector2(0.0f, bounceDir.y);
            rb.AddForce(bounceDir);
            currentArchDir = -paceArchForce;
            if (facingRight)
                Flip();
        }
        StartCoroutine(Arch(paceArchWait, currentArchDir));
    }
    IEnumerator Arch(float archWait, float archForce)
    {
        yield return new WaitForSeconds(archWait);
        rb.AddForce(new Vector2(archForce, 0.0f));
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !dying) {
            other.gameObject.GetComponent<PlayerControl>().TakeDamage(10);
        }
    }
    public void Death()
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
        dying = true;
        Flip();
        Collider2D[] enemyColls = GetComponents<Collider2D>();
        foreach(Collider2D col in enemyColls)
        {
            col.isTrigger = true;
        }
        rb.AddForce(new Vector2(Random.Range(-150.0f, 150.0f), Random.Range(50.0f, 300.0f)));
        rb.AddTorque(Random.Range(0.0f, 5.0f));
        Destroy(gameObject, 0.5f);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !chasing)
        {
            StartCoroutine(ChasePlayer(other.gameObject));
            CancelInvoke("PaceBounce");
        }
    }
    IEnumerator ChasePlayer(GameObject GO)
    {
        float currentDistance = gameObject.transform.position.x - GO.transform.position.x;
        chasing = true;
        Vector2 bounceDir = new Vector2(0.0f, jumpHeight);
        yield return new WaitForSeconds(chaseDelay);
        while (currentDistance < playerRadius)
        {
            if (currentDistance <= 0.0f)
            {
                rb.AddForce(bounceDir);
                currentArchDir = paceArchForce;
                if (!facingRight)
                    Flip();
            }
            else
            {
                rb.AddForce(bounceDir);
                currentArchDir = -paceArchForce;
                if (facingRight)
                    Flip();
            }
            StartCoroutine(Arch(chaseArchWait, currentArchDir));
            yield return new WaitForSeconds(chaseWait);
            if (GO != null)
                currentDistance = gameObject.transform.position.x - GO.transform.position.x;
        }
        InvokeRepeating("PaceBounce", 0.0f, bounceWait);
        chasing = false;
    }
}
