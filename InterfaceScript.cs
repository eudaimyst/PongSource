using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{

    GameScript gameReference;

    public GameObject twoPlayerInterface;
    public GameObject fourPlayerInterface;

    public Image[] PlayerKingImages = new Image[4];
    public Text[] PlayerNames = new Text[4];
    public Text[] PlayerScores = new Text[4];

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
        if (gameReference.returnNoOfPaddles() == 2)
        {
            twoPlayerInterface.SetActive(true);
            fourPlayerInterface.SetActive(false);
        }
        if (gameReference.returnNoOfPaddles() == 4)
        {
            twoPlayerInterface.SetActive(true);
            fourPlayerInterface.SetActive(true);
        }
        for (var i = 0; i < gameReference.returnNoOfPaddles(); i++)
        {
            PlayerNames[i].text = gameReference.game.names[i].ToString();
            PlayerScores[i].text = gameReference.game.scores[i].ToString();
        }

    }
}
