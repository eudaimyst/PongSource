using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlComputerScript : MonoBehaviour {

    public PaddleScript activePaddle;
    public BallScript ballReference;

    float moveFactor;

    float randomOffset;

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
            randomOffset = Random.Range(-.1f, .1f);
            predictedNextGoal = ballReference.ReturnPredictedNextGoal();
        }

        if (predictedNextGoal == activePaddle.paddlePosition+1)
        {
            //Debug.Log(transform.position + ", " + ballReference.ReturnNextHitPoint());
            if (predictedHitPoint.z < transform.position.z + randomOffset)
            {
                moveFactor = -.4f;
            }
            if (predictedHitPoint.z > transform.position.z + randomOffset)
            {
                moveFactor = +.4f;
            }
        }
        else
        {
            if (transform.position.z < 0)
            {
                moveFactor = +.4f;
            }
            if (transform.position.z > 0)
            {
                moveFactor = -.4f;
            }
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
