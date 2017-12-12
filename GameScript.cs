using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    enum GamePaddles {two, four};
    enum GameState {initialising, ready, running, scored, paused, debug};
    enum GameRules {classic, testing};
    enum GamePlayerRules {single, online, local};

    struct GameStruct
    {
        public GamePaddles paddles;
        public GameState currentState;
        public GameRules rules;
        public GamePlayerRules playerRules;
    }

    private GameStruct game = new GameStruct();


    private PaddleScript[] paddles = new PaddleScript[4];
    private BallScript ball;

    // Use this for initialization
    void Start () {
        game.currentState = GameState.initialising;

        //we will pass the GameStruct values from the main menu but for now...
        game.paddles = GamePaddles.two;
        game.rules = GameRules.classic;
        game.playerRules = GamePlayerRules.single;
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
            game.currentState = GameState.ready;
        }
    }

    bool InitialiseGameState()
    {

        ball = GameObject.Find("Ball").GetComponent<BallScript>();
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

        return true;

    }

    public void Scored()
    {
        Debug.Log("SCORED");
        game.currentState = GameState.scored;
    }
}
