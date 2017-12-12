using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    GameScript gameReference;

    Vector3 movementVector;

    Vector3 nextHitPoint; //when getting the next hit point from a raycast, we store the point the ball will hit next 
    Vector3 nextHitNormal; //and the normal to determine which way the ball will move at the next collision
    Vector3 finalHitPoint; //do recursive raycasts until we find the hit point that hits the goal/paddle
    Vector3 prevfinalHitPoint; //we store the previous finalHitPoint in case we can't get a calculation for the next one
    int predictedNextGoal = 2; //used by controlcomputer based off predictions

    List<Collider2D> collisionList = new List<Collider2D>(); //we going back to collision list so we can see if we're colliding with paddle or wall specifically, and if so ignore goals
    Collider2D colliderHit; //test only checking one collider instead of iterating through a list of all hit.
    bool HitTrigger; //when we hit a trigger set this bool to true, when we process trigger set the bool to false so we don't process the same trigger hit twice
    
    float ballSpeed = 100f;

    // Use this for initialization
    void Start () {
	}

    public void ResetPosition()
    {
        //Debug.Log("reseting position");
        transform.position = Vector3.zero;
    }

    public void ServeTowards(Vector3 servePosition)
    {
        movementVector = Vector3.Normalize(servePosition - transform.position);
        DoPrediction();
        //GetNextHitPoint();
    }

    public void Init (GameScript g)
    {
        gameReference = g;
    }

    void HitPaddle(PaddleScript paddle)
    {
        if (paddle.movementDirection == PaddleScript.MovementDirection.vertical)
        {
            float paddleOffset = (transform.position.y - paddle.transform.position.y) / 3;
            //Debug.Log("vertical PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.y = movementVector.y + paddleOffset;
            movementVector = ReflectHorizontal(movementVector);
        } else
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
        return new Vector3(-V.x * .8f, V.y, V.z);
    }

    Vector3 ReflectVertical(Vector3 V)
    {
        //Debug.Log("----REFLECTING VERTICAL");
        return new Vector3(V.x, -V.y * .8f, V.z);
    }

    void DoPrediction()
    {
        if (finalHitPoint != Vector3.zero)
        {
            prevfinalHitPoint = finalHitPoint;
        }
        finalHitPoint = Vector3.zero;
        RaycastHit2D[] predictedHits = new RaycastHit2D[10];
        Vector2[] predictedMovementVectors = new Vector2[11];
        RaycastHit2D[] allColliders = new RaycastHit2D[10]; //we need to do a RaycastAll for each raycast to remove if we started inside a collider (not iterated, raycast should never pass through more than 5 colliders(?))

        int i = 0;
        while (finalHitPoint == Vector3.zero)
        {
            if (i > 9) {
                //Debug.Log("Failed to find bounce that didnt hit wall after 10 bounces, giving up on finding final point");
                finalHitPoint = prevfinalHitPoint;
                return;
            }
            if (i == 0)
            {
                // bit shift the index of the layer to get a bit mask
                predictedHits[i] = Physics2D.Raycast(transform.position, movementVector, 500f, 1 << 8); // bit shift the index of the layer to get a bit mask
                Debug.DrawLine(transform.position, predictedHits[i].point, Color.grey, .5f);
                predictedMovementVectors[i] = movementVector;
            }
            else
            {
                allColliders = Physics2D.RaycastAll(predictedHits[i - 1].point, predictedMovementVectors[i], 300f, 1 << 8); // bit shift the index of the layer to get a bit mask
                if (allColliders.Length > 1)
                {
                    if (allColliders[0].collider == predictedHits[i - 1].collider) predictedHits[i] = allColliders[1]; //we started inside the collider we hit previously so we will ignore it
                    else predictedHits[i] = allColliders[0];
                    Debug.DrawLine(predictedHits[i - 1].point, predictedHits[i].point, Color.grey, .5f);
                    //Debug.Log("Prediction Bounce: " + i + " -- Hit collider name: " + predictedHits[i].collider.name);
                }
            }

            if (predictedHits[i].collider != null)
            {
                if (predictedHits[i].collider.name.Contains("Vertical")) //predicted ray hit a vertical wall
                {
                    predictedMovementVectors[i + 1] = ReflectHorizontal(predictedMovementVectors[i]);
                }
                if (predictedHits[i].collider.name.Contains("Horizontal")) //predicted ray hit Horizontal wall
                {
                    predictedMovementVectors[i + 1] = ReflectVertical(predictedMovementVectors[i]);
                }
                if (predictedHits[i].collider.name.Contains("Goal"))
                {

                    if (gameReference.ReturnNoOfPaddles() == 2)
                    {
                        if (predictedHits[i].collider.name.Contains("1"))
                        {
                            finalHitPoint = predictedHits[i].point;
                            predictedNextGoal = 0;
                        }
                        else if (predictedHits[i].collider.name.Contains("2"))
                        {
                            finalHitPoint = predictedHits[i].point;
                            predictedNextGoal = 1;
                        }
                    }
                    else
                    {
                        finalHitPoint = predictedHits[i].point;
                        if (predictedHits[i].collider.name.Contains("1")) predictedNextGoal = 0;
                        else if (predictedHits[i].collider.name.Contains("2")) predictedNextGoal = 1;
                        else if (predictedHits[i].collider.name.Contains("3")) predictedNextGoal = 3;
                        else if (predictedHits[i].collider.name.Contains("4")) predictedNextGoal = 4;
                    }
                }
            }
            i++;
        }
    }

    //Update is called once per frame=
    void FixedUpdate()
    {
        //draw gizmo
        Debug.DrawLine(transform.position, transform.position + movementVector * 30, Color.blue);

        //move ball
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementVector * 30, ballSpeed * Time.fixedDeltaTime);
    }

    
    

    // this is Triggered collision logic

    void OnCollisionEnter2D(Collision2D col)
    {
        if (HitTrigger == false || (col.collider.name.Contains("Wall") && colliderHit.name.Contains("Wall") ))
        {
            HitTrigger = true;
            colliderHit = col.collider;

            //Debug.Log("ENTERING " + col.collider.name);
            if (col.collider.name.Contains("Paddle"))
            {
                HitPaddle(col.gameObject.GetComponent<PaddleScript>());
                DoPrediction();
            }
            if (col.collider.name.Contains("Wall"))
            {
                ProcessBallHit(col);
                DoPrediction();
            }
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
    



    //these are called by computercontrolscript to get where the paddle should move to
    public Vector3 ReturnPredictedHitPoint()
    {
        return finalHitPoint;
    }
    public int ReturnPredictedNextGoal()
    {
        return predictedNextGoal;
    }


    
}
