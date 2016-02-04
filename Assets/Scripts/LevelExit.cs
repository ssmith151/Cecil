using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{

    public bool slowfade;
    public GameObject blackOverlay;
    public int newLevelIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!slowfade)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerControl>().StopPlayer(4);
                StartCoroutine(QuickFade());
            }
        } else
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerControl>().StopPlayer(4);
                StartCoroutine(SlowFade());
            }
        }
    }
    IEnumerator QuickFade()
    {
        blackOverlay.SetActive(true);
        yield return StartCoroutine(ColorFade(1));
        Application.LoadLevel(newLevelIndex);
    }
    IEnumerator SlowFade()
    {
        blackOverlay.SetActive(true);
        yield return StartCoroutine(ColorFade(10));
        Application.LoadLevel(newLevelIndex);
    }
    IEnumerator ColorFade(float speed)
    {
        for (int i = 1; i <= 60; i++)
        {
            blackOverlay.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, i / 40.0f);
            yield return new WaitForSeconds(speed*0.01f);
        }
    }

}
