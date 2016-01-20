using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public bool sunny;
    public bool water;
    public GameObject character;
    public SpriteRenderer dialogBubble;
    public Vector2 offSetPosition;
    public float fadeDelay;

    

    public void Conversation(GameObject talker, string dialog)
    {
        if (dialogBubble.gameObject.activeSelf)
        {
            return;
        }
        dialogBubble.gameObject.SetActive(true);
        Fader();
        dialogBubble.transform.position = new Vector3(talker.transform.position.x, talker.transform.position.y, talker.transform.position.z);
        dialogBubble.transform.SetParent(talker.transform);
    }
    void Fader()
    {
        Color fadeColor = dialogBubble.color;
        for (float i = 0; i < 255; i++)
        {
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, i);
        }
    }
}
