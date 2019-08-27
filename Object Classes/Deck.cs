using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {

    Card[] cards;
    int used;

    /// <summary>
    /// Creates a new deck object of 52 cards with the standard suits and ranks
    /// </summary>
    public Deck()
    {
        // Instantiate variables
        used = 0;
        cards = new Card[52];

        // Get the ranks and suits
        Enums.Suits[] tempSuits = (Enums.Suits[])Enum.GetValues(typeof(Enums.Suits));
        Enums.Ranks[] tempRanks = (Enums.Ranks[])Enum.GetValues(typeof(Enums.Ranks));

        Enums.Suits[] suits = new Enums.Suits[4];
        for (int i = 1; i < tempSuits.Length; i++)
            suits[i - 1] = tempSuits[i];
        Enums.Ranks[] ranks = new Enums.Ranks[13];
        for (int i = 1; i < tempRanks.Length; i++)
            ranks[i - 1] = tempRanks[i];

        // Go through each rank and suit and add the card to the deck
        for (int s = 0; s < 4; s++)
            for (int r = 0; r < 13; r++)
                cards[(s * 13) + r] = new Card(ranks[r], suits[s]);
    }

    /// <summary>
    /// Shuffles the deck and resets the amount of used cards.
    /// </summary>
    public void shuffle()
    {
        //Go through all the cards starting at the end
        for (int i = cards.Length - 1; i > 0; i--)
        {
            int num = (int)(UnityEngine.Random.value * (i + 1)); // Get a random card
            // Swap the cards
            Card temp = cards[i];
            cards[i] = cards[num];
            cards[num] = temp;
        }
        used = 0;
    }

    /// <summary>
    /// Deals the card in the deck
    /// </summary>
    /// <returns> The card dealt </returns>
    public Card dealCard()
    {
        return cards[used++];
    }

}
