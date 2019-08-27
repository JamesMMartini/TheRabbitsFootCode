using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TexasHoldEmAI : TexasHoldEmPlayer {

	public TexasHoldEmAI()
    {
        pocket = new Card[2];
        money = 60;
        totalBet = 0;
    }
    
    public new int bet(int highBet)
    {
        int checkNum = highBet - totalBet;
        int tempMoney = money;
        if (checkNum > money)
            return int.MinValue;

        Enums.Hands hand = highestHand();

        switch (hand)
        {
            case Enums.Hands.HighCard:
                {
                    // CALL
                    if (tempMoney > (checkNum * 2))
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.Pair:
                {
                    // CALL
                    if (tempMoney > checkNum * 2)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.TwoPair:
                {
                    // RAISE
                    if (tempMoney > checkNum * 2)
                    {
                        totalBet += checkNum + (tempMoney / 4);
                        money -= (checkNum + (tempMoney / 4));
                        return checkNum + (tempMoney / 4);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.ThreeOfAKind:
                {
                    // RAISE
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.5)
                    {
                        totalBet += checkNum + (tempMoney / 4);
                        money -= (checkNum + (tempMoney / 4));
                        return checkNum + (tempMoney / 4);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.Straight:
                {
                    // RAISE
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.75)
                    {
                        totalBet += checkNum + (tempMoney / 3);
                        money -= (checkNum + (tempMoney / 3));
                        return checkNum + (tempMoney / 3);
                    }
                    else if (tempMoney > Convert.ToDouble(checkNum) * 1.5)
                    {
                        totalBet += checkNum + (tempMoney / 5);
                        money -= (checkNum + (tempMoney / 5));
                        return checkNum + (tempMoney / 5);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.Flush:
                {
                    // RAISE
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.9)
                    {
                        totalBet += checkNum + (tempMoney / 2);
                        money -= (checkNum + (tempMoney / 2));
                        return checkNum + (tempMoney / 2);
                    }
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.75)
                    {
                        totalBet += checkNum + (tempMoney / 3);
                        money -= (checkNum + (tempMoney / 3));
                        return checkNum + (tempMoney / 3);
                    }
                    else if (tempMoney > Convert.ToDouble(checkNum) * 1.5)
                    {
                        totalBet += checkNum + (tempMoney / 5);
                        money -= (checkNum + (tempMoney / 5));
                        return checkNum + (tempMoney / 5);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.FullHouse:
                {
                    // RAISE
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.9)
                    {
                        totalBet += checkNum + (tempMoney / 2);
                        money -= (checkNum + (tempMoney / 2));
                        return checkNum + (tempMoney / 2);
                    }
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.75)
                    {
                        totalBet += checkNum + (tempMoney / 3);
                        money -= (checkNum + (tempMoney / 3));
                        return checkNum + (tempMoney / 3);
                    }
                    else if (tempMoney > Convert.ToDouble(checkNum) * 1.5)
                    {
                        totalBet += checkNum + (tempMoney / 5);
                        money -= (checkNum + (tempMoney / 5));
                        return checkNum + (tempMoney / 5);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.FourOfAKind:
                {
                    // RAISE
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.9)
                    {
                        totalBet += checkNum + (tempMoney * 3) / 4;
                        money -= (checkNum + ((tempMoney * 3) / 4));
                        return checkNum + ((tempMoney * 3) / 4);
                    }
                    if (tempMoney > Convert.ToDouble(checkNum) * 1.75)
                    {
                        totalBet += checkNum + (tempMoney / 2);
                        money -= (checkNum + (tempMoney / 2));
                        return checkNum + (tempMoney / 2);
                    }
                    else if (tempMoney > Convert.ToDouble(checkNum) * 1.5)
                    {
                        totalBet += checkNum + ((tempMoney * 4) / 10);
                        money -= (checkNum + ((tempMoney * 4) / 10));
                        return checkNum + ((tempMoney * 4) / 10);
                    }
                    // CALL
                    else if (tempMoney > checkNum)
                    {
                        totalBet += checkNum;
                        money -= checkNum;
                        return checkNum;
                    }
                    // CHECK
                    else if (checkNum == 0)
                    {
                        return 0;
                    }
                    // FOLD
                    else
                        return int.MinValue;
                }
            case Enums.Hands.StraightFlush:
                {
                    // ALL-IN
                    totalBet += tempMoney;
                    money = 0;
                    return tempMoney;
                }
            case Enums.Hands.RoyalFlush:
                {
                    // ALL-IN
                    totalBet += tempMoney;
                    money = 0;
                    return tempMoney;
                }
            default:
                {
                    return int.MinValue;
                }
        }
    }

}
