using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelObjective : MonoBehaviour {

    private LevelController levelController;
    private GameObject levelControllerGO;
    public GameObject helpMenu;
    public string title;
    public string panelOne;
    public string panelTwo;
    public string panelThree;
    public bool stopPlayer;
    public int timer;
    private bool done;

    void Awake()
    {
        done = false;
        levelControllerGO = gameObject.transform.root.gameObject;
        levelController = levelControllerGO.GetComponent<LevelController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !done)
        {
            done = true;
            GameObject a = helpMenu;
            string b = title;
            string c = panelOne;
            string d = panelTwo;
            string e = panelThree;
            levelController.HelpPanel(a, b, c, d, e);
            //levelController.Conversation(a, b, stopPlayer, timer);
        }
    }
}
