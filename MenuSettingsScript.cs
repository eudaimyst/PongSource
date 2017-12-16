using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsScript : MonoBehaviour {

    //attached to settings panel, handles all logic for settings menu

    public GameObject subsettingsHolder; //this is a child of the mask object, we move this up and down in relation to the scrollbar
    public Scrollbar settingsScrollbar;

    public float subsettingsTotalHeight;

    #region messy subsetting variable definitions

    public RectTransform cameraSettingsPanel;
    bool cameraSettingsMinimized;
    float cameraSettingsHeight;
    public Text cameraSettingsMinimizeText;

    public RectTransform controlsSettingsPanel;
    bool controlsSettingsMinimized;
    float controlsSettingsHeight;
    public Text controlsSettingsMinimizeText;

    public RectTransform graphicsSettingsPanel;
    bool graphicsSettingsMinimized;
    float graphicsSettingsHeight;
    public Text graphicsSettingsMinimizeText;

    public RectTransform gameplaySettingsPanel;
    bool gameplaySettingsMinimized;
    float gameplaySettingsHeight;
    public Text gameplaySettingsMinimizeText;

    public RectTransform soundSettingsPanel;
    bool soundSettingsMinimized;
    float soundSettingsHeight;
    public Text soundSettingsMinimizeText;

    public RectTransform developerSettingsPanel;
    bool developerSettingsMinimized;
    float developerSettingsHeight;
    public Text developerSettingsMinimizeText;

    #endregion

    // Use this for initialization
    void Start () {

        //set the initial heights of the subsettings so we can revert to them when we maximise those panels
        controlsSettingsHeight = controlsSettingsPanel.rect.height;
        cameraSettingsHeight = cameraSettingsPanel.rect.height;
        graphicsSettingsHeight = graphicsSettingsPanel.rect.height;
        gameplaySettingsHeight = gameplaySettingsPanel.rect.height;
        soundSettingsHeight = soundSettingsPanel.rect.height;
        developerSettingsHeight = developerSettingsPanel.rect.height;

        UpdatePositions();
        ToggleCameraSettings();
        ToggleControlsSettings();
        ToggleDeveloperSettings();
        ToggleGameplaySettings();
        ToggleGraphicsSettings();
        ToggleSoundSettings();
    }
	
	// Update is called once per frame
	void Update () {

    }

    #region Maximize Subsetting button functions (very messy/bad, got lazy)

    public void ToggleCameraSettings()
    {
        if (cameraSettingsMinimized)
        {
            //maximise settings
            cameraSettingsMinimized = false;
            cameraSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cameraSettingsHeight);
            cameraSettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            cameraSettingsMinimized = true;
            cameraSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            cameraSettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    public void ToggleControlsSettings()
    {
        if (controlsSettingsMinimized)
        {
            //maximise settings
            controlsSettingsMinimized = false;
            controlsSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, controlsSettingsHeight);
            controlsSettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            controlsSettingsMinimized = true;
            controlsSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            controlsSettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    public void ToggleGraphicsSettings()
    {
        if (graphicsSettingsMinimized)
        {
            //maximise settings
            graphicsSettingsMinimized = false;
            graphicsSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, graphicsSettingsHeight);
            graphicsSettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            graphicsSettingsMinimized = true;
            graphicsSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            graphicsSettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    public void ToggleGameplaySettings()
    {
        if (gameplaySettingsMinimized)
        {
            //maximise settings
            gameplaySettingsMinimized = false;
            gameplaySettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gameplaySettingsHeight);
            gameplaySettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            gameplaySettingsMinimized = true;
            gameplaySettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            gameplaySettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    public void ToggleSoundSettings()
    {
        if (soundSettingsMinimized)
        {
            //maximise settings
            soundSettingsMinimized = false;
            soundSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, soundSettingsHeight);
            soundSettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            soundSettingsMinimized = true;
            soundSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            soundSettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    public void ToggleDeveloperSettings()
    {
        if (developerSettingsMinimized)
        {
            //maximise settings
            developerSettingsMinimized = false;
            developerSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, developerSettingsHeight);
            developerSettingsMinimizeText.text = "-";

            UpdatePositions();
        }
        else
        {
            //minimize settings
            developerSettingsMinimized = true;
            developerSettingsPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);
            developerSettingsMinimizeText.text = "+";

            UpdatePositions();
        }
    }

    #endregion

    //called whenever we maximize any button or when settings open, moves each subsetting panel into positions based oon whether others are open or close
    void UpdatePositions()
    {
        cameraSettingsPanel.localPosition = new Vector2(0f, -5f);
        controlsSettingsPanel.localPosition = new Vector2(0f, -cameraSettingsPanel.rect.height - 10f);
        graphicsSettingsPanel.localPosition = new Vector2(0f, -cameraSettingsPanel.rect.height -controlsSettingsPanel.rect.height - 15f);
        gameplaySettingsPanel.localPosition = new Vector2(0f, -cameraSettingsPanel.rect.height -controlsSettingsPanel.rect.height -graphicsSettingsPanel.rect.height - 20f);
        soundSettingsPanel.localPosition = new Vector2(0f, -cameraSettingsPanel.rect.height -controlsSettingsPanel.rect.height -graphicsSettingsPanel.rect.height -gameplaySettingsPanel.rect.height - 25f);
        developerSettingsPanel.localPosition = new Vector2(0f, -cameraSettingsPanel.rect.height -controlsSettingsPanel.rect.height -graphicsSettingsPanel.rect.height -gameplaySettingsPanel.rect.height -soundSettingsPanel.rect.height - 30f);

        subsettingsTotalHeight = cameraSettingsPanel.rect.height + controlsSettingsPanel.rect.height + graphicsSettingsPanel.rect.height + gameplaySettingsPanel.rect.height + soundSettingsPanel.rect.height + developerSettingsPanel.rect.height + 35f;
    }

    //called when scrollbar value changes
    public void ScrollbarUpdate()
    {
        subsettingsHolder.transform.localPosition = new Vector2(-8f, settingsScrollbar.value * (subsettingsTotalHeight - 145f) + 145f);
    }
}
