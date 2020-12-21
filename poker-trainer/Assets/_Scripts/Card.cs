using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;

    public string _suit;
    public int _value;
    public int _id;

    public bool isOpen;

    /*private void OnMouseDown()
    {
        if (cardBack.activeSelf)
        {
            cardBack.SetActive(false);
            isOpen = true;
        }
    } */

    public void OpenCard()
    {
        cardBack.SetActive(false);
        isOpen = true;
    } 

    public void SetCard(int value, string suit, Sprite image, int id)
    {
        SpriteRenderer renderer = cardFront.GetComponent<SpriteRenderer>();
        renderer.sprite = image;
        _value = value;
        _suit = suit;
        _id = id;
    }
}
