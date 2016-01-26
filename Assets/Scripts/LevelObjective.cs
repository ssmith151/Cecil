using UnityEngine;
using System.Collections;

public class LevelObjective : MonoBehaviour {

    private LevelController levelController;
    private GameObject levelControllerGO;
    public string playerText;
    public bool stopPlayer;
    public int timer;

    void Awake()
    {
        levelControllerGO = gameObject.transform.root.gameObject;
        levelController = levelControllerGO.GetComponent<LevelController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject a = other.gameObject;
            string b = playerText;
            levelController.Conversation(a, b, stopPlayer, timer);
        }
    }
}
