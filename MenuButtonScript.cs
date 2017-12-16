using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    public enum MenuOptions { None, Classic, Campaign, Modern, CustomGame, Multiplayer, Settings, Quit, BackToMainMenu };
    public MenuOptions menuOption;

    public enum SettingsToggles { none, camera2D, camera3D, cameraPlayerBottom, cameraBallFollow, cameraSwingToggle, graphicsWindowMode, gameplayBallTrail, gameplayCosmetics, gameplayColorPaddles, gameplayMovementIndicator, gameplayHitEffect};
    public SettingsToggles settingsToggle;

    public enum SettingsSliders { none, cameraHeight, cameraFOV, mouseSensitivity, mouseAcceleration, keyAcceleration, keyDeceleration, soundEffectsVolume, soundMusicVolume};
    public SettingsSliders settingsSlider;

    public enum SettingsDropdowns { none, controller1, controller2, controller3, controller4, graphicsQuality };
    public SettingsDropdowns settingsDropdown;

    Button buttonReference;

    MenuMainScript mainMenu;
    SettingsScript settingsScript;

    // Use this for initialization
    void Start () {

        mainMenu = GameObject.Find("MenuScript").GetComponent<MenuMainScript>();
        settingsScript = GameObject.Find("SettingsHolder").GetComponent<SettingsScript>();

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


    public void SliderAction(float value)
    {
        //cameraHeight, cameraFOV, mouseSensitivity, mouseAcceleration, soundEffectsVolume, soundMusicVolume
        Debug.Log("slider name: " + gameObject.name + " value: " + value);
        if (settingsSlider == SettingsSliders.cameraHeight)
        {
            settingsScript.cameraHeight = -value;
            GameObject.Find("CameraHeightValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.cameraFOV)
        {
            settingsScript.cameraFOV = value;
            GameObject.Find("CameraFOVValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.mouseSensitivity)
        {
            settingsScript.mouseSensitivity = value;
            GameObject.Find("MouseSensitivityValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.mouseAcceleration)
        {
            settingsScript.mouseAcceleration = value;
            GameObject.Find("MouseAccelerationValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.keyAcceleration)
        {
            settingsScript.keyboardAcceleration = value;
            GameObject.Find("KeyboardAccelerationValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.keyDeceleration)
        {
            settingsScript.keyboardDeceleration = value;
            GameObject.Find("KeyboardDecelerationValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.soundEffectsVolume)
        {
            settingsScript.soundEffectsVolume = value;
            GameObject.Find("EffectsVolumeValueText").GetComponent<Text>().text = value.ToString();
        }
        if (settingsSlider == SettingsSliders.soundMusicVolume)
        {
            settingsScript.soundMusicVolume = value;
            GameObject.Find("MusicVolumeValueText").GetComponent<Text>().text = value.ToString();
        }
    }

    public void ToggleAction(bool value)
    {
        //camera2D, camera3D, cameraPlayerBottom, cameraBallFollow, cameraSwingToggle, graphicsWindowMode, gameplayBallTrail, gameplayCosmetics, gameplayColorPaddles, gameplayMovementIndicator, gameplayHitEffect
        Debug.Log("toggle name: " + gameObject.name + " value: " + value);

        if (settingsToggle == SettingsToggles.camera2D)
        {
            Toggle camera3Dtoggle = GameObject.Find("3DCameraToggle").GetComponent<Toggle>();
            if (value == true)
            {
                GetComponent<Toggle>().interactable = false;
                if (camera3Dtoggle.isOn == true)
                {
                    camera3Dtoggle.isOn = false;
                    settingsScript.camera3D = false;
                    GameObject.Find("CameraPlayerBottomToggle").GetComponent<Toggle>().isOn = false;
                }
            }
            else
            {
                GetComponent<Toggle>().interactable = true;
            }
        }
        if (settingsToggle == SettingsToggles.camera3D)
        {
            Toggle camera2Dtoggle = GameObject.Find("2DCameraToggle").GetComponent<Toggle>();
            if (value == true)
            {
                GetComponent<Toggle>().interactable = false;
                if (camera2Dtoggle.isOn == true)
                {
                    camera2Dtoggle.isOn = false;
                    settingsScript.camera3D = true;
                    GameObject.Find("CameraPlayerBottomToggle").GetComponent<Toggle>().isOn = true;
                }
            }
            else
            {
                GetComponent<Toggle>().interactable = true;
            }
        }
        if (settingsToggle == SettingsToggles.cameraPlayerBottom)
        {
            settingsScript.cameraPlayerOnBottom = value;
        }
        if (settingsToggle == SettingsToggles.cameraBallFollow)
        {
            settingsScript.cameraFollowBall = value;
        }
        if (settingsToggle == SettingsToggles.cameraSwingToggle)
        {
            settingsScript.cameraSwing = value;
        }
        if (settingsToggle == SettingsToggles.graphicsWindowMode)
        {
            settingsScript.graphicsWindowMode = value;
        }
        if (settingsToggle == SettingsToggles.gameplayBallTrail)
        {
            settingsScript.gameplayBallTrail = value;
        }
        if (settingsToggle == SettingsToggles.gameplayCosmetics)
        {
            settingsScript.gameplayDisableCosmetics = value;
        }
        if (settingsToggle == SettingsToggles.gameplayColorPaddles)
        {
            settingsScript.gameplayColorPaddles = value;
        }
        if (settingsToggle == SettingsToggles.gameplayMovementIndicator)
        {
            settingsScript.gameplayMovementIndicators = value;
        }
        if (settingsToggle == SettingsToggles.gameplayHitEffect)
        {
            settingsScript.gameplayHitEffects = value;
        }
    }

    public void DropdownAction(int value)
    {
        //controller1, controller2, controller3, controller4, graphicsQuality
        Debug.Log("dropdown name: " + gameObject.name + " value: " + value);
        if (settingsDropdown == SettingsDropdowns.controller1)
        {
            settingsScript.controller1 = value;
        }
        if (settingsDropdown == SettingsDropdowns.controller2)
        {
            settingsScript.controller2 = value;
        }
        if (settingsDropdown == SettingsDropdowns.controller3)
        {
            settingsScript.controller3 = value;
        }
        if (settingsDropdown == SettingsDropdowns.controller4)
        {
            settingsScript.controller4 = value;
        }
        if (settingsDropdown == SettingsDropdowns.graphicsQuality)
        {
            settingsScript.graphicsQuality = value;
        }
    }

}
