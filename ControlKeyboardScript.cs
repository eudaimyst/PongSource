using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlKeyboardScript : MonoBehaviour {

    public PaddleScript activePaddle;

    public SettingsScript settingsScript; //get keybindings stored here

    KeyCode up;
    KeyCode down;
    KeyCode left;
    KeyCode right;
    KeyCode ability;

    bool movingUp;
    bool movingDown;
    bool movingLeft;
    bool movingRight;
    bool abilityPressed;

    float baseMovespeed = 1f; //the base movespeed we use as a multiplier for the acceleration rate
    float maxMovespeed = 1f; //the max movespeed before we use as a limiter for the acceleration rate
    Vector2 moveSpeed; //the final movespeed we pass to the paddle to do the actual movement (which then has its own max move speed variable)
    float accelerationRate; //multiply this by the base movespeed until max movespeed is reached once button is pressed
    float decelerationRate; //multiply this by the base movespeed until reaches 0 once button is released

    bool doOnce = true;

    // Use this for initialization
    void Start () {

        settingsScript = GameObject.Find("SettingsHolder").GetComponent<SettingsScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (activePaddle != null)
        {
            if (doOnce)
            {
                LoadKeyboardSettings();
                doOnce = false;
            }
            //activePaddle.MoveVertical(Input.GetAxis("Vertical"));
            //activePaddle.MoveHorizontal(Input.GetAxis("Horizontal"));

            if (Input.GetKeyDown(up))
            {
                movingUp = true;
                moveSpeed = Vector2.zero;
            }
            if (Input.GetKeyUp(up))
            {
                movingUp = false;
            }
            if (Input.GetKeyDown(down))
            {
                movingDown = true;
                moveSpeed = Vector2.zero;
            }
            if (Input.GetKeyUp(down))
            {
                movingDown = false;
            }
            if (Input.GetKeyDown(left))
            {
                movingLeft = true;
                moveSpeed = Vector2.zero;
            }
            if (Input.GetKeyUp(left))
            {
                movingLeft = false;
            }
            if (Input.GetKeyDown(right))
            {
                movingRight = true;
                moveSpeed = Vector2.zero;
            }
            if (Input.GetKeyUp(right))
            {
                movingRight = false;
            }

            if (movingUp)
            {
                if (moveSpeed.y < maxMovespeed) moveSpeed.y += accelerationRate;
            }
            else
            {
                if (moveSpeed.y > 0) moveSpeed.y -= decelerationRate;
            }

            if (movingDown)
            {
                if (moveSpeed.y > -maxMovespeed) moveSpeed.y -= accelerationRate;
            }
            else
            {
                if (moveSpeed.y < 0) moveSpeed.y += decelerationRate;
            }


            activePaddle.MoveVertical(moveSpeed.y);
            //activePaddle.MoveHorizontal(Input.GetAxis("Horizontal"));


        }
    }

    public void SetActivePaddle(PaddleScript paddleReference)
    {
        activePaddle = paddleReference;
    }

    public void LoadKeyboardSettings()
    {
        if (settingsScript != null)
        {
            /*
            up = settingsScript.keyboardControls[activePaddle.paddlePosition].up;
            down = settingsScript.keyboardControls[activePaddle.paddlePosition].down;
            left = settingsScript.keyboardControls[activePaddle.paddlePosition].left;
            right = settingsScript.keyboardControls[activePaddle.paddlePosition].right;
            ability = settingsScript.keyboardControls[activePaddle.paddlePosition].ability;
            */

            accelerationRate = settingsScript.keyboardAcceleration;
            decelerationRate = settingsScript.keyboardDeceleration;
        }
        else Debug.Log("unable to load keyboard settings in paddle");
    }

    public void SetInactive()
    {
        activePaddle = null;
    }
}
