using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour {

    public bool camera3D; //set to true if camera set to 3d, false if 2d
    public bool cameraPlayerOnBottom; //set to true if player should be on bottom of screen, also adjust controls to left/right
    public float cameraHeight; //height of 3d camera
    public float cameraFOV; //fov of 3d camera
    public bool cameraFollowBall; //3d camera target follows the ball movement
    public bool cameraSwing; //3d camera swings with paddle movement

    public class KeyboardControls
    {
        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;
        public KeyCode ability;
    }
    public KeyboardControls[] keyboard = new KeyboardControls[4];
    public int controller1;
    public int controller2;
    public int controller3;
    public int controller4;
    public float mouseSensitivity;
    public float mouseAcceleration; //
    public float keyboardAcceleration; //
    public float keyboardDeceleration; //

    public bool graphicsWindowMode; //true for windowed mode (duh)
    public int graphicsQuality; //0=low, 1=med, 2=high

    public float soundEffectsVolume;
    public float soundMusicVolume;

    public bool gameplayBallTrail;
    public bool gameplayDisableCosmetics;
    public bool gameplayColorPaddles;
    public bool gameplayMovementIndicators;
    public bool gameplayHitEffects;

    // Use this for initialization
    void Start () {
        SetDefaults();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDefaults()
    {

        camera3D = false;
        cameraPlayerOnBottom = false;
        cameraHeight = -75f;
        cameraFOV = 30f;
        cameraFollowBall = false;
        cameraSwing = false;

        controller1 = 0;
        controller2 = 3;
        controller3 = 4;
        controller4 = 5;

        mouseSensitivity = .5f;
        mouseAcceleration = .5f;
        keyboardAcceleration = 0f;
        keyboardDeceleration = 0f;

        graphicsWindowMode = false;
        graphicsQuality = 2;

        soundEffectsVolume = 100f;
        soundMusicVolume = 100f;

        gameplayBallTrail = true;
        gameplayDisableCosmetics = false;
        gameplayColorPaddles = true;
        gameplayMovementIndicators = false;
        gameplayHitEffects = false;

    }
}
