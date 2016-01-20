using UnityEngine;
using System.Collections;

public class SunscreenDolop : MonoBehaviour
{




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.SendMessage("SunscreenApply");
    }
}
