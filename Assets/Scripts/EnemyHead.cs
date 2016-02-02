using UnityEngine;
using System.Collections;

public class EnemyHead : MonoBehaviour
{
    public GameObject notification;
    public int pointAmount;

    private LevelController LC;
    private BouncyEnemy bouncyEnemy;
    private bool isDead;

    void Awake()
    {
        LC = FindObjectOfType<LevelController>();
        isDead = false;
        bouncyEnemy = gameObject.transform.parent.gameObject.GetComponent<BouncyEnemy>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isDead)
        {
            isDead = true;
            GameObject not = Instantiate(notification, transform.position, Quaternion.identity) as GameObject;
            not.GetComponentInChildren<DestroyTime>().notificationMessage = pointAmount.ToString();
            LC.AddPoints(pointAmount);
            PlayerControl PlyCon = other.GetComponent<PlayerControl>();
            PlyCon.enemyBounce = true;
            bouncyEnemy.Death();
            StartCoroutine(PreDeath(PlyCon));
        }
    }
    IEnumerator PreDeath(PlayerControl PC)
    {
        yield return new WaitForSeconds(0.15f);
        DeathSoon();
        PC.enemyBounce = false;
    }
    void DeathSoon()
    {
        Collider2D[] cols = gameObject.GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            col.isTrigger = true;
        }
    }
}
