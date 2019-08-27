using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexasHoldEmPlayer : MonoBehaviour {

    public Card[] pocket;

    public TexasHoldEmTable table;

    public int money;

    public int totalBet;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Card[] hand()
    {
        Card[] finalHand = new Card[2 + table.communityCards.Count];
        finalHand[0] = pocket[0];
        finalHand[1] = pocket[1];
        for (int i = 2; i < finalHand.Length; i++)
            finalHand[i] = table.communityCards[i - 2];
        Array.Sort(finalHand);
        return finalHand;
    }

    public int bet(int highbet)
    {
        return Int32.MinValue;
    }

    public void blind(int blind)
    {
        if (blind > money)
        {
            totalBet = money;
            money = 0;
        }
        else
        {
            totalBet += blind;
            money -= blind;
        }
    }

    public void reset()
    {
        pocket = new Card[2];
        totalBet = 0;
    }

    public void win(int pot)
    {
        money += pot;
    }

    /// <summary>
    /// Finds the highest hand available to the player
    /// </summary>
    /// <returns> The Enumerator corresponding to the highest available hand </returns>
    public Enums.Hands highestHand()
    {
        // Get all seven available cards in ascending order
        Card[] hand  = new Card[2 + table.communityCards.Count];
        hand[0] = pocket[0];
        hand[1] = pocket[1];
        for (int i = 2; i < table.communityCards.Count + 2; i++)
            hand[i] = table.communityCards[i - 2];
        Array.Sort(hand);
        
        bool hasRoyalFlush = false;
        bool hasStraightFlush = false;
        bool hasStraight = false;
        int straightlow = 0;
        
        /////////////////////////////
        //// CHECK FOR STRAIGHTS ////
        /////////////////////////////
        for (int i = 0; i < hand.Length - 5 /*&& j < hand.Length*/; i++)
        {
            int count = 0;
            for (int j = i + 1; j < hand.Length; j++)
            {
                if (hand[j].rank == Enums.Next(hand[j - 1].rank))
                {
                    count++;
                }
                else
                    break;

            }
            if (count >= 4)
            {
                hasStraight = true;
                straightlow = i;
            }
            else if (count == 3 && hand[straightlow].rank == Enums.Ranks.Two)
            {
                if (hand[hand.Length - 1].rank == Enums.Ranks.Ace)
                {
                    hasStraight = true;
                    straightlow = hand.Length - 1;
                }
            }
        }
        if (hasStraight)
        {
            // Check for straight flushes
            int count = 0;
            for (int i = straightlow + 1; i < straightlow + 5; i++)
            {
                if (hand[i].suit == hand[straightlow].suit)
                    count++;
                else
                    break;
            }
            if (count == 4)
                hasStraightFlush = true;

            // If there's a straight flush
            if (hasStraightFlush)
            {
                // If it's royal
                if (hand[straightlow].rank == Enums.Ranks.Ten)
                {
                    hasRoyalFlush = true;
                    return Enums.Hands.RoyalFlush;
                }
                // If not royal
                else
                    return Enums.Hands.StraightFlush;
            }
        }
        // DONE ROYAL FLUSHES
        // DONE STRAIGHT FLUSHES
        //////////////////////////////////////////////////////

        // CHECK FOR TWO, THREE, FOUR OF A KIND
        bool hasFourOfAKind = false;
        bool hasFullHouse = false;
        bool hasThreeOfAKind = false;
        bool hasTwoPair = false;
        bool hasPair = false;

        Enums.Ranks countedRank = Enums.Ranks.Blank;
        for (int i = 0; i < hand.Length; i++)
        {
            int count = 0;
            for (int j = i + 1; j < hand.Length; j++)
            {
                if (hand[i].rank == hand[j].rank && hand[i].rank != countedRank)
                    count++;
                else
                    break;
            }
            if (count == 3)
            {
                hasFourOfAKind = true;
                break;
            }
            else if (count == 2)
            {
                hasThreeOfAKind = true;
                countedRank = hand[i].rank;
            }
            else if (hasPair && count == 1)
                hasTwoPair = true;
            else if (count == 1)
                hasPair = true;
        }
        if (hasFourOfAKind)
            return Enums.Hands.FourOfAKind;
        if (hasThreeOfAKind && hasPair)
        {
            hasFullHouse = true;
            return Enums.Hands.FullHouse;
        }
        // DONE FOUR OF A KIND
        // DONE FULL HOUSE
        // CHECKED THREE OF A KIND
        // CHECKED TWO PAIR
        // CHECKED PAIR
        //////////////////////////////////////////////////////

        // CHECK FLUSH
        bool hasFlush = false;

        for (int i = 0; i < hand.Length - 5; i++)
        {
            int count = 0;
            for (int j = i + 1; j < hand.Length; j++)
            {
                if (hand[j].suit == hand[i].suit)
                    count++;
            }
            if (count >= 4)
                hasFlush = true;
        }

        // CHECK EVERYTHING ELSE
        if (hasFlush)
            return Enums.Hands.Flush;
        if (hasStraight)
            return Enums.Hands.Straight;
        if (hasThreeOfAKind)
            return Enums.Hands.ThreeOfAKind;
        if (hasTwoPair)
            return Enums.Hands.TwoPair;
        if (hasPair)
            return Enums.Hands.Pair;

        return Enums.Hands.HighCard;
    }
}
