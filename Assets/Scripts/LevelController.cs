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
        dialogGO = GameObject.Find("Dialog");
        CC = character.GetComponent<PlayerControl>();
        dialogGO.SetActive(false);
    }

    public void Conversation(GameObject talker, string dialog, bool makeStop)
    {
        if (dialogGO.activeSelf)
        {
            return;
        }
        if (makeStop)
        {
            CC.StopPlayer();
        }
        if (dialog.Length > 16)
            dialog = DialogChop(dialog);
        dialogGO.SetActive(true);
        StartCoroutine( Fader(true));
        dialogGO.transform.position = new Vector3(talker.transform.position.x, talker.transform.position.y + 1.0f, talker.transform.position.z);
        dialogGO.transform.SetParent(talker.transform);
    //    StartCoroutine(FlipChecker());
        StartCoroutine(AnimateText(dialog));
    }
    string DialogChop(string dialog)
    {
        string newDialog = "";
        string[] dialogWords = dialog.Split(' ');
        int counter = 0;
        foreach (string s in dialogWords)
        {
            counter += s.Length;
            //Debug.Log(s + " is " + s.Length + " and the count is " + counter);
            if (counter > 14)
            {
                newDialog += s + System.Environment.NewLine;
                counter = 0;
            } else
            {
                newDialog += s + " ";
            }
        }
        return newDialog;
    }
    bool CharacterFlipped()
    {
        if (CC.facingRight)
            return false;
        else
            return true;
    }
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
        yield return StartCoroutine(Fader(false));
        dialogBubble.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        dialogGO.gameObject.SetActive(false);
    }
    IEnumerator AnimateText(string dialogComplete)
    {
        string dialogPart = "";
        float dialogBubbleWidth = 1.0f;
        float dialogBubbleHeight = 1.0f;
        for (int i = 0; i < dialogComplete.Length; i++)
        {
            dialogPart += dialogComplete[i];
            dialogText.text = dialogPart;
            if (i > 12) {
                dialogBubbleWidth = dialogPart.Length / 8.0f;
                dialogBubbleWidth = Mathf.Clamp(dialogBubbleWidth, 1.0f, 2.5f);
            }
            if (i > 46)
            {
                if (dialogPart.EndsWith(System.Environment.NewLine))
                    dialogBubbleHeight += 0.333f;
            }
            TextFixer();
            dialogBubble.transform.localScale = new Vector3(dialogBubbleWidth, dialogBubbleHeight, 1.0f);
            yield return new WaitForSeconds(0.1f * dialogScrollTime);
        }
        StartCoroutine(EndBubble());
    }
    void TextFixer()
    {
        if (CharacterFlipped())
        {
            dialogText.rectTransform.localScale = new Vector3(-dialogText.rectTransform.localScale.x *
                Mathf.Sign(dialogText.rectTransform.localScale.x),
                dialogText.rectTransform.localScale.y, dialogText.rectTransform.localScale.z);
        } else
        {
            dialogText.rectTransform.localScale = new Vector3(dialogText.rectTransform.localScale.x *
                Mathf.Sign(dialogText.rectTransform.localScale.x),
                dialogText.rectTransform.localScale.y, dialogText.rectTransform.localScale.z);
        }
    }
    IEnumerator Fader(bool incoming)
    {
        Color fadeColorBubble = dialogBubble.color;
        Color fadeColorText = dialogText.color;
        if (incoming)
        {
            for (float i = 0; i <= 10; i++)
            {
                fadeColorBubble = new Color(fadeColorBubble.r, fadeColorBubble.g, fadeColorBubble.b, i * 0.1F);
                fadeColorText = new Color(fadeColorText.r, fadeColorText.g, fadeColorText.b, i * 0.1F);
                dialogText.color = fadeColorText;
                dialogBubble.color = fadeColorBubble;
                yield return new WaitForSeconds(0.02f);
            }
        } else
        {
            for (float i = 10; i >= 0; i--)
            {
                fadeColorBubble = new Color(fadeColorBubble.r, fadeColorBubble.g, fadeColorBubble.b, i * 0.1F);
                fadeColorText = new Color(fadeColorText.r, fadeColorText.g, fadeColorText.b, i * 0.1F);
                dialogText.color = fadeColorText;
                dialogBubble.color = fadeColorBubble;
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
