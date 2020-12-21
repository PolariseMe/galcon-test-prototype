using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationFactory 
{
    public static Combination CreateKicker(List<Card> list)
    {
        return new Combination("Kicker", 0, list);
    }

    public static Combination CreatePair(List<Card> list)
    {
        return new Combination("Pair", 1, list);
    }

    public static Combination CreateTwoPairs(List<Card> list)
    {
        return new Combination("TwoPairs", 2, list);
    }

    public static Combination CreateSet(List<Card> list)
    {
        return new Combination("Set", 3, list);
    }

    public static Combination CreateStraight(List<Card> list)
    {
        return new Combination("Straight", 4, list);
    }

    public static Combination CreateFlash(List<Card> list)
    {
        return new Combination("Flash", 5, list);
    }

    public static Combination CreateFullHouse(List<Card> list)
    {
        return new Combination("FullHouse", 6, list);
    }

    public static Combination CreateCarre(List<Card> list)
    {
        return new Combination("Carre", 7, list);
    }

    public static Combination CreateStraightFlash(List<Card> list)
    {
        return new Combination("StraightFlash", 8, list);
    }

    public static Combination CreateRoyalFlash(List<Card> list)
    {
        return new Combination("RoyalFlash", 9, list);
    }


}


