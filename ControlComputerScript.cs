using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlComputerScript : MonoBehaviour {

    public PaddleScript activePaddle;
    public BallScript ballReference;

    float moveFactor;

    float randomOffset;
    float randomOffsetRange = 4f; //maximum distance for random offset
    float timeSinceLastOffsetChange;
    float offsetChangeTimer = .7f; //how often to pick a new random Offset (lower is higher difficulty because less prediction possible)

    float jitterFactor = .5f; //if paddle is close enough to this point don't do movement to prevent jitter
    float maxInput = 10f;

    float timeSinceLastPrediction = 0f;
    
    Vector3 predictedHitPoint = Vector3.zero;
    BallScript.PredictionInfo predictionInfo;
    int predictionConfidence = 0; //higher is more confident

    // Use this for initialization
    void Start ()
    {
        SetNewOffset();

        ballReference = GameObject.Find("PhysicsBall").GetComponent<BallScript>();

    }
	
	// Update is called once per frame
	void Update () {

        if (activePaddle == null) return; //this script reference has no active paddle been set so don't do any computer control logic

        timeSinceLastPrediction += Time.deltaTime;
        timeSinceLastOffsetChange += Time.deltaTime;

        if (timeSinceLastOffsetChange > offsetChangeTimer)
        {
            SetNewOffset();
            timeSinceLastOffsetChange = 0f;
        }
        
        if (predictionConfidence == 0)
        {
            if (timeSinceLastPrediction > .1f)
            {
                DoPrediction();
            }
        }
        else if (predictionConfidence == 1)
        {
            if (timeSinceLastPrediction > .6f)
            {
                DoPrediction();
            }
        }
        else if (predictionConfidence == 2)
        {
            if (timeSinceLastPrediction > 1f)
            {
                DoPrediction();
            }
        }
        else if (predictionConfidence == 3)
        {
        }

        if (predictionInfo != null)
        {
            if (transform.position.y < predictedHitPoint.y - jitterFactor + randomOffset) moveFactor = maxInput;
            else if (transform.position.y > predictedHitPoint.y + jitterFactor + randomOffset) moveFactor = -maxInput;
            else if (transform.position.y > predictedHitPoint.y - jitterFactor + randomOffset && transform.position.y < predictedHitPoint.y + jitterFactor + randomOffset) moveFactor = 0f;
        }
        else //we have no predicted point so we move to y = 0, this should never trigger anymore because we never clearing predictionInfo
        {
            if (transform.position.y < -jitterFactor) moveFactor = maxInput;
            else if (transform.position.y > jitterFactor) moveFactor = -maxInput;
            else if (transform.position.y > -jitterFactor && transform.position.y < jitterFactor) moveFactor = 0f;
        }

        //Debug.Log("movement factor: " + movementFactor);
        activePaddle.MoveVertical(moveFactor);
    }

    public void DoPrediction()
    {
        if (ballReference == null || activePaddle == null) return; //can't do predictions because scripts are not initialised/can't find ball

        timeSinceLastPrediction = 0;
        predictionInfo = ballReference.DoPrediction(activePaddle.paddlePosition);
        if (predictionInfo.success)
        {
            //Debug.Log("PREDICTION SUCCESS !!!!!!!!!!!!!");

            predictedHitPoint = predictionInfo.finalHitPoint;
            Debug.DrawLine(transform.position, predictedHitPoint, Color.yellow, .2f);

            /* prediction confidence not working
            if (predictionInfo.bounces == 0) //if only one bounce and a success, do another prediction to confirm it's accuracy
            {
                if (doConfirmPrediction == true) //if we already started a confirmation request
                {
                    if (hitPointToConfirm == predictedHitPoint) predictionConfidence = 3; //prediction is the same, confidence is high, no need for more predictions
                    doConfirmPrediction = false; //remove confirmation request
                    hitPointToConfirm = Vector3.zero;
                }
                else
                {
                    doConfirmPrediction = true; //if we have not started a confirmation request, request one
                    hitPointToConfirm = predictedHitPoint;
                }

            }
            else
            {
                if (doConfirmPrediction == true)
                {
                    doConfirmPrediction = false; //if we return more than on bounce on confirmation prediction was not accurate
                    hitPointToConfirm = Vector3.zero;
                    predictionConfidence = 0;
                }
            }
            if (predictionInfo.bounces == 1) predictionConfidence = 1;
            if (predictionInfo.bounces > 3) predictionConfidence = 2;
            */
        }
        else
        {
            //Debug.Log("PREDICTION Failure !!!!!!!!!!!!!");
            if (predictionInfo.bounces > 2) predictedHitPoint = Vector3.zero;
            predictionConfidence = 0;
        }
    }

    public void SetNewOffset()
    {
        randomOffset = Random.Range(-randomOffsetRange, randomOffsetRange);
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
