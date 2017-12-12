using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{

    GameScript gameReference;

    public Text Player1Name;
    public Text Player1Score;

    public Text Player2Name;
    public Text Player2Score;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameScript g)
    {
        gameReference = g;
        UpdateScores();
    }

    public void UpdateScores()
    {
        Player1Name.text = gameReference.game.names[0].ToString();
        Player2Name.text = gameReference.game.names[1].ToString();
        Player1Score.text = gameReference.game.scores[0].ToString();
        Player2Score.text = gameReference.game.scores[1].ToString();

    }
}
