using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    public enum MenuOptions { Classic, Campaign, Modern, CustomGame, Multiplayer, Settings, Quit, BackToMainMenu };
    public MenuOptions menuOption;

    Button buttonReference;

    MenuMainScript mainMenu;

    // Use this for initialization
    void Start () {

        mainMenu = GameObject.Find("MenuScript").GetComponent<MenuMainScript>();

        buttonReference = GetComponent<Button>();

        if (menuOption == MenuOptions.Campaign)
        {
            buttonReference.interactable = false;
        }
        else if (menuOption == MenuOptions.Modern)
        {
            buttonReference.interactable = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ButtonAction()
    {
        Debug.Log("Button pressed " + menuOption);
        if (menuOption == MenuOptions.Classic)
        {

        }
        else if (menuOption == MenuOptions.Campaign)
        {

        }
        else if (menuOption == MenuOptions.Modern)
        {

        }
        else if (menuOption == MenuOptions.CustomGame)
        {
            mainMenu.GoToMenu(MenuMainScript.MenuStates.CustomGame);
        }
        else if (menuOption == MenuOptions.Multiplayer)
        {
            mainMenu.GoToMenu(MenuMainScript.MenuStates.Multiplayer);
        }
        else if (menuOption == MenuOptions.Settings)
        {
            mainMenu.GoToMenu(MenuMainScript.MenuStates.Settings);
        }
        else if (menuOption == MenuOptions.Quit)
        {
            if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false;
            else Application.Quit();
        }
        else if (menuOption == MenuOptions.BackToMainMenu)
        {
            mainMenu.GoToMenu(MenuMainScript.MenuStates.MainMenu);
        }

    }

}
