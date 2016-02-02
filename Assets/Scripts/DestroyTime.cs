using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestroyTime : MonoBehaviour {

    public bool imageNotificaion;
    public Sprite image;
    public string notificationMessage;


    private Text notificationText;
    private SpriteRenderer notificationImage;
    private Color notificationColor;
    private Color fadeColor;

    // Use this for initialization
    void Start () {
        Destroy(gameObject.transform.parent.gameObject, 2.0f);
        if (imageNotificaion)
            ImageNotification();
        else
            TextNotification();
    }
    void TextNotification()
    {
        notificationText = gameObject.GetComponent<Text>();
        notificationText.text = notificationMessage;
        notificationColor = notificationText.color;
        fadeColor = new Color(1.0f, 0.0f, 0.5f, 0.0f);
        StartCoroutine(Rise());
    }
    void ImageNotification()
    {
        notificationImage = gameObject.GetComponent<SpriteRenderer>();
        notificationColor = notificationImage.color;
        fadeColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        StartCoroutine(RiseImage());
    }
    IEnumerator Rise()
    {
        for (int i = 0; i < 40; i++)
        {
            notificationText.color = Color.Lerp(notificationColor, fadeColor, i * 0.05f);
            transform.Translate(0.0f, 0.04f, 0.0f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator RiseImage()
    {
        for (int i = 0; i < 40; i++)
        {
            notificationImage.color = Color.Lerp(notificationColor, fadeColor, i * 0.05f);
            transform.Translate(0.0f, 0.04f, 0.0f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
