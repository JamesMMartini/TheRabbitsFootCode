using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexasHoldEmHuman : TexasHoldEmPlayer {

    public bool isTurn;
    public GameObject AddLabel;
    public GameObject SubtractLabel;
    public GameObject AcceptLabel;
    public GameObject BetTextBox;

    public TexasHoldEmHuman()
    {
        pocket = new Card[2];
        money = 60;
        totalBet = 0;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (isTurn)
        //{
        //    Instantiate(AddLabel);
        //}

	}
}
