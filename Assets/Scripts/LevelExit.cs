using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{

    public bool fade;
    public GameObject blackOverlay;
    public int newLevelIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!fade)
        {
            if (other.CompareTag("Player"))
                Application.LoadLevel(newLevelIndex);
        } else
        {
            StartCoroutine(SlowFade());
        }
    }
    IEnumerator SlowFade()
    {
        blackOverlay.SetActive(true);
        yield return StartCoroutine(ColorFade());
        //yield return new WaitForSeconds(2.5f);
        Application.LoadLevel(newLevelIndex);
    }
    IEnumerator ColorFade()
    {
        for (int i = 1; i <= 60; i++)
        {
            blackOverlay.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, i / 40.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
