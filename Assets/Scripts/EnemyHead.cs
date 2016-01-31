using UnityEngine;
using System.Collections;

public class EnemyHead : MonoBehaviour
{

    private BouncyEnemy bouncyEnemy;

    void Awake()
    {
        bouncyEnemy = gameObject.transform.parent.gameObject.GetComponent<BouncyEnemy>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("AddPoints", 5.0f);
            bouncyEnemy.Death();
            Invoke("DeathSoon", 0.05f);
        }
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
