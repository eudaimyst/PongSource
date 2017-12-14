using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDisplayScript : MonoBehaviour {

    public BallScript physicsBall;
    public GameObject ballTrail;

	// Use this for initialization
	void Start ()
    {
        physicsBall = GameObject.Find("PhysicsBall").GetComponent<BallScript>();
        ballTrail = GameObject.Find("BallTrail");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(transform.position, physicsBall.transform.position) > 3f)
        {
            transform.position = physicsBall.transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, physicsBall.transform.position, physicsBall.ballSpeed * Time.deltaTime);
        }
        ballTrail.transform.localEulerAngles = new Vector3(180, 0, physicsBall.GetTrailAngle());
    }
}
