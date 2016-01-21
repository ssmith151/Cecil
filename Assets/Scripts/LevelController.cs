using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public bool sunny;
    public bool water;
    public GameObject character;
    public SpriteRenderer dialogBubble;
    public Vector2 offSetPosition;
    public float fadeDelay;
    public float dialogWait;
    public float dialogScrollTime;
    public Text dialogText;

    public void Conversation(GameObject talker, string dialog)
    {
        if (dialogBubble.gameObject.activeSelf)
        {
            return;
        }
        dialogBubble.gameObject.SetActive(true);
        //Fader();
        dialogBubble.transform.position = new Vector3(talker.transform.position.x, talker.transform.position.y + 1.0f, talker.transform.position.z);
        dialogBubble.transform.SetParent(talker.transform);
        StartCoroutine(AnimateText(dialog));
        StartCoroutine(EndBubble());
    }
    IEnumerator EndBubble()
    {
        yield return new WaitForSeconds(dialogWait);
        dialogBubble.gameObject.SetActive(false);
    }
    IEnumerator AnimateText(string dialogComplete)
    {
        string dialogPart = "";
        for (int i = 0; i < dialogComplete.Length; i++)
        {
            dialogPart += dialogComplete[i];
            dialogText.text = dialogPart;
            yield return new WaitForSeconds(0.1f * dialogScrollTime);
        }
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
