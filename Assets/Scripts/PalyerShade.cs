using UnityEngine;
using System.Collections;

public class PalyerShade : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.SendMessage("InShade");
    }
}
