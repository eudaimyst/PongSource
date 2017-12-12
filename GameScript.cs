using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public enum GamePaddles {two, four};
    public enum GameState {initialising, ready, running, scored, paused, debug};
    public enum GameRules {classic, testing};
    public enum GamePlayerRules {single, online, local};

    public struct GameStruct
    {
        public GamePaddles paddles;
        public GameState currentState;
        public GameRules rules;
        public GamePlayerRules playerRules;
        public int[] scores;
        public string[] names;
    }

    public GameStruct game = new GameStruct();

    private InterfaceScript uiReference;


    private PaddleScript[] paddles = new PaddleScript[4];
    private BallScript ball;

    // Use this for initialization
    void Start () {

        game.currentState = GameState.initialising;

        //we will pass the GameStruct values from the main menu but for now...
        game.paddles = GamePaddles.two;
        game.rules = GameRules.classic;
        game.playerRules = GamePlayerRules.single;
        game.scores = new int[4];
        game.names = new string[4];
    }
	
	// Update is called once per frame
	void Update () {
		if (game.currentState == GameState.initialising)
        {
            if (InitialiseGameState() == true)
            {
                Debug.Log("game state initialisation complete");
                game.currentState = GameState.ready;
            }
            else
            {
                Debug.Log("unable to initialise");
            }
        }
        if (game.currentState == GameState.ready)
        {
            Debug.Log("SERVING!!!");
            ball.ServeTowards(paddles[1].transform.position);
            game.currentState = GameState.running;
        }
        if (game.currentState == GameState.running)
        {

        }
        if (game.currentState == GameState.scored)
        {
            ball.ResetPosition();
            for (var i = 0; i < paddles.Length; i++)
            {
                paddles[i].ResetPosition();
            }

            game.currentState = GameState.ready;
        }
    }

    bool InitialiseGameState()
    {

        ball = GameObject.Find("Ball").GetComponent<BallScript>();
        uiReference = GameObject.Find("Interface").GetComponent<InterfaceScript>();

        if (ball == null) return false;
        else ball.Init(this);

        if (GameObject.Find("Player1")) Debug.Log("found"); else Debug.Log("not found");
        for (int i = 0; i < paddles.Length; i++)
        {
            Debug.Log("searching for Paddle" + (i + 1));
            paddles[i] = GameObject.Find("Paddle" + (i + 1)).GetComponent<PaddleScript>();
            paddles[i].SetPaddlePosition(i);
            if (paddles[i] == null) return false;
        }

        if (game.paddles == GamePaddles.two)
        {
            if (game.playerRules == GamePlayerRules.single)
            {
                paddles[0].SetControllerType(2); //controller type 0 = keyboard, 1 = mouse, 2 = computer
                paddles[1].SetControllerType(2);
                paddles[2].DisablePaddle();
                paddles[3].DisablePaddle();
                game.names[0] = "Player1";
                game.names[1] = "Player2";
            }
        }
        else if (game.paddles == GamePaddles.four)
        {
            if (game.playerRules == GamePlayerRules.single)
            {
                paddles[0].SetControllerType(0); //controller type 0 = keyboard, 1 = mouse, 2 = computer
                paddles[1].SetControllerType(2);
                paddles[2].SetControllerType(2);
                paddles[3].SetControllerType(2);
            }
        }


        if (uiReference == null) return false;
        else uiReference.Init(this);

        return true;

    }

    public void Scored(int goalNumber)
    {
        game.currentState = GameState.scored;
        if (game.paddles == GamePaddles.two)
        {
            Debug.Log("SCORED" + goalNumber);
            if (goalNumber == 0)
            {
                game.scores[1] += 1;
            }
            if (goalNumber == 1)
            {
                game.scores[0] += 1;
            }
            uiReference.UpdateScores();
        }
        if (game.paddles == GamePaddles.four)
        {
            Debug.Log("SCORED" + goalNumber);
        }
    }

    public int ReturnNoOfPaddles()
    {
        if (game.paddles == GamePaddles.two) return 2;
        else return 4;
    }
}
