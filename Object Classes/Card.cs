using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : IComparable<Card> {

    public Enums.Ranks rank;
    public Enums.Suits suit;

    /// <summary>
    /// Creates a new card of the given rank and suit
    /// </summary>
    /// <param name="rank"> The rank </param>
    /// <param name="suit"> The suit </param>
	public Card(Enums.Ranks rank, Enums.Suits suit)
    {
        // Set the variables
        this.rank = rank;
        this.suit = suit;
    }

    /// <summary>
    /// Compares two cards to determine which one is greater
    /// </summary>
    /// <param name="c"> The card to compare </param>
    /// <returns> An int. Positive if greater, negative if less than, 0 if equal </returns>
    public int CompareTo(Card c)
    {        
        // Subtract the value of the c's rank from this cards rank
        return rankNum() - c.rankNum();
    }

    /// <summary>
    /// Finds the card's integer value based off of rank
    /// to help with the CompareTo method. Aces high.
    /// </summary>
    /// <returns> The card's rank as an int </returns>
    public int rankNum()
    {
        switch (rank)
        {
            case Enums.Ranks.Two:
                return 2;
            case Enums.Ranks.Three:
                return 3;
            case Enums.Ranks.Four:
                return 4;
            case Enums.Ranks.Five:
                return 5;
            case Enums.Ranks.Six:
                return 6;
            case Enums.Ranks.Seven:
                return 7;
            case Enums.Ranks.Eight:
                return 8;
            case Enums.Ranks.Nine:
                return 9;
            case Enums.Ranks.Ten:
                return 10;
            case Enums.Ranks.Jack:
                return 11;
            case Enums.Ranks.Queen:
                return 12;
            case Enums.Ranks.King:
                return 13;
            case Enums.Ranks.Ace:
                return 14;
            default:
                return Int32.MinValue;
        }
    }
}
