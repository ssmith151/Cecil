using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject levelMenu;
    public GameObject thisMenu;
    public GameObject border;
    public GameObject blackOverlay;
    public PlayerControl gc;
    public bool menuOpen;

    void OnAwake() {
        if (!levelMenu)
        {
            levelMenu = null; 
        }
        menuOpen = false;
    }
    void Start()
    {
        StartCoroutine(Fader(true));
    }
    IEnumerator Fader(bool darker)
    {
        if (darker)
        {
            blackOverlay.SetActive(true);
            yield return StartCoroutine(ColorFadeIn(0.5f));
        } else
        {
            blackOverlay.SetActive(true);
            yield return StartCoroutine(ColorFadeOut(0.5f));
        }
    }
    IEnumerator ColorFadeOut(float speed)
    {
        for (int i = 1; i <= 60; i++)
        {
            blackOverlay.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, i / 40.0f);
            yield return new WaitForSeconds(speed * 0.05f);
        }
    }
    IEnumerator ColorFadeIn(float speed)
    {
        for (int i = 1; i <= 60; i++)
        {
            blackOverlay.GetComponent<Image>().color = Color.Lerp(Color.black, Color.clear, i / 40.0f);
            yield return new WaitForSeconds(speed * 0.05f);
        }
        blackOverlay.SetActive(false);
    }
    public void OnBackToMain()
    {
        thisMenu.SetActive(false);
        mainMenu.SetActive(true);
        UpdateMenuStatus();
    }
    private void UpdateMenuStatus()
    {
        if (mainMenu.activeSelf)
            menuOpen = true;
        else if (optionsMenu.activeSelf)
            menuOpen = true;
        else
            menuOpen = false;
    }
    public void OnOpenOptions()
    {
        thisMenu = optionsMenu;
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void OnOpenLevelSelect ()
    {
        thisMenu = levelMenu;
        levelMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void OnStartGame()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        StartCoroutine(Fader(false));
        StartCoroutine(DelayLoad(1));
    }
    public void LoadLevel(int levelToLoad)
    {
        Time.timeScale = 1;
        StartCoroutine(Fader(false));
        StartCoroutine(DelayLoad(levelToLoad));
    }
    IEnumerator DelayLoad(int levelNo)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(levelNo);
        //Application.LoadLevel(levelNo);
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
    public void OnInGameMenuOpen()
    {
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        border.SetActive(true);
        UpdateMenuStatus();
    }
    public void OnInGameMenuClose()
    {
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        border.SetActive(false);
        UpdateMenuStatus();
    }
}
