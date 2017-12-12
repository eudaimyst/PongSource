using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    GameScript gameReference;

    Vector3 movementVector;

    Vector3 nextHitPoint; //when getting the next hit point from a raycast, we store the point the ball will hit next 
    Vector3 nextHitNormal; //and the normal to determine which way the ball will move at the next collision
    Vector3 finalHitPoint; //do recursive raycasts until we find the hit point that hits the goal/paddle
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
        Debug.Log("reseting position");
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
            float paddleOffset = (transform.position.y - paddle.transform.position.y) / 5f;
            Debug.Log("verical PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.y = (movementVector.y/2 + paddleOffset) + movementVector.y * ((paddleOffset)*1.25f);
            movementVector = ReflectHorizontal(movementVector);
        } else
        {
            float paddleOffset = (transform.position.x - paddle.transform.position.x) / 5f;
            Debug.Log("horizontal PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.x = (movementVector.x/2 + paddleOffset/2) + movementVector.x * ((paddleOffset)*1.25f);
            movementVector = ReflectVertical(movementVector);
        }
    }

    void ProcessBallHit(Collision2D col) //when the ball hits the wall or paddle
    {
        /* CONTACT POINT LOGIC - this is good but it's so much logic and still inaccurate if we only have one contact point (corner hit)
        //contact points are generated for from each corner of the ball, therefore we can average them to find vector for the collision to the side of the ball

        Vector2 contactPointTotal = Vector2.zero;
        for (var i = 0; i < col.contacts.Length; i++)
        {
            Debug.DrawLine(transform.position, col.contacts[i].point, Color.magenta, 1f);
            contactPointTotal += col.contacts[i].point;
        }
        Vector3 contactPointAverage = new Vector3(contactPointTotal.x / col.contacts.Length, contactPointTotal.y / col.contacts.Length, 0f);
        Vector3 contactPointDiff = contactPointAverage - transform.position;

        Debug.DrawLine(transform.position, transform.position + (contactPointDiff * 10), Color.red, 2f);

        Debug.Log("CONTACT POINT DIFFERENCE: " + contactPointDiff);

        if (Mathf.Abs(contactPointDiff.x) > Mathf.Abs(contactPointDiff.y)) //we hit a horizontal wall (contact point is further away horizontally than vertically)
        {
            movementVector = ReflectHorizontal(movementVector);
        }
        else //we hit a vertical wall collision
        {
            movementVector = ReflectVertical(movementVector);
        }
        */

        if (col.collider.name.Contains("Vertical")) //we hit a vertical wall
        {
            movementVector = ReflectHorizontal(movementVector);
        }
        else //we hit a horizontal wall collision
        {
            movementVector = ReflectVertical(movementVector);
        }
    }

    Vector3 ReflectHorizontal(Vector3 V)
    {
        //Debug.Log("----REFLECTING HORIZONTAL");
        return new Vector3(-V.x, V.y, V.z);
    }

    Vector3 ReflectVertical(Vector3 V)
    {
        //Debug.Log("----REFLECTING VERTICAL");
        return new Vector3(V.x, -V.y, V.z);
    }

    void DoPrediction()
    {
        finalHitPoint = Vector3.zero;
        RaycastHit2D[] predictedHits = new RaycastHit2D[10];
        Vector2[] predictedMovementVectors = new Vector2[11];
        RaycastHit2D[] allColliders = new RaycastHit2D[10]; //we need to do a RaycastAll for each raycast to remove if we started inside a collider (not iterated, raycast should never pass through more than 5 colliders(?))

        int i = 0;
        while (finalHitPoint == Vector3.zero)
        {
            if (i > 9) { Debug.Log("Failed to find bounce that didnt hit wall after 10 bounces, giving up on finding final point"); return; }
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
                    finalHitPoint = predictedHits[i].point;
                    if (predictedHits[i].collider.name.Contains("1")) predictedNextGoal = 0;
                    else if (predictedHits[i].collider.name.Contains("2")) predictedNextGoal = 1;
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
        transform.position = Vector3.MoveTowards(transform.position, transform.position + movementVector, ballSpeed * Time.deltaTime);
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
            gameReference.Scored();
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
