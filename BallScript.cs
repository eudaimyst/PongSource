using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    GameScript gameReference;

    Vector3 movementVector;

    float bounceMultiplier = 1.2f; //when we do a bounce we multiply the angle we bounce on by this amount (to add some variation)


    Collider2D colliderHit; //test only checking one collider instead of iterating through a list of all hit.
    bool HitTrigger; //when we hit a trigger set this bool to true, when we process trigger set the bool to false so we don't process the same trigger hit twice

    public float ballSpeed = 120f;

    public class PredictionInfo //return this class for predictions
    {
        public Vector3 finalHitPoint; //the point of the final hit
        public int bounces = 0; //number of bounces it took to find the final hit point
        public bool success; //if we found the final hit point successfully
    }

    public float GetTrailAngle()
    {
        return (Mathf.Atan2(movementVector.x, movementVector.y)* Mathf.Rad2Deg + 90);
    }

    // Use this for initialization
    void Start()
    {
    }

    public void ResetPosition()
    {
        //Debug.Log("reseting position");
        transform.position = Vector3.zero;
    }

    public void ServeTowards(Vector3 servePosition)
    {
        movementVector = Vector3.Normalize(servePosition - transform.position);
        //GetNextHitPoint();
    }

    public void Init(GameScript g)
    {
        gameReference = g;
    }

    void HitPaddle(PaddleScript paddle)
    {
        paddle.PaddleWasHit(); //we call a function on the paddle when it is hit solely so we can run a function on That paddle, which calls computer script to update its random offset
        if (paddle.movementDirection == PaddleScript.MovementDirection.vertical)
        {
            float paddleOffset = (transform.position.y - paddle.transform.position.y) / 3;
            //Debug.Log("vertical PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.y = movementVector.y + paddleOffset;
            movementVector = ReflectHorizontal(movementVector);
        }
        else
        {
            float paddleOffset = (transform.position.x - paddle.transform.position.x) / 3;
            //Debug.Log("horizontal PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.x = movementVector.x + paddleOffset;
            movementVector = ReflectVertical(movementVector);
        }
    }

    void ProcessBallHit(Collision2D col) //when the ball hits the wall or paddle
    {
        if (col.collider.name.Contains("Vertical")) //we hit a vertical wall
        {
            movementVector = Vector3.Normalize(ReflectHorizontal(movementVector));
        }
        else //we hit a horizontal wall collision
        {
            movementVector = Vector3.Normalize(ReflectVertical(movementVector));
        }
    }

    Vector3 ReflectHorizontal(Vector3 V)
    {
        //Debug.Log("----REFLECTING HORIZONTAL");
        return new Vector3(-V.x, V.y * bounceMultiplier, V.z);
    }

    Vector3 ReflectVertical(Vector3 V)
    {
        //Debug.Log("----REFLECTING VERTICAL");
        return new Vector3(V.x * bounceMultiplier, -V.y, V.z);
    }

    public PredictionInfo DoPrediction(int paddlePosition)
    {
        Color debugColor = Color.black;
        if (paddlePosition == 0) debugColor = Color.red;
        if (paddlePosition == 1) debugColor = Color.blue;

        int nextGoal = 0; //temp variable just to store the next goal we hit to check against paddle position
        RaycastHit2D[] predictedHits = new RaycastHit2D[10];
        Vector2[] predictedMovementVectors = new Vector2[11];
        RaycastHit2D[] allColliders = new RaycastHit2D[10]; //we need to do a RaycastAll for each raycast to remove if we started inside a collider (not iterated, raycast should never pass through more than 5 colliders(?))
        PredictionInfo predictionInfo = new PredictionInfo();

        for (var i = 0; i < 10; i++)
        {
            debugColor[3] = .75f - (i / 10f);
            //Debug.Log("prediction point " + i);
            //first prediction
            if (i == 0)
            {
                // bit shift the index of the layer to get a bit mask
                predictedHits[i] = Physics2D.Raycast(transform.position, movementVector, 500f, 1 << 8); // bit shift the index of the layer to get a bit mask
                Debug.DrawLine(transform.position, predictedHits[i].point, debugColor, .12f);
                predictedMovementVectors[i] = movementVector;
            }
            //all other predictions
            else
            {
                if (predictedHits[i - 1].collider != null) //if the previous iterations raycast found a collider
                {
                    if (predictedHits[i - 1].collider.name.Contains("Wall") || predictedHits[i - 1].collider.name.Contains("Prediction")) //reflect previous iterations movement vector to use for this ray
                    {
                        if (predictedHits[i - 1].collider.name.Contains("Vertical")) predictedMovementVectors[i] = ReflectHorizontal(predictedMovementVectors[i - 1]);
                        if (predictedHits[i - 1].collider.name.Contains("Horizontal")) predictedMovementVectors[i] = ReflectVertical(predictedMovementVectors[i - 1]);
                    }
                    //do raycast
                    allColliders = Physics2D.RaycastAll(predictedHits[i - 1].point, predictedMovementVectors[i], 300f, 1 << 8); // bit shift the index of the layer to get a bit mask
                    if (allColliders.Length > 1)
                    {
                        if (allColliders[0].collider == predictedHits[i - 1].collider)
                        {
                            predictedHits[i] = allColliders[1]; //we started inside the collider we hit previously so we will ignore it
                            Debug.DrawLine(predictedHits[i - 1].point, predictedHits[i].point, debugColor, .3f);
                            //Debug.Log("Prediction Bounce: " + i + " -- Hit collider name: " + predictedHits[i].collider.name);
                        }
                    }
                }

            }
            //after we have our predicted hit for this iteration, check collisions to see if prediction hits the goal
            if (predictedHits[i].collider != null)
            {
                if (predictedHits[i].collider.name.Contains("Prediction"))
                {
                    Collider2D finalCollider = predictedHits[i].collider;
                    if (finalCollider.name.Contains("1")) nextGoal = 0;
                    else if (finalCollider.name.Contains("2")) nextGoal = 1;
                    else if (finalCollider.name.Contains("3")) nextGoal = 2;
                    else if (finalCollider.name.Contains("4")) nextGoal = 3;
                    if (nextGoal == paddlePosition) //this ray collides with a goal that IS our paddles
                    {
                        predictionInfo.finalHitPoint = predictedHits[i].point;
                        predictionInfo.bounces = i;
                        predictionInfo.success = true;
                        return predictionInfo;
                    }
                    else
                    {
                        if (finalCollider.name.Contains("Prediction")) //reflect previous iterations movement vector to use for this ray
                        {
                            if (finalCollider.name.Contains("Vertical"))
                            {
                                float distance = gameReference.paddles[nextGoal].transform.position.y - predictedHits[i].point.y;
                                if (Mathf.Abs(distance) < 5f)
                                {
                                    //Debug.Log("Predicting bounce off enemy paddle #" + nextGoal + " With distance :" + distance);
                                    predictedMovementVectors[i].y = predictedMovementVectors[i].y - (distance / 3) * bounceMultiplier;
                                }
                            }
                                if (finalCollider.name.Contains("Horizontal"))
                            {

                            }
                        }
                    }
                }
            }
        }

        //end of for loop
        //Debug.Log("Failed to find bounce that didnt hit wall after 10 bounces, giving up on finding final point");
        predictionInfo.finalHitPoint = Vector3.zero;
        predictionInfo.bounces = 0;
        predictionInfo.success = false;
        return predictionInfo;
    }

    //Update is called once per frame=
    void FixedUpdate()
    {
        //draw gizmo
        Debug.DrawLine(transform.position, transform.position + movementVector * 30, Color.black);

        //move ball
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementVector * 30, ballSpeed * Time.fixedDeltaTime);
    }




    // this is Triggered collision logic

    void OnCollisionEnter2D(Collision2D col)
    {

        //we have this if because if we don't if the ball hits paddle and wall at same time it processes both movement and double reflects
        //the second part is because if we don't it will ignore double wall hits as well if they happen quick enough together
        //so we let wall hit collision logic go through if we're already processing another wall hit
        if (HitTrigger == false || (col.collider.name.Contains("Wall") && colliderHit.name.Contains("Wall")))
        {
            HitTrigger = true;
            colliderHit = col.collider;

            //Debug.Log("ENTERING " + col.collider.name);
            if (col.collider.name.Contains("Paddle"))
            {
                HitPaddle(col.gameObject.GetComponent<PaddleScript>());
                gameReference.PaddleWasHit(); //we call a function in game reference that a paddle was hit, solely so we can run a function on all paddles, that calls computer script to update prediction
            }
            if (col.collider.name.Contains("Wall"))
            {
                ProcessBallHit(col);
            }
        }
        else
        {
            //Debug.Log("IGNORED COLLISION!!!!!! ON " + col.collider.name);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (colliderHit == col.collider) HitTrigger = false;
        //Debug.Log("EXITING " + col.collider.name);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Goal"))
        {
            if (col.gameObject.name.Contains("1")) gameReference.Scored(0);
            if (col.gameObject.name.Contains("2")) gameReference.Scored(1);
            if (col.gameObject.name.Contains("3")) gameReference.Scored(2);
            if (col.gameObject.name.Contains("4")) gameReference.Scored(3);
        }
    }

}
