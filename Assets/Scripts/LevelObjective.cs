using UnityEngine;
using System.Collections;

public class LevelObjective : MonoBehaviour {

    public LevelController levelController;
    public GameObject levelControllerGO;

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject a = other.gameObject;
        string b = "It is dark\nand cool here!";
        levelController.Conversation(a, b);
    }
}
