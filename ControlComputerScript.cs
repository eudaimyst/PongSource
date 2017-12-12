﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlComputerScript : MonoBehaviour {

    public PaddleScript activePaddle;
    public BallScript ballReference;

    float moveFactor;

    float randomOffset;
    float jitterFactor = 1f; //if paddle is close enough to this point don't do movement to prevent jitter

    Vector3 predictedHitPoint;
    int predictedNextGoal;
    // Use this for initialization
    void Start () {

        ballReference = GameObject.Find("Ball").GetComponent<BallScript>();

    }
	
	// Update is called once per frame
	void Update () {

        if (ballReference.ReturnPredictedHitPoint() != predictedHitPoint)
        {
            predictedHitPoint = ballReference.ReturnPredictedHitPoint();
            randomOffset = Random.Range(-2.5f, 2.5f);
            predictedNextGoal = ballReference.ReturnPredictedNextGoal();
        }

        if (predictedNextGoal == activePaddle.paddlePosition)
        {
            if (transform.position.y < predictedHitPoint.y - jitterFactor + randomOffset) moveFactor = +.4f;
            else if (transform.position.y > predictedHitPoint.y + jitterFactor + randomOffset) moveFactor = -.4f;
            else if (transform.position.y > predictedHitPoint.y - jitterFactor + randomOffset && transform.position.y < predictedHitPoint.y + jitterFactor + randomOffset) moveFactor = 0f;
        }
        else
        {
            if (transform.position.y < -jitterFactor) moveFactor = +.4f;
            else if (transform.position.y > jitterFactor) moveFactor = -.4f;
            else if (transform.position.y > -jitterFactor && transform.position.y < jitterFactor) moveFactor = 0f;
        }

        //Debug.Log("movement factor: " + movementFactor);
        activePaddle.MoveVertical(moveFactor);
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