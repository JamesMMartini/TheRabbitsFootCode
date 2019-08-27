using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexasHoldEmTable : MonoBehaviour {

    public GameObject card;

    string gameStep;
    
    bool hasMadeLabels;
    GameObject canvas;
    GameObject camera;
    Vector3 cameraPosMain;
    Vector3 cameraPosCC;
    Vector3 cameraEulerMain;
    Vector3 cameraEulerCC;
    //Quaternion cameraQuatMain;
    //Quaternion cameraQuatCC;

    public Image AddLabel, SubtractLabel, AcceptLabel, FoldLabel, FirstPocket, SecondPocket;
    public Sprite AddSprite, SubtractSprite, AcceptSprite, FoldSprite;
    public Image addInstantiated, subtractInstantiated, acceptInstantiated, foldInstantiated, firstPocketInstantiated, secondPocketInstantiated;
    public GameObject flop1Display, flop2Display, flop3Display, flop4Display, flop5Display;
    public GameObject[] cardsInstantiated;


    public Text BetTextBox, PotTextBox, MoneyTextBox;
    public Text betBoxInstantiated, potBoxInstantiated, moneyBoxInstantiated, outputText;
    public GameObject outputBox, outputBoxInstantiated;

    List<string> outputList;
    int outputBoxIndex;

    List<TexasHoldEmPlayer> players;
    List<TexasHoldEmPlayer> activePlayers;
    public List<Card> communityCards;
    Deck deck;
    public int pot;
    public int maxBet;
    int highBet;
    int bigBlind;
    int smallBlind;
    int dealer;

    int playerBetting = 0;
    int checkCount = 0;

    public TexasHoldEmTable(List<TexasHoldEmPlayer> players)
    {
        //deck = new Deck();

        //this.players = new List<TexasHoldEmPlayer>();
        //foreach (TexasHoldEmPlayer player in players)
        //    this.players.Add(player);
    }

    // Use this for initialization
    void Start()
    {
        List<TexasHoldEmPlayer> players = new List<TexasHoldEmPlayer>();
        for (int i = 0; i < 3; i++)
            players.Add(new TexasHoldEmAI());
        players.Add(new TexasHoldEmHuman());
        int bigBlind = 4;

        // Initialize a new, shuffled deck
        deck = new Deck();
        deck.shuffle();

        // Set all the instance variables
        checkCount = 0;
        pot = 0;
        maxBet = 0;
        this.bigBlind = bigBlind;
        smallBlind = bigBlind / 2;
        dealer = 0;
        communityCards = new List<Card>();

        // Copy all the players into the table
        this.players = new List<TexasHoldEmPlayer>();
        foreach (TexasHoldEmPlayer player in players)
        {
            player.table = this;
            this.players.Add(player);
        }

        canvas = GameObject.Find("Canvas");
        camera = GameObject.Find("Main Camera");
        cameraPosMain = new Vector3(-13.03f, 7.08f, -2.09f);
        cameraEulerMain = new Vector3(43.755f, 302.981f, 0.387f);
        cameraPosCC = new Vector3(-17.67f, 6.04f, 0.59f);
        cameraEulerCC = new Vector3(87.729f, 669.292f, 308.525f);

        outputBoxInstantiated = Instantiate(outputBox, new Vector3(835f, 150f, 0f), new Quaternion(0f, 0f, 0f, 0f));
        outputBoxInstantiated.transform.SetParent(canvas.transform);
        outputText = outputBoxInstantiated.transform.GetChild(1).gameObject.GetComponent<Text>();
        outputText.text = "";
        potBoxInstantiated = Instantiate(PotTextBox, new Vector3(1860f, 1020f, 0f), new Quaternion(0f, 0f, 0f, 0f));
        potBoxInstantiated.text = "$" + pot + ".00";
        potBoxInstantiated.alignment = TextAnchor.MiddleRight;
        potBoxInstantiated.transform.SetParent(canvas.transform);

        outputList = new List<string>();
        outputBoxIndex = 0;

        gameStep = "play game";
        //playGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            outputUpFive();
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            outputDownFive();
        //if (Input.GetKey(KeyCode.R) && camera.transform.position != cameraPosCC)
        //{
        //    camera.transform.position = cameraPosCC;
        //    camera.transform.eulerAngles = cameraEulerCC;
        //}
        //else if (!Input.GetKey(KeyCode.R) && camera.transform.position != cameraPosMain)
        //{
        //    camera.transform.position = cameraPosMain;
        //    camera.transform.eulerAngles = cameraEulerMain;
        //}
        if (Input.GetKey(KeyCode.R) && camera.transform.position != cameraPosCC)
        {
            camera.transform.position = cameraPosCC;
            camera.transform.eulerAngles = cameraEulerCC;
        }

        if (gameStep == "play game")
        {
            // While there's not a winner
            if (!hasWinner())
            {
                // REMOVE ANY INSTANTIATED CARDS
                Destroy(flop1Display);
                Destroy(flop2Display);
                Destroy(flop3Display);
                Destroy(flop4Display);
                Destroy(flop5Display);
                foreach (GameObject card in cardsInstantiated)
                    Destroy(card);

                // Reset all the variables
                cardsInstantiated = new GameObject[8];
                pot = 0;
                maxBet = 0;
                maxBet = bigBlind;
                checkCount = 0;
                communityCards = new List<Card>();
                deck.shuffle();
                foreach (TexasHoldEmPlayer player in players)
                {
                    player.reset();
                }

                // Deal the first round
                //StartCoroutine(deal());
                testDeal();
                if (players.Count < 3) // If there are less than three players (which means there are two players)
                {
                    players[dealer].blind(smallBlind);
                    players[nextPlayer(dealer)].blind(bigBlind);
                    highBet = bigBlind;
                    pot += smallBlind + bigBlind;
                }
                else
                {
                    players[nextPlayer(dealer)].blind(smallBlind);
                    players[nextPlayer(dealer + 1)].blind(bigBlind);
                    highBet = bigBlind;
                    pot += smallBlind + bigBlind;
                }
                updatePotBox();
                addOutputLog("Beginning the Game. Big Blind: $" + bigBlind);
                //Debug.Log("BEGINNING GAME!!!!!!");
                //Debug.Log("highBet: $" + highBet + ", bigBlind: $" + bigBlind);
                gameStep = "round";
                //round();
            }
        }
        else if (gameStep == "round")
        {
            if (pot == smallBlind + bigBlind)
            {
                //Debug.Log("FIRST ROUND OF BETTING");
                activePlayers = new List<TexasHoldEmPlayer>();
                foreach (TexasHoldEmPlayer player in players)
                {
                    Debug.Log(player.totalBet);
                    activePlayers.Add(player);
                }
                addOutputLog("First round of betting: " + activePlayers.Count + " players");
                // First Round of Betting
                //Debug.Log("Active Players:" +activePlayers.Count);
                gameStep = "betting round";
                //bettingRound(activePlayers);
            }
            else if (communityCards.Count == 0)
            {
                addOutputLog("The Flop: " + activePlayers.Count + " players left");
                //Debug.Log("THE FLOP");
                // The Flop
                checkCount = 0;
                playerBetting = 0;

                deck.dealCard(); // Dealer burns a card
                Card flop1 = deck.dealCard();
                flop1Display = Instantiate(card, CardTablePositions.FLOP_POS_1, CardTablePositions.FLOP_ROT_1);
                Renderer rend1 = flop1Display.GetComponent<Renderer>();
                rend1.material.SetTexture("_MainTex", Resources.Load<Texture2D>("card textures\\" + flop1.rank + "_" + flop1.suit));
                communityCards.Add(flop1);

                Card flop2 = deck.dealCard();
                flop2Display = Instantiate(card, CardTablePositions.FLOP_POS_2, CardTablePositions.FLOP_ROT_2);
                Renderer rend2 = flop2Display.GetComponent<Renderer>();
                rend2.material.SetTexture("_MainTex", Resources.Load<Texture2D>("card textures\\" + flop2.rank + "_" + flop2.suit));
                communityCards.Add(flop2);

                Card flop3 = deck.dealCard();
                flop3Display = Instantiate(card, CardTablePositions.FLOP_POS_3, CardTablePositions.FLOP_ROT_3);
                Renderer rend3 = flop3Display.GetComponent<Renderer>();
                rend3.material.SetTexture("_MainTex", Resources.Load<Texture2D>("card textures\\" + flop3.rank + "_" + flop3.suit));
                communityCards.Add(flop3);

                //Debug.Log("Active Players:" + activePlayers.Count);
                gameStep = "betting round";
                //bettingRound(activePlayers);
            }
            else if (communityCards.Count == 3)
            {
                addOutputLog("The Turn: " + activePlayers.Count + " players left");
                //Debug.Log("THE TURN");
                // The Turn
                checkCount = 0;
                playerBetting = 0;

                Card flop4 = deck.dealCard();
                flop4Display = Instantiate(card, CardTablePositions.FLOP_POS_4, CardTablePositions.FLOP_ROT_4);
                Renderer rend4 = flop4Display.GetComponent<Renderer>();
                rend4.material.SetTexture("_MainTex", Resources.Load<Texture2D>("card textures\\" + flop4.rank + "_" + flop4.suit));
                communityCards.Add(flop4);

                //Debug.Log("Active Players:" + activePlayers.Count);
                gameStep = "betting round";
                //bettingRound(activePlayers);
            }
            else if (communityCards.Count == 4)
            {
                addOutputLog("The River: " + activePlayers.Count + " players left");
                //Debug.Log("THE RIVER");
                // The River
                checkCount = 0;
                playerBetting = 0;

                Card flop5 = deck.dealCard();
                flop5Display = Instantiate(card, CardTablePositions.FLOP_POS_5, CardTablePositions.FLOP_ROT_5);
                Renderer rend5 = flop5Display.GetComponent<Renderer>();
                rend5.material.SetTexture("_MainTex", Resources.Load<Texture2D>("card textures\\" + flop5.rank + "_" + flop5.suit));
                communityCards.Add(flop5);

                //Debug.Log("Active Players:" + activePlayers.Count);
                gameStep = "betting round";
                //bettingRound(activePlayers);
            }
            else
            {
                addOutputLog("The Showdown");
                for (int i = 0; i < activePlayers.Count; i++)
                    addOutputLog("Player " + (i + 1) + ": " + activePlayers[i].highestHand());
                //Debug.Log("THE SHOWDOWN");
                // Showdown
                Enums.Hands highestHand = Enums.Hands.HighCard;
                int winnerIndex = 0;
                for (int i = 0; i < activePlayers.Count; i++)
                {
                    Enums.Hands hand = activePlayers[i].highestHand();
                    if (hand > highestHand)
                    {
                        highestHand = hand;
                        winnerIndex = i;
                    }
                }

                // Check for ties
                bool tieBreakerBool = false;
                for (int i = 0; i < activePlayers.Count; i++)
                {
                    Enums.Hands hand = activePlayers[i].highestHand();
                    if (hand == highestHand && i != winnerIndex)
                        tieBreakerBool = true;
                }

                // Assign winner
                if (tieBreakerBool)
                {
                    List<int> winningIndeces = new List<int>();
                    for (int i = 0; i < activePlayers.Count; i++)
                    {
                        Enums.Hands hand = activePlayers[i].highestHand();
                        if (hand == highestHand)
                            winningIndeces.Add(i);
                    }
                    List<int> finalWinners = tieBreaker(activePlayers, winningIndeces, highestHand);
                    foreach (int winner in finalWinners)
                    {
                        activePlayers[winner].win(pot / finalWinners.Count);
                        addOutputLog("Player " + (winner + 1) + " wins $" + (pot / finalWinners.Count));
                        //Debug.Log(winner + " wins $" + (pot / finalWinners.Count));
                    }
                }
                else
                {
                    players[winnerIndex].win(pot);
                    addOutputLog("Player " + (winnerIndex + 1) + " wins $" + pot);
                    //Debug.Log(winnerIndex + " wins $" + pot);
                }
                foreach (TexasHoldEmPlayer player in players)
                    if (player.money == 0)
                        players.Remove(player);
                gameStep = "play game";
            }
        }
        else if (gameStep == "betting round")
        {
            //int highBet = maxBet;
            if (checkCount < activePlayers.Count && activePlayers.Count > 1)
            {
                //for (int i = 0; i < activePlayers.Count; i++)
                //{
                //if (checkCount >= activePlayers.Count)
                //    break;
                if (activePlayers[playerBetting].GetType().Equals(typeof(TexasHoldEmAI)))
                {
                    TexasHoldEmAI tempPlayer = activePlayers[playerBetting] as TexasHoldEmAI;

                    if (tempPlayer.money != 0)
                    {
                        int bet = tempPlayer.bet(highBet);

                        string betInfo = "Player " + playerBetting;
                        if (bet == int.MinValue)
                        {
                            activePlayers.RemoveAt(playerBetting);
                            playerBetting--;
                            addOutputLog("Player " + playerBetting + " folds");
                            //foreach (TexasHoldEmPlayer player in activePlayers)
                            //    Debug.Log(player.pocket[0].rank + " " + player.pocket[0].suit + ", " + player.pocket[1].rank + " " + player.pocket[1].suit);
                        }
                        else if (bet == 0 && activePlayers[playerBetting].totalBet == highBet)
                        {
                            checkCount++;
                            betInfo += " checks";
                        }
                        else if (activePlayers[playerBetting].totalBet == highBet)
                        {
                            pot += bet;
                            updatePotBox();
                            betInfo += " calls";
                        }
                        else if (activePlayers[playerBetting].totalBet > highBet)
                        {
                            highBet = activePlayers[playerBetting].totalBet;
                            pot += bet;
                            checkCount = 0;
                            updatePotBox();
                            betInfo += " raises $" + tempPlayer.totalBet;
                        }
                        addOutputLog(betInfo);
                    }
                    else
                    {
                        addOutputLog("Player " + playerBetting + " is all in with $" + tempPlayer.totalBet);
                    }
                    //Debug.Log(betInfo);
                    if (playerBetting == activePlayers.Count - 1)
                        playerBetting = 0;
                    else
                        playerBetting++;
                }
                else
                {
                    TexasHoldEmHuman tempPlayer = activePlayers[playerBetting] as TexasHoldEmHuman;
                    if (tempPlayer.money != 0)
                    {
                        if (!hasMadeLabels)
                        {
                            // Set all the labels
                            addInstantiated = Instantiate(AddLabel, new Vector3(75f, 225f), new Quaternion(0f, 0f, 0f, 0f));
                            addInstantiated.sprite = AddSprite;
                            addInstantiated.transform.localScale -= new Vector3(0.65f, 0.65f, 0f);
                            addInstantiated.transform.SetParent(canvas.transform);

                            subtractInstantiated = Instantiate(SubtractLabel, new Vector3(75f, 70f), new Quaternion(0f, 0f, 0f, 0f));
                            subtractInstantiated.sprite = SubtractSprite;
                            subtractInstantiated.transform.localScale -= new Vector3(0.65f, 0.65f, 0f);
                            subtractInstantiated.transform.SetParent(canvas.transform);

                            acceptInstantiated = Instantiate(AcceptLabel, new Vector3(240f, 140f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            acceptInstantiated.sprite = AcceptSprite;
                            acceptInstantiated.transform.localScale -= new Vector3(0.4f, 0.4f, 0f);
                            acceptInstantiated.transform.SetParent(canvas.transform);

                            foldInstantiated = Instantiate(FoldLabel, new Vector3(230f, 225f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            foldInstantiated.sprite = FoldSprite;
                            foldInstantiated.transform.localScale -= new Vector3(0.7f, 0.7f, 0f);
                            foldInstantiated.transform.SetParent(canvas.transform);

                            betBoxInstantiated = Instantiate(BetTextBox, new Vector3(40f, 145f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            if (highBet < tempPlayer.totalBet + tempPlayer.money)
                                betBoxInstantiated.text = "$" + highBet + ".00";
                            else
                                betBoxInstantiated.text = "$" + (tempPlayer.totalBet + tempPlayer.money) + ".00";
                            betBoxInstantiated.alignment = TextAnchor.MiddleLeft;
                            betBoxInstantiated.transform.SetParent(canvas.transform);

                            moneyBoxInstantiated = Instantiate(MoneyTextBox, new Vector3(60f, 1020f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            moneyBoxInstantiated.text = "Money: $" + (tempPlayer.money + tempPlayer.totalBet) + ".00";
                            moneyBoxInstantiated.alignment = TextAnchor.MiddleLeft;
                            moneyBoxInstantiated.transform.SetParent(canvas.transform);

                            Card firstPocket = tempPlayer.pocket[0];
                            firstPocketInstantiated = Instantiate(FirstPocket, new Vector3(1525f, 150f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            firstPocketInstantiated.sprite = Resources.Load<Sprite>("Sprites\\" + firstPocket.rank + "_" + firstPocket.suit);
                            firstPocketInstantiated.transform.Rotate(Vector3.back * 90);
                            firstPocketInstantiated.transform.localScale += new Vector3(1f, 1f, 0f);
                            firstPocketInstantiated.transform.SetParent(canvas.transform);

                            Card secondPocket = tempPlayer.pocket[1];
                            secondPocketInstantiated = Instantiate(SecondPocket, new Vector3(1750f, 150f, 0f), new Quaternion(0f, 0f, 0f, 0f));
                            secondPocketInstantiated.sprite = Resources.Load<Sprite>("Sprites\\" + secondPocket.rank + "_" + secondPocket.suit);
                            secondPocketInstantiated.transform.Rotate(Vector3.back * 90);
                            secondPocketInstantiated.transform.localScale += new Vector3(1f, 1f, 0f);
                            secondPocketInstantiated.transform.SetParent(canvas.transform);

                            hasMadeLabels = true; // Indicate that the labels have been created
                        }
                        if (Input.GetKeyUp(KeyCode.W))
                        {
                            int dollarSign = 0;
                            int decPoint = betBoxInstantiated.text.IndexOf(".");
                            int bet = Convert.ToInt32(betBoxInstantiated.text.Substring(dollarSign + 1, decPoint - (dollarSign + 1)));

                            if (bet < tempPlayer.money + tempPlayer.totalBet)
                                betBoxInstantiated.text = "$" + (bet + 1) + ".00";
                        }
                        else if (Input.GetKeyUp(KeyCode.S))
                        {
                            int dollarSign = 0;
                            int decPoint = betBoxInstantiated.text.IndexOf(".");
                            int bet = Convert.ToInt32(betBoxInstantiated.text.Substring(dollarSign + 1, decPoint - (dollarSign + 1)));

                            if (bet > highBet)
                                betBoxInstantiated.text = "$" + (bet - 1) + ".00";
                        }
                        else if (Input.GetKeyUp(KeyCode.F))
                        {
                            activePlayers.RemoveAt(playerBetting);
                            playerBetting--;
                            addOutputLog("You fold");
                            //foreach (TexasHoldEmPlayer player in activePlayers)
                            //    Debug.Log(player.pocket[0].rank + " " + player.pocket[0].suit + ", " + player.pocket[1].rank + " " + player.pocket[1].suit);

                            // CLOSE ALL THE UI ELEMENTS
                            Destroy(addInstantiated);
                            Destroy(subtractInstantiated);
                            Destroy(acceptInstantiated);
                            Destroy(foldInstantiated);
                            Destroy(betBoxInstantiated);
                            Destroy(firstPocketInstantiated);
                            Destroy(secondPocketInstantiated);
                            hasMadeLabels = false;

                            if (playerBetting == activePlayers.Count - 1)
                                playerBetting = 0;
                            else
                                playerBetting++;
                        }
                        else if (Input.GetKeyUp(KeyCode.Return))
                        {
                            int dollarSign = 0;
                            int decPoint = betBoxInstantiated.text.IndexOf(".");
                            int rawBet = Convert.ToInt32(betBoxInstantiated.text.Substring(dollarSign + 1, decPoint - (dollarSign + 1)));
                            int bet = rawBet - tempPlayer.totalBet;
                            tempPlayer.totalBet += bet;
                            tempPlayer.money -= bet;

                            string betInfo = "You ";
                            if (bet == 0 && activePlayers[playerBetting].totalBet == highBet)
                            {
                                checkCount++;
                                betInfo += " check";
                            }
                            else if (activePlayers[playerBetting].totalBet == highBet)
                            {
                                pot += bet;
                                updatePotBox();
                                checkCount = 0;
                                betInfo += " call";
                            }
                            else if (activePlayers[playerBetting].totalBet > highBet)
                            {
                                highBet = activePlayers[playerBetting].totalBet;
                                pot += bet;
                                updatePotBox();
                                checkCount = 0;
                                betInfo += " raise $" + tempPlayer.totalBet;
                            }
                            else if (bet == activePlayers[playerBetting].totalBet + activePlayers[playerBetting].money && bet < highBet)
                            {
                                pot += bet;
                                updatePotBox();
                                checkCount = 0;
                                betInfo += " go all in with $" + tempPlayer.totalBet;
                            }
                            addOutputLog(betInfo);
                            //Debug.Log(betInfo);

                            // CLOSE ALL THE UI ELEMENTS
                            Destroy(addInstantiated);
                            Destroy(subtractInstantiated);
                            Destroy(acceptInstantiated);
                            Destroy(foldInstantiated);
                            Destroy(betBoxInstantiated);
                            Destroy(firstPocketInstantiated);
                            Destroy(secondPocketInstantiated);
                            Destroy(moneyBoxInstantiated);
                            hasMadeLabels = false;

                            if (playerBetting == activePlayers.Count - 1)
                                playerBetting = 0;
                            else
                                playerBetting++;
                        }
                    }
                    else
                    {
                        addOutputLog("You are all in with $" + tempPlayer.totalBet);
                        if (playerBetting == activePlayers.Count - 1)
                            playerBetting = 0;
                        else
                            playerBetting++;
                    }
                }
                //}
            }
            else if (checkCount == activePlayers.Count || activePlayers.Count == 1)
                gameStep = "round";
            //maxBet = highBet;
        }
    }

    private void updatePotBox()
    {
        potBoxInstantiated.text = "Pot: $" + pot + ".00";
    }

    List<int> tieBreaker(List<TexasHoldEmPlayer> players, List<int> winningIndices, Enums.Hands winningHand)
    {
        switch (winningHand)
        {
            case Enums.Hands.HighCard:
                {
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    Card highCard = hands[0][hands[0].Length - 1];
                    List<int> highestCards = new List<int>();
                    foreach (Card[] hand in hands)
                        if (hand[hand.Length - 1].rank >= highCard.rank)
                            highCard.rank = hand[hand.Length - 1].rank;
                    for (int i = 0; i < hands.Count; i++)
                        if (hands[i][hands[i].Length - 1].rank == highCard.rank)
                            highestCards.Add(winningIndices[i]);

                    int endIndex = hands[0].Length - 2;
                    while (highestCards.Count > 1 && endIndex >= 0)
                    {
                        highestCards = new List<int>();
                        highCard = hands[0][endIndex];
                        foreach (Card[] hand in hands)
                            if (hand[endIndex].rank >= highCard.rank)
                                highCard.rank = hand[hand.Length - 1].rank;
                        for (int i = 0; i < hands.Count; i++)
                            if (hands[i][endIndex].rank == highCard.rank)
                                highestCards.Add(winningIndices[i]);
                        endIndex--;
                    }
                    return highestCards;

                }
            case Enums.Hands.Pair:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    Enums.Ranks highPairRank = Enums.Ranks.Two;
                    // Find the highest pair of all the players
                    foreach (Card[] hand in hands)
                        for (int i = 0; i < hand.Length - 1; i++)
                            if (hand[i].rank == hand[i + 1].rank && hand[i].rank > highPairRank)
                                highPairRank = hand[i].rank;
                    // Find the players with the highest pair
                    List<int> highestPairs = new List<int>();
                    for (int ind = 0; ind < hands.Count; ind++)
                        for (int i = 0; i < hands[ind].Length - 1; i++)
                            if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == highPairRank)
                                highestPairs.Add(winningIndices[ind]);

                    Enums.Ranks highCard = hands[0][hands[0].Length - 1].rank;
                    int endIndex = hands[0].Length - 1;
                    // Cycle through all the players until you find the players with the highest hand
                    while (highestPairs.Count > 1 && endIndex >= 0)
                    {
                        highestPairs = new List<int>();
                        highCard = hands[0][endIndex].rank;
                        foreach (Card[] hand in hands)
                            if (hand[endIndex].rank >= highCard)
                                highCard = hand[hand.Length - 1].rank;
                        for (int i = 0; i < hands.Count; i++)
                            if (hands[i][endIndex].rank == highCard)
                                highestPairs.Add(winningIndices[i]);
                        endIndex--;
                    }
                    return highestPairs;
                }
            case Enums.Hands.TwoPair:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    Enums.Ranks highPairRank = Enums.Ranks.Two;
                    // Find the highest pair of all the players
                    foreach (Card[] hand in hands)
                        for (int i = 0; i < hand.Length - 1; i++)
                            if (hand[i].rank == hand[i + 1].rank && hand[i].rank > highPairRank)
                                highPairRank = hand[i].rank;

                    // Find the players with the highest pair
                    List<int> highestPairs = new List<int>();
                    for (int ind = 0; ind < hands.Count; ind++)
                        for (int i = 0; i < hands[ind].Length - 1; i++)
                            if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == highPairRank)
                                highestPairs.Add(winningIndices[ind]);

                    if (highestPairs.Count > 1) // If multiple people have the highest pair
                    {
                        Enums.Ranks secondHighPair = Enums.Ranks.Two;
                        // Find the second highest pair of all the players
                        foreach (Card[] hand in hands)
                            for (int i = 0; i < hand.Length - 1; i++)
                                if (hand[i].rank == hand[i + 1].rank && hand[i].rank > highPairRank && hand[i].rank != highPairRank)
                                    secondHighPair = hand[i].rank;

                        // Find the players with the highest pair
                        List<int> secondHighestPairs = new List<int>();
                        for (int ind = 0; ind < hands.Count; ind++)
                            for (int i = 0; i < hands[ind].Length - 1; i++)
                                if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == secondHighPair)
                                    secondHighestPairs.Add(winningIndices[ind]);

                        List<int> highCards = new List<int>();
                        Enums.Ranks highCard = hands[0][hands[0].Length - 1].rank;
                        int endIndex = hands[0].Length - 1;
                        // Cycle through all the players until you find the players with the highest hand
                        while (highCards.Count > 1 && endIndex >= 0)
                        {
                            highCards = new List<int>();
                            highCard = hands[0][endIndex].rank;
                            foreach (Card[] hand in hands)
                                if (hand[endIndex].rank >= highCard)
                                    highCard = hand[hand.Length - 1].rank;
                            for (int i = 0; i < hands.Count; i++)
                                if (hands[i][endIndex].rank == highCard)
                                    highCards.Add(winningIndices[i]);
                            endIndex--;
                        }
                        return highCards;
                    }
                    else
                    {
                        return highestPairs;
                    }
                }
            case Enums.Hands.ThreeOfAKind:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    Enums.Ranks highThreeSet = Enums.Ranks.Two;
                    // Find the highest three of a kind of all the players
                    foreach (Card[] hand in hands)
                        for (int i = 0; i < hand.Length - 2; i++)
                            if (hand[i].rank == hand[i + 1].rank && hand[i].rank == hand[i + 2].rank && hand[i].rank > highThreeSet)
                                highThreeSet = hand[i].rank;
                    // Find the players with the highest three of a kind
                    List<int> highestThreeSets = new List<int>();
                    for (int ind = 0; ind < hands.Count; ind++)
                        for (int i = 0; i < hands[ind].Length - 2; i++)
                            if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == hands[ind][i + 2].rank && hands[ind][i].rank == highThreeSet)
                                highestThreeSets.Add(winningIndices[ind]);

                    Enums.Ranks highCard = hands[0][hands[0].Length - 1].rank;
                    int endIndex = hands[0].Length - 1;
                    // Cycle through all the players until you find the players with the highest hand
                    while (highestThreeSets.Count > 1 && endIndex >= 0)
                    {
                        highestThreeSets = new List<int>();
                        highCard = hands[0][endIndex].rank;
                        foreach (Card[] hand in hands)
                            if (hand[endIndex].rank >= highCard)
                                highCard = hand[hand.Length - 1].rank;
                        for (int i = 0; i < hands.Count; i++)
                            if (hands[i][endIndex].rank == highCard)
                                highestThreeSets.Add(winningIndices[i]);
                        endIndex--;
                    }
                    return highestThreeSets;
                }
            case Enums.Hands.Straight:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    // Find the highest ranking straight in the hands
                    Enums.Ranks highCard = Enums.Ranks.Two;
                    foreach (Card[] hand in hands)
                    {
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].rank == Enums.Next(hand[j - 1].rank))
                                    count++;
                                else
                                    break;
                            }
                            if (count >= 4)
                                highCard = hand[i + 4].rank;
                            else if (count == 3 && hand[i].rank == Enums.Ranks.Two)
                                if (hand[hand.Length - 1].rank == Enums.Ranks.Ace)
                                    highCard = hand[i + 3].rank;
                        }
                    }

                    // Find all the hands with a straight equal to the highest straight
                    List<int> highStraights = new List<int>();
                    for (int player = 0; player < hands.Count; player++)
                    {
                        Card[] hand = hands[player];
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].rank == Enums.Next(hand[j - 1].rank))
                                    count++;
                                else
                                    break;
                            }
                            if (count >= 4 && hand[i + 4].rank == highCard)
                                highStraights.Add(winningIndices[player]);
                            else if (count == 3 && hand[i].rank == Enums.Ranks.Two)
                                if (hand[hand.Length - 1].rank == Enums.Ranks.Ace && highCard == Enums.Ranks.Five)
                                    highStraights.Add(winningIndices[player]);
                        }
                    }
                    return highStraights;
                }
            case Enums.Hands.Flush:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());
                    
                    // Find the highest flush card
                    Enums.Ranks flushHigh = Enums.Ranks.Two;
                    foreach (Card[] hand in hands)
                    {
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            Enums.Ranks handFlushHigh = Enums.Ranks.Two;
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].suit == hand[i].suit)
                                {
                                    handFlushHigh = hand[j].rank;
                                    count++;
                                }
                            }
                            if (count >= 4 && handFlushHigh > flushHigh)
                                flushHigh = handFlushHigh;
                        }
                    }

                    // Find everyone with a flush that matches the highest
                    List<int> highFlushes = new List<int>();
                    for (int player = 0; player < hands.Count; player++)
                    {
                        Card[] hand = hands[player];
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            Enums.Ranks handFlushHigh = Enums.Ranks.Two;
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].suit == hand[i].suit)
                                {
                                    handFlushHigh = hand[j].rank;
                                    count++;
                                }
                            }
                            if (count >= 4 && handFlushHigh == flushHigh)
                                highFlushes.Add(winningIndices[player]);
                        }
                    }

                    // Cycle through the top five cards in the players hands until the winners are found
                    int cardsChecked = 1; // The number of cards that have been checked. Stop at 5
                    while (highFlushes.Count > 1 && cardsChecked < 5)
                    {
                        highFlushes = new List<int>();
                        flushHigh = Enums.Ranks.Two;
                        foreach (Card[] hand in hands)
                        {
                            for (int i = 0; i < hand.Length - 5; i++)
                            {
                                Enums.Ranks handFlushHigh = Enums.Ranks.Two;
                                int count = 0;
                                for (int j = i + 1; j < hand.Length; j++)
                                {
                                    if (hand[j].suit == hand[i].suit && count <= 4 - cardsChecked)
                                    {
                                        handFlushHigh = hand[j].rank;
                                        count++;
                                    }
                                }
                                if (count >= 4 - cardsChecked && handFlushHigh > flushHigh)
                                    flushHigh = handFlushHigh;
                            }
                        }

                        for (int player = 0; player < hands.Count; player++)
                        {
                            Card[] hand = hands[player];
                            for (int i = 0; i < hand.Length - 5; i++)
                            {
                                Enums.Ranks handFlushHigh = Enums.Ranks.Two;
                                int count = 0;
                                for (int j = i + 1; j < hand.Length; j++)
                                {
                                    if (hand[j].suit == hand[i].suit && count <= 4 - cardsChecked)
                                    {
                                        handFlushHigh = hand[j].rank;
                                        count++;
                                    }
                                }
                                if (count >= 4 - cardsChecked && handFlushHigh == flushHigh)
                                    highFlushes.Add(winningIndices[player]);
                            }
                        }
                        cardsChecked++;
                    }
                    return highFlushes;
                }
            case Enums.Hands.FullHouse:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    // Find the highest set of three
                    Enums.Ranks highThreeSet = Enums.Ranks.Two;
                    // Find the highest three of a kind of all the players
                    foreach (Card[] hand in hands)
                        for (int i = 0; i < hand.Length - 2; i++)
                            if (hand[i].rank == hand[i + 1].rank && hand[i].rank == hand[i + 2].rank && hand[i].rank > highThreeSet)
                                highThreeSet = hand[i].rank;
                    // Find the players with the highest three of a kind
                    List<int> highestThreeSets = new List<int>();
                    List<int> highestThreeSetsHandIndeces = new List<int>();
                    for (int ind = 0; ind < hands.Count; ind++)
                        for (int i = 0; i < hands[ind].Length - 2; i++)
                            if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == hands[ind][i + 2].rank && hands[ind][i].rank == highThreeSet)
                            {
                                highestThreeSets.Add(winningIndices[ind]);
                                highestThreeSetsHandIndeces.Add(ind);
                            }

                    if (highestThreeSets.Count > 1)
                    {
                        List<int> highestPairs = new List<int>();
                        Enums.Ranks highPairRank = Enums.Ranks.Two;
                        foreach (int ind in highestThreeSetsHandIndeces)
                        {
                            Card[] hand = hands[ind];
                            // Find the highest pair among player with a full house
                            for (int i = 0; i < hand.Length - 1; i++)
                                if (hand[i].rank == hand[i + 1].rank && hand[i].rank > highPairRank)
                                    highPairRank = hand[i].rank;
                        }
                        // Find the players with the highest pair
                        foreach (int ind in highestThreeSetsHandIndeces)
                            for (int i = 0; i < hands[ind].Length - 1; i++)
                                if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == highPairRank)
                                    highestPairs.Add(winningIndices[ind]);
                        return highestPairs;
                    }
                    else
                        return highestThreeSets;
                }
            case Enums.Hands.FourOfAKind:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    // Find the highest four of a kind
                    Enums.Ranks highestFour = Enums.Ranks.Two;
                    int winningIndex = 0;
                    for (int ind = 0; ind < hands.Count; ind++)
                        for (int i = 0; i < hands[ind].Length - 4; i++)
                            if (hands[ind][i].rank == hands[ind][i + 1].rank && hands[ind][i].rank == hands[ind][i + 2].rank && 
                                hands[ind][i].rank == hands[ind][i + 3].rank && hands[ind][i].rank > highestFour)
                            {
                                highestFour = hands[ind][i].rank;
                                winningIndex = ind;
                            }
                    List<int> winner = new List<int>();
                    winner.Add(winningIndex);
                    return winner;
                }
            case Enums.Hands.StraightFlush:
                {
                    // Add all of the hand arrays to the hands list
                    List<Card[]> hands = new List<Card[]>();
                    foreach (int player in winningIndices)
                        hands.Add(players[player].hand());

                    // Find the highest ranking straight in the hands
                    Enums.Ranks highCard = Enums.Ranks.Two;
                    foreach (Card[] hand in hands)
                    {
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].rank == Enums.Next(hand[j - 1].rank) && hand[j].suit == hand[j - 1].suit)
                                    count++;
                                else
                                    break;
                            }
                            if (count >= 4)
                                highCard = hand[i + 4].rank;
                            else if (count == 3 && hand[i].rank == Enums.Ranks.Two)
                                if (hand[hand.Length - 1].rank == Enums.Ranks.Ace)
                                    highCard = hand[i + 3].rank;
                        }
                    }

                    // Find all the hands with a straight equal to the highest straight
                    List<int> highStraights = new List<int>();
                    for (int player = 0; player < hands.Count; player++)
                    {
                        Card[] hand = hands[player];
                        for (int i = 0; i < hand.Length - 5; i++)
                        {
                            int count = 0;
                            for (int j = i + 1; j < hand.Length; j++)
                            {
                                if (hand[j].rank == Enums.Next(hand[j - 1].rank) && hand[j].suit == hand[j - 1].suit)
                                    count++;
                                else
                                    break;
                            }
                            if (count >= 4 && hand[i + 4].rank == highCard)
                                highStraights.Add(winningIndices[player]);
                            else if (count == 3 && hand[i].rank == Enums.Ranks.Two)
                                if (hand[hand.Length - 1].rank == Enums.Ranks.Ace && highCard == Enums.Ranks.Five)
                                    highStraights.Add(winningIndices[player]);
                        }
                    }
                    return highStraights;
                }
            case Enums.Hands.RoyalFlush:
                {
                    return winningIndices;
                }
            default:
                return new List<int>();

        }
    }

    IEnumerator deal()
    {
        for (int i = 0; i < players.Count * 2; i++)
        {
            players[i % players.Count].pocket[i / players.Count] = deck.dealCard();
            if (i < players.Count)
            {
                if (i == 0)
                    Instantiate(card, CardTablePositions.FIRST_POS_1, CardTablePositions.FIRST_ROT_1);
                else if (i == 1)
                    Instantiate(card, CardTablePositions.SECOND_POS_1, CardTablePositions.SECOND_ROT_1);
                else if (i == 2)
                    Instantiate(card, CardTablePositions.THIRD_POS_1, CardTablePositions.THIRD_ROT_1);
                else
                    Instantiate(card, CardTablePositions.FOURTH_POS_1, CardTablePositions.FOURTH_ROT_1);
            }
            else
            {
                if (i == players.Count)
                    Instantiate(card, CardTablePositions.FIRST_POS_2, CardTablePositions.FIRST_ROT_2);
                else if (i == players.Count + 1)
                    Instantiate(card, CardTablePositions.SECOND_POS_2, CardTablePositions.SECOND_ROT_2);
                else if (i == players.Count + 2)
                    Instantiate(card, CardTablePositions.THIRD_POS_2, CardTablePositions.THIRD_ROT_2);
                else
                    Instantiate(card, CardTablePositions.FOURTH_POS_2, CardTablePositions.FOURTH_ROT_2);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void testDeal()
    {
        for (int i = 0; i < players.Count * 2; i++)
        {
            players[i % players.Count].pocket[i / players.Count] = deck.dealCard();
            if (i < players.Count)
            {
                if (i == 0)
                    cardsInstantiated[0] = Instantiate(card, CardTablePositions.FIRST_POS_1, CardTablePositions.FIRST_ROT_1);
                else if (i == 1)
                    cardsInstantiated[1] = Instantiate(card, CardTablePositions.SECOND_POS_1, CardTablePositions.SECOND_ROT_1);
                else if (i == 2)
                    cardsInstantiated[2] = Instantiate(card, CardTablePositions.THIRD_POS_1, CardTablePositions.THIRD_ROT_1);
                else
                    cardsInstantiated[3] = Instantiate(card, CardTablePositions.FOURTH_POS_1, CardTablePositions.FOURTH_ROT_1);
            }
            else
            {
                if (i == players.Count)
                    cardsInstantiated[4] = Instantiate(card, CardTablePositions.FIRST_POS_2, CardTablePositions.FIRST_ROT_2);
                else if (i == players.Count + 1)
                    cardsInstantiated[5] = Instantiate(card, CardTablePositions.SECOND_POS_2, CardTablePositions.SECOND_ROT_2);
                else if (i == players.Count + 2)
                    cardsInstantiated[6] = Instantiate(card, CardTablePositions.THIRD_POS_2, CardTablePositions.THIRD_ROT_2);
                else
                    cardsInstantiated[7] = Instantiate(card, CardTablePositions.FOURTH_POS_2, CardTablePositions.FOURTH_ROT_2);
            }
        }
    }

    bool hasWinner()
    {
        return players.Count < 2;
    }

    int nextPlayer(int pos)
    {
        if (pos < players.Count - 1)
            return pos + 1;
        else
            return pos - (players.Count - 1);
    }

    private void addOutputLog(string line)
    {
        outputList.Add(line);
        outputText.text = "";
        for (int i = outputList.Count - 5; i >= 0 && i < outputList.Count; i++)
        {
            outputText.text += outputList[i];
            outputText.text += "\n";
        }
        outputBoxIndex = outputList.Count - 5;
    }

    private void outputUpFive()
    {
        outputText.text = "";
        if (outputBoxIndex - 5 < 0)
        {
            for (int i = 0; i < 5; i++)
            {
                outputText.text += outputList[i];
                outputText.text += "\n";
            }
            outputBoxIndex = 0;
        }
        else
        {
            for (int i = outputBoxIndex - 5; i < outputBoxIndex; i++)
            {
                outputText.text += outputList[i];
                outputText.text += "\n";
            }
            outputBoxIndex = outputBoxIndex - 5;
        }
    }

    private void outputDownFive()
    {
        outputText.text = "";
        if (outputBoxIndex + 10 >= outputList.Count)
        {
            for (int i = outputList.Count - 5; i >= 0 && i < outputList.Count; i++)
            {
                outputText.text += outputList[i];
                outputText.text += "\n";
            }
            outputBoxIndex = outputList.Count - 5;
        }
        else
        {
            for (int i = outputBoxIndex + 5; i < outputBoxIndex + 10; i++)
            {
                outputText.text += outputList[i];
                outputText.text += "\n";
            }
            outputBoxIndex = outputBoxIndex + 5;
        }
    }
}
