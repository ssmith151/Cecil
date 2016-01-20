using UnityEngine;
using System.Collections;

public class ObjectivePoint : MonoBehaviour {

    public LevelController levelController;
    public GameObject levelControllerGO;

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject a = other.gameObject;
        string b = "It is dark and cool here!";
        object[] convoObj = new object[2];
        convoObj[0] = a;
        convoObj[1] = b;
        levelController.Conversation(a, b);
    }
}
