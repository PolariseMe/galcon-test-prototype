using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination 
{
    public string Name { get; private set; }
    public int Strenght { get; private set; }
    public List<Card> Cards { get; private set; }

    public Combination(string name, int strenght, List<Card> cards)
    {
        Name = name;
        Strenght = strenght;
        Cards = cards;
    } 
}


