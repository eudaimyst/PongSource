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

    Vector3 debugOffset = new Vector3(0, 0, .025f);

    public float ballTravelAngle;
    float ballSpeed = 1.3f;

    int framesSinceCollision; //use this counter to prevent processing immediate collisions straight after another collision

    // Use this for initialization
    void Start () {
	}

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }

    public void ServeTowards(Vector3 servePosition)
    {
        movementVector = Vector3.Normalize(servePosition - transform.position) / 10;
        GetNextHitPoint();
    }

    public void Init (GameScript g)
    {
        gameReference = g;
    }

    void HitPaddle(PaddleScript paddle)
    {
        if (paddle.movementDirection == PaddleScript.MovementDirection.vertical)
        {
            float paddleOffset = transform.position.z - paddle.transform.position.z;
            Debug.Log("PADDLE to ball OFFSET: " + paddleOffset);
            movementVector.z = movementVector.z / 2 + paddleOffset / 2 + movementVector.z * paddleOffset;
        }
    }

    void HitWall()
    {
        //ReflectVertical();
    }

    void ProcessBallHit(Collider col) //when the ball hits the wall or paddle
    {
        if (nextHitNormal.x != 0) //we hit a horizontal collision
        {
            movementVector = ReflectHorizontal(movementVector);
        }
        if (nextHitNormal.z != 0) //we hit a vertical collision
        {
            movementVector = ReflectVertical(movementVector);
        }
        GetNextHitPoint();
    }

    Vector3 ReflectHorizontal(Vector3 V)
    {
        return new Vector3(-V.x, V.y, V.z);
    }

    Vector3 ReflectVertical(Vector3 V)
    {
        return new Vector3(V.x, V.y, -V.z);
    }

    void GetNextHitPoint()
    {
        RaycastHit nextHit;

        Debug.DrawLine(transform.position, transform.position + movementVector, Color.red, 2f);
        Physics.Raycast(transform.position, movementVector, out nextHit);
        nextHitNormal = nextHit.normal;
        nextHitPoint = nextHit.point;
        //Debug.Log("next hit point set to: " + nextHitPoint);
        Debug.DrawLine(transform.position, nextHitPoint, Color.magenta, 1f);


        finalHitPoint = Vector3.zero;
        RaycastHit[] predictedHits = new RaycastHit[10];
        Vector3[] predictedMovementVectors = new Vector3[10];
        int i = 0;
        while (finalHitPoint == Vector3.zero)
        {
            if (i > 9) { Debug.Log("Failed to find bounce that didnt hit wall, giving up on finding final point"); return; }
            if (i == 0)
            {
                Physics.Raycast(transform.position, movementVector, out predictedHits[i]);
                Debug.DrawLine(transform.position, predictedHits[i].point, Color.grey, .3f);

                predictedMovementVectors[i] = movementVector;
            }
            else
            {
                Physics.Raycast(predictedHits[i - 1].point, predictedMovementVectors[i], out predictedHits[i]);
                Debug.DrawLine(predictedHits[i - 1].point, predictedHits[i].point, Color.grey, .3f);
            }

            if (predictedHits[i].normal.x != 0) //predicted path hit horizontal normal
            {
                predictedMovementVectors[i+1] = ReflectHorizontal(predictedMovementVectors[i]);
            }
            if (predictedHits[i].normal.z != 0) //predicted path hit horizontal normal
            {
                predictedMovementVectors[i+1] = ReflectVertical(predictedMovementVectors[i]);
            }

            if (predictedHits[i].collider.name.Contains("Wall"))
            {
                Debug.Log("Predicted Bounce#" + i + " hit a wall");
            }
            else
            {
                if (predictedHits[i].collider.name == ("Player1Goal"))
                {
                    predictedNextGoal = 1;
                }
                if (predictedHits[i].collider.name == ("Player2Goal"))
                {
                    predictedNextGoal = 2;
                }
                finalHitPoint = predictedHits[i].point;
            }
            i++;
        }
    }

    /* Update is called once per frame=
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + movementVector);

        transform.position = Vector3.MoveTowards(transform.position, nextHitPoint, ballSpeed * Time.deltaTime);
    }*/

    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + movementVector);

        transform.position = Vector3.MoveTowards(transform.position, nextHitPoint, ballSpeed * Time.deltaTime);

        /*
        //i really don't think we need this, it's hacky and not right, errors are caused by innacurate nexthitpoint logic rather than finding the actual point we hit
        if (framesSinceCollision < 10)
        {
            //Debug.Log(framesSinceCollision);
            framesSinceCollision++;
            return;
        }*/

        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(.025f, .025f, .025f));
        for (var i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name != "Ball")
            {
                Debug.Log("HITTING " + colliders[i].name);
                framesSinceCollision = 0;
            }
            if (colliders[i].name.Contains("Paddle"))
            {
                HitPaddle(colliders[i].gameObject.GetComponent<PaddleScript>());
                ProcessBallHit(colliders[i]);
                return;
            }
            if (colliders[i].name.Contains("Wall"))
            {
                HitWall();
                ProcessBallHit(colliders[i]);
                return;
            }
            if (colliders[i].name.Contains("Goal"))
            {
                gameReference.Scored();
                return;
            }
        }
    }

    //collisions
    /*
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("COLLIDING WITH " + Physics.OverlapBox(transform.position, new Vector3(.025f, .025f, .025f)).Length + " THING(S)");
        if (Physics.OverlapBox(transform.position, new Vector3(.025f, .025f, .025f)).Length > 2)
        {
            Debug.Log("IGNORING COLLISION WITH " + col.gameObject.name);
            HitWall();
            ProcessBallHit(col);
            return;
        }
        Debug.Log("BALL enter " + col.gameObject.name + ", " + col.ClosestPoint(transform.position));
        if (col.gameObject.name.Contains("Paddle"))
        {
            HitPaddle(col.gameObject.GetComponent<PaddleScript>());
            ProcessBallHit(col);
        }
        if (col.gameObject.name.Contains("Wall"))
        {
            HitWall();
            ProcessBallHit(col);
        }
        if (col.gameObject.name.Contains("Goal"))
        {
            gameReference.Scored();
        }
    }
    */

    public Vector3 ReturnPredictedHitPoint()
    {
        return finalHitPoint;
    }
    public int ReturnPredictedNextGoal()
    {
        return predictedNextGoal;
    }
}
