using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlKeyboardScript : MonoBehaviour {

    public PaddleScript activePaddle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (activePaddle != null)
        {
            activePaddle.MoveVertical(Input.GetAxis("Vertical"));
            activePaddle.MoveHorizontal(Input.GetAxis("Horizontal"));
        }
    }

    public void SetActivePaddle(PaddleScript paddleReference)
    {
        activePaddle = paddleReference;
    }

    public void SetInactive()
    {
        activePaddle = null;
    }
}
