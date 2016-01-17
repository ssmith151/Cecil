using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour
{

    public int newLevelIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Application.LoadLevel(newLevelIndex);
    }

}
