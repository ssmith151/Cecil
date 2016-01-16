using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject levelMenu;
    public GameObject thisMenu;
    public GameObject border;
    public PlayerControl gc;
    public bool menuOpen;

    void OnAwake() {
        if (!levelMenu)
        {
            levelMenu = null; 
        }
        menuOpen = false;
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
        Application.LoadLevel(1);
    }
    public void LoadLevel(int levelToLoad)
    {
        Time.timeScale = 1;
         Application.LoadLevel(levelToLoad);
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
