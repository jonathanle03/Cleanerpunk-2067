using UnityEngine;
using System.Collections;
using System;

public class CardDB : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Hashtable CardList;

    void Start()
    {
        Hashtable CardList = new Hashtable();

    }

    public CardOld giveCard()
    {
        System.Random r = new System.Random();
        CardOld card = gameObject.AddComponent<CardOld>();
        card.name = "mouse";
        card.cost = r.Next(1, 10);
        card.name = "Kabuto";
        return card;
    }
}
