using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public bool sunny;
    public bool water;
    public GameObject character;
    private PlayerControl CC;
    public SpriteRenderer dialogBubble;
    public Vector2 offSetPosition;
    public float fadeDelay;
    public float dialogWait;
    public float dialogScrollTime;
    public Text dialogText;
    private GameObject dialogGO;

    void Awake ()
    {
        dialogGO = dialogBubble.transform.parent.gameObject;
        CC = character.GetComponent<PlayerControl>();
    }

    public void Conversation(GameObject talker, string dialog)
    {
        if (dialogGO.activeSelf)
        {
            return;
        }
        dialogGO.SetActive(true);
        StartCoroutine( Fader());
        dialogGO.transform.position = new Vector3(talker.transform.position.x, talker.transform.position.y + 1.0f, talker.transform.position.z);
        dialogGO.transform.SetParent(talker.transform);
    //    StartCoroutine(FlipChecker());
        StartCoroutine(AnimateText(dialog));
        StartCoroutine(EndBubble());
    }
    //bool CharacterFlipped()
    //{
    //    if (CC.facingRight)
    //        return true;
    //    else
    //        return false;
    //}
    //IEnumerator FlipChecker()
    //{
    //    if (CharacterFlipped()) { 
    //        gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x,
    //            gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    //    }
    //    yield return new WaitForSeconds(0.05f);
    //}
    IEnumerator EndBubble()
    {
        yield return new WaitForSeconds(dialogWait);
        dialogBubble.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        dialogGO.gameObject.SetActive(false);
    }
    IEnumerator AnimateText(string dialogComplete)
    {
        string dialogPart = "";
        float dialogBubbleWidth = 1.0f;
        for (int i = 0; i < dialogComplete.Length; i++)
        {
            dialogPart += dialogComplete[i];
            dialogText.text = dialogPart;
            if (i > 16) {
                dialogBubbleWidth = dialogPart.Length / 12.0f;
                Mathf.Clamp(dialogBubbleWidth, 1.0f, 2.5f);
                dialogBubble.transform.localScale = new Vector3(dialogBubbleWidth, 1.0f, 1.0f);
            }
            yield return new WaitForSeconds(0.1f * dialogScrollTime);
        }
    }
    IEnumerator Fader()
    {
        Color fadeColor = dialogBubble.color;
        for (float i = 0; i < 10; i++)
        {
            fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, i*0.1F);
            dialogBubble.color = fadeColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
