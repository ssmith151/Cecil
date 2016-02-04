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
    public bool dialog;
    public string speech;
    public int timer;
    private bool done;

    void Awake()
    {
        done = false;
        levelControllerGO = gameObject.transform.root.gameObject;
        levelController = levelControllerGO.GetComponent<LevelController>();
        if (!dialog && helpMenu == null)
            helpMenu = GameObject.Find("HelpPanel");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!dialog && !done)
            {
                done = true;
                GameObject a = helpMenu;
                string b = title;
                string c = panelOne;
                string d = panelTwo;
                string e = panelThree;
                levelController.HelpPanel(a, b, c, d, e);
                //levelController.Conversation(a, b, stopPlayer, timer);
            } else if (!done)
            {
                GameObject a = other.gameObject;
                string b = speech;
                bool c = stopPlayer;
                int d = timer;
                levelController.Conversation(a, b, c, d);
                done = true;
                stopPlayer = false;
            } else
            {
                GameObject a = other.gameObject;
                string b = speech;
                bool c = stopPlayer;
                int d = timer;
                levelController.Conversation(a, b, c, d);
            }
        }
    }
}
