using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuButtonScript : MonoBehaviour {

    public enum MenuOptions { None, Classic, Campaign, Modern, CustomGame, Multiplayer, Settings, Quit, BackToMainMenu, CustomGamePlay };
    public MenuOptions menuOption;

    public enum SettingsToggles { none, camera2D, camera3D, cameraPlayerBottom, cameraBallFollow, cameraSwingToggle, graphicsWindowMode, gameplayBallTrail, gameplayCosmetics, gameplayColorPaddles, gameplayMovementIndicator, gameplayHitEffect};
    public SettingsToggles settingsToggle;

    public enum SettingsSliders { none, cameraHeight, cameraFOV, mouseSensitivity, mouseAcceleration, keyAcceleration, keyDeceleration, soundEffectsVolume, soundMusicVolume};
    public SettingsSliders settingsSlider;

    public enum SettingsDropdowns { none, controller1, controller2, controller3, controller4, graphicsQuality };
    public SettingsDropdowns settingsDropdown;

    public enum CustomGameDropdowns { none, gameplayRules, pointLimit, rounds, slotController1, slotController2, slotController3, slotController4};
    public CustomGameDropdowns customGameDropdown;

    public enum CustomGameSliders { none, ballSpeed, paddleSpeed, pcAggressive, pcResponsive}
    public CustomGameSliders customGameSlider;

    public enum CustomGameToggles { none, roomIsPublic }
    public CustomGameToggles customGameToggles;

    public enum CustomGameInput { none, roomName }
    public CustomGameInput customGameInput;

    Button buttonReference;

    MenuMainScript mainMenu;
    SettingsScript settingsScript;
    SettingsCustomGameScript settingsCustomGameScript;

    // Use this for initialization
    void Start () {

        mainMenu = GameObject.Find("MenuScript").GetComponent<MenuMainScript>();
        settingsScript = GameObject.Find("SettingsHolder").GetComponent<SettingsScript>();
        settingsCustomGameScript = GameObject.Find("SettingsHolder").GetComponent<SettingsCustomGameScript>();
        DontDestroyOnLoad(settingsScript.gameObject);

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
            settingsCustomGameScript.gameplayRules = 0;
            SceneManager.LoadScene("2DGameScene");
        }
        else if (menuOption == MenuOptions.Campaign)
        {

        }
        else if (menuOption == MenuOptions.Modern)
        {
            settingsCustomGameScript.gameplayRules = 1;
            SceneManager.LoadScene("2DGameScene");

        }
        else if (menuOption == MenuOptions.CustomGame)
        {
            mainMenu.GoToMenu(MenuMainScript.MenuStates.CustomGame);
        }
        else if (menuOption == MenuOptions.CustomGamePlay)
        {
            settingsCustomGameScript.launchCustom = true;
            SceneManager.LoadScene("2DGameScene");
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
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
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

        if (customGameSlider == CustomGameSliders.ballSpeed)
        {
            settingsCustomGameScript.ballSpeed = value;
            GameObject.Find("BallSpeedValueText").GetComponent<Text>().text = value.ToString();
        }

        if (customGameSlider == CustomGameSliders.paddleSpeed)
        {
            settingsCustomGameScript.paddleSpeed = value;
            GameObject.Find("PaddleSpeedValueText").GetComponent<Text>().text = value.ToString();
        }

        if (customGameSlider == CustomGameSliders.pcAggressive)
        {
            settingsCustomGameScript.pcAggressive = value;
            GameObject.Find("AggressiveValueText").GetComponent<Text>().text = value.ToString();
        }

        if (customGameSlider == CustomGameSliders.pcResponsive)
        {
            settingsCustomGameScript.pcResponsive = value;
            GameObject.Find("ResponsiveValueText").GetComponent<Text>().text = value.ToString();
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

        if (customGameToggles == CustomGameToggles.roomIsPublic)
        {
            settingsCustomGameScript.roomIsPublic = value;
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
        if (customGameDropdown == CustomGameDropdowns.gameplayRules)
        {
            settingsCustomGameScript.gameplayRules = value;
            if (value == 0 || value == 1)
            {
                GameObject.Find("Slot3Dropdown").GetComponent<Dropdown>().interactable = false;
                GameObject.Find("Slot4Dropdown").GetComponent<Dropdown>().interactable = false;
                GameObject.Find("PointsDropdown").GetComponent<Dropdown>().interactable = true;
                GameObject.Find("RoundsDropdown").GetComponent<Dropdown>().interactable = true;
            }
            else if (value == 2)
            {
                GameObject.Find("Slot3Dropdown").GetComponent<Dropdown>().interactable = true;
                GameObject.Find("Slot4Dropdown").GetComponent<Dropdown>().interactable = true;
                GameObject.Find("PointsDropdown").GetComponent<Dropdown>().interactable = false;
                GameObject.Find("PointsDropdown").GetComponent<Dropdown>().value = 5;
                GameObject.Find("RoundsDropdown").GetComponent<Dropdown>().interactable = false;
            }
        }
        if (customGameDropdown == CustomGameDropdowns.pointLimit)
        {
            settingsCustomGameScript.pointLimit = value;
            if (value == 5) //infinite point limit
            {
                GameObject.Find("RoundsDropdown").GetComponent<Dropdown>().interactable = false;
            }
            else
            {
                GameObject.Find("RoundsDropdown").GetComponent<Dropdown>().interactable = true;
            }
        }
        if (customGameDropdown == CustomGameDropdowns.rounds)
        {
            settingsCustomGameScript.rounds = value;
        }
        if (customGameDropdown == CustomGameDropdowns.slotController1)
        {
            settingsCustomGameScript.slotControl1 = value;
            DisableComputerOrMultiplayerInput();
        }
        if (customGameDropdown == CustomGameDropdowns.slotController2)
        {
            settingsCustomGameScript.slotControl2 = value;
            DisableComputerOrMultiplayerInput();
        }
        if (customGameDropdown == CustomGameDropdowns.slotController3)
        {
            settingsCustomGameScript.slotControl3 = value;
            DisableComputerOrMultiplayerInput();
        }
        if (customGameDropdown == CustomGameDropdowns.slotController4)
        {
            settingsCustomGameScript.slotControl4 = value;
            DisableComputerOrMultiplayerInput();
        }
    }

    public void InputAction(string value)
    {
        if (customGameInput == CustomGameInput.roomName)
        {
            settingsCustomGameScript.roomName = value;
        }
    }

    public void DisableComputerOrMultiplayerInput() //used for custom game settings to disable name input or sliders for computer settings if they're not in active slots
    {
        if (GameObject.Find("Slot1Dropdown").GetComponent<Dropdown>().value == 1 || GameObject.Find("Slot2Dropdown").GetComponent<Dropdown>().value == 1 || GameObject.Find("Slot3Dropdown").GetComponent<Dropdown>().value == 1 || GameObject.Find("Slot4Dropdown").GetComponent<Dropdown>().value == 1)
        {
            Debug.Log("online settings enabled");
            GameObject.Find("RoomNameInput").GetComponent<InputField>().interactable = true;
            GameObject.Find("RoomNameInput").GetComponent<InputField>().readOnly = false;
            GameObject.Find("PublicToggle").GetComponent<Toggle>().isOn = false;
            GameObject.Find("PublicToggle").GetComponent<Toggle>().interactable = true;
        }
        else
        {
            Debug.Log("online settings disabled");
            GameObject.Find("RoomNameInput").GetComponent<InputField>().interactable = false;
            GameObject.Find("RoomNameInput").GetComponent<InputField>().readOnly = true;
            GameObject.Find("PublicToggle").GetComponent<Toggle>().interactable = false;
        }
        if (GameObject.Find("Slot1Dropdown").GetComponent<Dropdown>().value == 2 || GameObject.Find("Slot2Dropdown").GetComponent<Dropdown>().value == 2 || GameObject.Find("Slot3Dropdown").GetComponent<Dropdown>().value == 2 || GameObject.Find("Slot4Dropdown").GetComponent<Dropdown>().value == 2)
        {
            Debug.Log("computer settings enabled");
            GameObject.Find("AggressiveSlider").GetComponent<Slider>().interactable = true;
            GameObject.Find("ResponsiveSlider").GetComponent<Slider>().interactable = true;
        }
        else
        {
            Debug.Log("computer settings disabled");
            GameObject.Find("AggressiveSlider").GetComponent<Slider>().interactable = false;
            GameObject.Find("ResponsiveSlider").GetComponent<Slider>().interactable = false;
        }
    }

}