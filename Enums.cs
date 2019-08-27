using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums {

    public enum Ranks { Blank, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

    public enum Suits { Blank, Hearts, Diamonds, Clubs, Spades };

    public enum Hands { HighCard, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush };

    public static Ranks Next(Ranks rank)
    {
        switch (rank)
        {
            case Ranks.Two:
                return Ranks.Three;
            case Ranks.Three:
                return Ranks.Four;
            case Ranks.Four:
                return Ranks.Five;
            case Ranks.Five:
                return Ranks.Six;
            case Ranks.Six:
                return Ranks.Seven;
            case Ranks.Seven:
                return Ranks.Eight;
            case Ranks.Eight:
                return Ranks.Nine;
            case Ranks.Nine:
                return Ranks.Ten;
            case Ranks.Ten:
                return Ranks.Jack;
            case Ranks.Jack:
                return Ranks.Queen;
            case Ranks.Queen:
                return Ranks.King;
            case Ranks.King:
                return Ranks.Ace;
            default:
                return Ranks.Blank;
        }
    }

}
