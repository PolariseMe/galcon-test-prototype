using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreLinq;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static public GameController S;

    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform deck;
    [SerializeField] private TextMesh scoreBoard;

    public List<Sprite> images;
    public List<string> suits;
    private int imageId = 0;
    private int cardId = 0;

    [Header("TEST FUNCTIONS")]
    public int cardsOnFlop;

    public List<Card> cards = new List<Card>();

    private Vector3 startPosition;
    private float offsetX = 1.5f;

    [SerializeField] private AudioClip card;
    [SerializeField] private AudioClip end;

    [SerializeField] private Button reload;

    public List<Card> tableCards = new List<Card>();

    private void Start()
    {
        S = this;

        startPosition = cardPrefab.transform.position;

        CreateDeck();
        GetCards();
    }

    private void CreateDeck()
    { 
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                Card card = Instantiate(cardPrefab);
                card.SetCard(j, suits[i], images[imageId], cardId);
                imageId++;
                cardId++;
                cards.Add(card);

                card.transform.SetParent(deck);
                card.transform.position = deck.transform.position;
            }
        }
    }

    private void GetCards()
    {
        AudioSource.PlayClipAtPoint(card, transform.position);
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, cards.Count);

            Card handCard = cards[index];
            handCard.OpenCard();
            tableCards.Add(handCard);
            cards.RemoveAt(index);

            float posX = (offsetX * i) + startPosition.x;

            handCard.transform.position = new Vector3(posX, startPosition.y, startPosition.z);

            DefineCombination();
        }
    }

    public void Flop()
    {
        AudioSource.PlayClipAtPoint(card, transform.position);
        for (int i = 0; i < cardsOnFlop; i++)
        {
            int index = Random.Range(0, cards.Count);
            Card flopCard = cards[index];
            flopCard.OpenCard();
            tableCards.Add(flopCard);

            cards.RemoveAt(index);

            float startPositionXScale = startPosition.x - 1;
            float posX = (offsetX * i) + startPositionXScale;
            flopCard.transform.position = new Vector3(posX, 0, 0);

            DefineCombination();
        }
    }   

    public void Tern()
    {
        AudioSource.PlayClipAtPoint(card, transform.position);
        int index = Random.Range(0, cards.Count);
        Card ternCard = cards[index];
        ternCard.OpenCard();
        tableCards.Add(ternCard);

        cards.RemoveAt(index);

        ternCard.transform.position = new Vector3(0.4f, 0, 0);

        DefineCombination();
    }

    public void River()
    {
        AudioSource.PlayClipAtPoint(end, transform.position);
        int index = Random.Range(0, cards.Count);
        Card riverCard = cards[index];
        riverCard.OpenCard();
        tableCards.Add(riverCard);

        cards.RemoveAt(index);

        riverCard.transform.position = new Vector3(1.9f, 0, 0);

        reload.gameObject.SetActive(true);

        DefineCombination();
    }

    public void DefineCombination()
    {
        List<Combination> tableCombinations = new List<Combination>();

        var straightFlashes = FindStraightFlash();
        foreach (Combination straightFlash in straightFlashes)
            tableCombinations.Add(straightFlash);

        var flashes = FindFlash();
        foreach (Combination flash in flashes)
            tableCombinations.Add(flash);

         var carres = FindCarre();
         foreach (Combination carre in carres)
             tableCombinations.Add(carre);

         var fullHouses = FindFullHouse();
         foreach (Combination fullHouse in fullHouses)
             tableCombinations.Add(fullHouse);

         var streights = FindStraight(tableCards);
          foreach (Combination streight in streights)
              tableCombinations.Add(streight); 

         var sets = FindSet();
         foreach (Combination set in sets)
             tableCombinations.Add(set);

         var twoPairs = FindTwoPairs(tableCards);
         foreach (Combination twoPair in twoPairs)
             tableCombinations.Add(twoPair);

         var pairs = FindPair();
         foreach (Combination pair in pairs)
             tableCombinations.Add(pair);

        var kicker =  FindKicker(tableCards);
        tableCombinations.Add(kicker);
        

        var bestComb = tableCombinations.OrderByDescending(c => c.Strenght).First();

        scoreBoard.text = bestComb.Name;
        //+ " " + bestComb.Strenght;
        
    }
    private Combination FindKicker(List<Card> tableCards) 
    {
        var kicker = tableCards.OrderByDescending(c => c._value).First();
        List<Card> card = new List<Card>{kicker};
        return CombinationFactory.CreateKicker(card);
    }

    private List<Combination> FindPair()
    {
        List<Combination> pairs = new List<Combination>();

        List<List<Card>> countedByValueCards = CountByValue(tableCards);

        foreach (List<Card> cards in countedByValueCards)
        {       
            if (cards.Count == 2)
                pairs.Add(CombinationFactory.CreatePair(cards));
        }
        return pairs;
    }
    private List<Combination> FindTwoPairs(List<Card> tableCards)
    {
        List<Combination> twoPairs = new List<Combination>();

        List<List<Card>> tempListPairs = new List<List<Card>>();
        List<Card> cardsInPairs = new List<Card>();

        var pairsComb = FindPair();

        if (pairsComb.Count >= 2)
        {
            foreach (Combination comb in pairsComb)
                tempListPairs.Add(comb.Cards);
            
             foreach (List<Card> cards in tempListPairs)
                foreach (Card card in cards)
                    cardsInPairs.Add(card);
            twoPairs.Add(CombinationFactory.CreateTwoPairs(cardsInPairs));
        }
        return twoPairs;
    }

    private List<Combination> FindSet() 
    {
        List<Combination> set = new List<Combination>();

        List<List<Card>> cauntedByValueCards = CountByValue(tableCards);

        foreach (List<Card> cards in cauntedByValueCards)
        {
            if (cards.Count == 3)
                set.Add(CombinationFactory.CreateSet(cards));
        }
        return set;

    }

    public List<Combination> FindStraight(List<Card> tableCards)
    {
        var sortedByValueCards = tableCards.OrderByDescending(c => c._value).DistinctBy(c => c._value).ToList();

        List<Combination> straight = new List<Combination>();

        List<Card> cardsInStraight = new List<Card>();

        int maxCount = 0;

        for (int i = 0; i < sortedByValueCards.Count - 1; i++)
        {
            if (sortedByValueCards[i]._value - sortedByValueCards[i + 1]._value == 1)
            {
                cardsInStraight.Add(sortedByValueCards[i]);
                maxCount++;

                if (maxCount == 5)
                {
                    straight.Add(CombinationFactory.CreateStraight(cardsInStraight));
                }
            }
            else
            {
                cardsInStraight.Clear();
                maxCount = 0;
            }
        }
        return straight;
    }

    private List<Combination> FindFlash()
    {
        List<Combination> flash = new List<Combination>();

        List<List<Card>> cardsInFlash = CountBySuit(tableCards);

        foreach (List<Card> cards in cardsInFlash)
        {
            if (cards.Count >= 5)
                flash.Add(CombinationFactory.CreateFlash(cards));
        }
        return flash;               
    }


    private List<Combination> FindFullHouse()
    {
        List<Combination> fullHouse= new List<Combination>();

        List<List<Card>> cardsInFullhouse = new List<List<Card>>();

        var pairs = FindPair();
        var sets = FindSet();

        if (pairs.Count >= 1 && sets.Count >= 1 || sets.Count >= 2)
        {
            fullHouse = pairs.Concat(sets).ToList();
            foreach (Combination comb in fullHouse)
                cardsInFullhouse.Add(comb.Cards);

            foreach (List<Card> cards in cardsInFullhouse)
                fullHouse.Add(CombinationFactory.CreateFullHouse(cards));
        }
        return fullHouse;
    }

    private List<Combination> FindCarre()
    {
        List<Combination> carre = new List<Combination>();

        List<List<Card>> cauntedByValueCards = CountByValue(tableCards);

        foreach (List<Card> cards in cauntedByValueCards)
        {
            if (cards.Count == 4)
                carre.Add(CombinationFactory.CreateCarre(cards));
        }
        return carre;
    }

    private List<Combination> FindStraightFlash()
    {
        List<Combination> straightFlash = new List<Combination>();

        Dictionary<string, List<Card>> tempDict = new Dictionary<string, List<Card>>();

        var straights = FindStraight(tableCards);

        List<List<Card>> cardListsInStraights = new List<List<Card>>();

        foreach (Combination straight in straights)
            cardListsInStraights.Add(straight.Cards);

        foreach (List<Card> cards in cardListsInStraights)
            foreach (Card card in cards)
                if (!tempDict.ContainsKey(card._suit))
                    tempDict[card._suit] = new List<Card> { card };
                else
                    tempDict[card._suit].Add(card);

        List<List<Card>> cardsInStraightFlash = tempDict.Values.ToList();
        foreach (List<Card> cards in cardsInStraightFlash)
            if (cards.Count == 5)
                straightFlash.Add(CombinationFactory.CreateStraightFlash(cards));
        return straightFlash;

    }
    private List<List<Card>> CountByValue(List<Card> tableCards)
    {
        var dictionary = new Dictionary<int, List<Card>>();

        foreach (var card in tableCards)
        {
            if (!dictionary.ContainsKey(card._value))
                dictionary[card._value] = new List<Card> { card };
            else
                dictionary[card._value].Add(card);
        }
        return dictionary.Values.ToList();
    }

    private List<List<Card>> CountBySuit(List<Card> tableCards)
    {
        var dictionary = new Dictionary<string, List<Card>>();

        foreach (var card in tableCards)
        {
            if (!dictionary.ContainsKey(card._suit))
                dictionary[card._suit] = new List<Card> { card };
            else
                dictionary[card._suit].Add(card);
        }
        return dictionary.Values.ToList();
    }
}



