using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMainScript : MonoBehaviour
{

    public enum MenuStates { MainMenu, CustomGame, Multiplayer, Settings };
    public MenuStates menuState;

    GameObject mainMenuPanel;
    GameObject customGamePanel;
    GameObject multiplayerPanel;
    GameObject settingsPanel;

    // Use this for initialization
    void Start () {

        menuState = MenuStates.MainMenu;

        mainMenuPanel = GameObject.Find("MainMenuPanel");
        customGamePanel = GameObject.Find("CustomGamePanel");
        multiplayerPanel = GameObject.Find("MultiplayerPanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        customGamePanel.transform.position = mainMenuPanel.transform.position;
        multiplayerPanel.transform.position = mainMenuPanel.transform.position;
        settingsPanel.transform.position = mainMenuPanel.transform.position;


        GoToMenu(MenuStates.MainMenu);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToMenu(MenuStates menu)
    {
        mainMenuPanel.SetActive(false);
        customGamePanel.SetActive(false);
        multiplayerPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (menu == MenuStates.MainMenu)
        {
            mainMenuPanel.SetActive(true);
        }
        else if (menu == MenuStates.CustomGame)
        {
            customGamePanel.SetActive(true);
        }
        else if (menu == MenuStates.Multiplayer)
        {
            multiplayerPanel.SetActive(true);
        }
        else if (menu == MenuStates.Settings)
        {
            settingsPanel.SetActive(true);
        }
    }
}
