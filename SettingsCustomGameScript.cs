using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCustomGameScript : MonoBehaviour {

    public int gameplayRules;

    public int slotControl1;
    public int slotControl2;
    public int slotControl3;
    public int slotControl4;

    public int rounds;
    public int pointLimit;

    public float ballSpeed;
    public float paddleSpeed;

    public float pcAggressive;
    public float pcResponsive;

    public string roomName;
    public bool roomIsPublic;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void SetDefaults()
    {

        gameplayRules = 0;
        slotControl1 = 0;
        slotControl2 = 0;
        slotControl3 = 0;
        slotControl4 = 0;

        rounds = 1;
        pointLimit = 11;

        ballSpeed = 100f;
        paddleSpeed = 100f;

        pcAggressive = 100f;
        pcResponsive = 100f;

        roomName = "noname";
        roomIsPublic = true;

    }
}
