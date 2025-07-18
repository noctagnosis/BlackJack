using System.Collections.Generic;
using UnityEngine;

public class UserScript : MonoBehaviour
{
    
    public CardScript cardScript;   
    public DeckScript deckScript;

    public int handValue = 0;

    private int money = 1000;

    public GameObject[] hand;
    public int cardIndex = 0;
    List<CardScript> aceList = new List<CardScript>();

    public void StartHand()
    {
        GetCard();
        GetCard();
    }
    public int GetCard()
    {
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        handValue += cardValue;
        if(cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
        AceCheck();
        cardIndex++;
        return handValue;
    }

    public void AceCheck()
    {
        foreach (CardScript ace in aceList)
        {
            if(handValue + 10 < 22 && ace.GetValueCards() == 1)
            {
                ace.SetValueCards(11);
                handValue += 10;
            } else if (handValue > 21 && ace.GetValueCards() == 11)
            {
                ace.SetValueCards(1);
                handValue -= 10;
            }
        }
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
    }
    public int GetMoney()
    {
        return money;
    }
    public void ResetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCards();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}