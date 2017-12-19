using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
    
    public enum GameState {initialising, ready, running, scored, paused, debug};
    public enum GameRules {classic, modern, fourplayer};
    public enum PlayerType {local, online, computer};

    public struct GameStruct
    {
        public GameState currentState;
        public GameRules rules;
        public PlayerType[] playerRules;
        public int numberOfPaddles;
        public int[] scores;
        public string[] names;
    }



    public GameStruct game = new GameStruct();

    private InterfaceScript uiReference;

    int playerThatScored;
    int playerThatGotScoredOn = 5;

    public PaddleScript[] paddles = new PaddleScript[4];
    private BallScript ball;
    public GameObject settingsHolder;
    public SettingsScript settingsScript;
    public SettingsCustomGameScript customGameScript;

    public Camera camera2DRef;
    public Camera camera3DRef;

    // Use this for initialization
    void Start () {

        game.currentState = GameState.initialising;



        //first thing we do when loading into the level
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

        //we will pass the GameStruct values from the main menu but for now...
        /*
        game.numberOfPaddles = GamePaddles.two
        game.rules = GameRules.classic;
        game.playerRules = GamePlayerRules.single;
        game.scores = new int[4];
        game.names = new string[4];
        */
    }

    // Update is called once per frame
    void Update () {


        if (game.currentState == GameState.ready)
        {
            Debug.Log("SERVING!!!");
            if (playerThatGotScoredOn == 5) //first server
            {
                ball.ServeTowards(paddles[Mathf.FloorToInt(Random.Range(0f, 2f))].transform.position);
            }
            else
            {
                ball.ServeTowards(paddles[playerThatGotScoredOn].transform.position);
            }
            TriggerPaddlePrediction();
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
        game = new GameStruct();
        game.names = new string[4];
        game.scores = new int[4];

        ball = GameObject.Find("PhysicsBall").GetComponent<BallScript>();
        uiReference = GameObject.Find("Interface").GetComponent<InterfaceScript>();
        camera2DRef = GameObject.Find("TopDownCamera").GetComponent<Camera>();
        camera3DRef = GameObject.Find("PerspectiveCamera").GetComponent<Camera>();


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

        /*set paddle controllers
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
                paddles[0].SetControllerType(2); //controller type 0 = keyboard, 1 = mouse, 2 = computer
                paddles[1].SetControllerType(2);
                paddles[2].SetControllerType(2);
                paddles[3].SetControllerType(2);
            }
        }*/


        if (LoadSettings() == false)
        {
            Debug.Log("could not load settings");
            return false;
        }

        //load ui after settings to get player names
        if (uiReference == null) return false;
        else uiReference.Init(this);


        return true;

    }

    public void Scored(int goalNumber)
    {
        game.currentState = GameState.scored;
        if (game.numberOfPaddles == 2)
        {
            Debug.Log("SCORED" + goalNumber);
            if (goalNumber == 0)
            {
                playerThatScored = 1;
            }
            if (goalNumber == 1)
            {
                playerThatScored = 0;
            }
            playerThatGotScoredOn = goalNumber;
            game.scores[playerThatScored] += 1;
            uiReference.UpdateScores();
        }
        if (game.numberOfPaddles == 4)
        {
            Debug.Log("SCORED" + goalNumber);
        }
    }

    public int returnNoOfPaddles()
    {
        return game.numberOfPaddles;
    }

    public void PaddleWasHit() //when a paddle is hit this function is called from ballscript, solely so we can run a function on all paddles, that calls computer script to update prediction
    {
        TriggerPaddlePrediction();
    }

    void TriggerPaddlePrediction() //when a paddle is hit this function is called from ballscript, solely so we can run a function on all paddles, that calls computer script to update prediction
    {
        for (var i = 0; i < game.numberOfPaddles; i++)
        {
            paddles[i].DoComputerPrediction();
        }
    }

    public bool LoadSettings()
    {
        settingsHolder = GameObject.Find("SettingsHolder");
        if (settingsHolder == null)
        {
            Debug.Log("unable to find settings holder, somethings gone wrong with how this seen was loaded");
            return false;
        }
        else
        {
            settingsScript = settingsHolder.GetComponent<SettingsScript>();
            customGameScript = settingsHolder.GetComponent<SettingsCustomGameScript>();
        }


        //set player names (we have no variables/menu for this yet)
        game.names[0] = "Player1";
        game.names[1] = "Player2";
        game.names[2] = "Player3";
        game.names[3] = "Player4";


        //we check the custom game settings for gameplay rules even if not launching a custom game because i'm lazy, it's set from the main menu buttons
        if (customGameScript.gameplayRules == 0) //classic game mode
        {
            Debug.Log("CLASSIC GAME MODE LOADING");
            game.rules = GameRules.classic;
            game.numberOfPaddles = 2;

            paddles[0].SetControllerType(0); //controller type 0 = keyboard, 1 = mouse, 2 = computer
            paddles[1].SetControllerType(2);
            paddles[2].DisablePaddle();
            paddles[3].DisablePaddle();
        }
        if (customGameScript.gameplayRules == 1) //modern game mode
        {
            Debug.Log("MODERN GAME MODE LOADING");
            game.rules = GameRules.modern;
            game.numberOfPaddles = 2;

            paddles[0].SetControllerType(0); //controller type 0 = keyboard, 1 = mouse, 2 = computer
            paddles[1].SetControllerType(2);
            paddles[2].DisablePaddle();
            paddles[3].DisablePaddle();
        }
        if (customGameScript.gameplayRules == 2) //4 player mode
        {
            Debug.Log("4PLAYER GAME MODE LOADING");
            game.rules = GameRules.fourplayer;
            game.numberOfPaddles = 4;

            paddles[0].SetControllerType(0); //controller type 0 = keyboard, 1 = mouse, 2 = computer
            paddles[1].SetControllerType(0);
            paddles[2].SetControllerType(0);
            paddles[3].SetControllerType(0);
        }

        if (customGameScript.launchCustom == true) //launched from custom game menu
        {
            if (customGameScript.gameplayRules == 2) //2 player
            {
                paddles[0].SetControllerType(customGameScript.slotControl1); //controller type 0 = keyboard, 1 = mouse, 2 = computer
                paddles[1].SetControllerType(customGameScript.slotControl2);
                paddles[2].DisablePaddle();
                paddles[3].DisablePaddle();
            }
            else //4 player
            {
                paddles[0].SetControllerType(customGameScript.slotControl1); //controller type 0 = keyboard, 1 = mouse, 2 = computer
                paddles[1].SetControllerType(customGameScript.slotControl2);
                paddles[2].SetControllerType(customGameScript.slotControl3);
                paddles[3].SetControllerType(customGameScript.slotControl4);
            }
        }
        else //launched from main menu button
        {

        }

        if (settingsScript.camera3D)
        {
            camera3DRef.enabled = true;
            camera2DRef.enabled = false;
            camera2DRef.gameObject.GetComponent<FlareLayer>().enabled = false;
            camera2DRef.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            camera2DRef.enabled = true;
            camera3DRef.enabled = false;
            camera3DRef.gameObject.GetComponent<FlareLayer>().enabled = false;
            camera3DRef.gameObject.GetComponent<AudioListener>().enabled = false;
        }

        return true;
    }


}
