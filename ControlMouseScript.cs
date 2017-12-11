using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMouseScript : MonoBehaviour {

    public PaddleScript activePaddle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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
