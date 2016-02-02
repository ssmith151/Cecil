using UnityEngine;
using System.Collections;

public class PlayerDrown : MonoBehaviour
{
    private bool takingDamage;

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!takingDamage && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().DrownDamage();
        }
    }
    IEnumerable DrownDelay()
    {
        takingDamage = true;
        yield return new WaitForSeconds(0.05f);
        takingDamage = false;
    }
}
