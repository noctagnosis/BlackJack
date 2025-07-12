using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button DealBtn;
    [SerializeField] Button HitBtn;
    [SerializeField] Button StandBtn;

    private int standClicks = 0;

    [SerializeField] UserScript playerScript;
    [SerializeField] UserScript dealerScript;

    [SerializeField] Text HandText;
    [SerializeField] Text DealerText;
    [SerializeField] Text BetText;
    [SerializeField] Text CashText;
    [SerializeField] Text MainText;
    [SerializeField] Text StandBtnText;
    [SerializeField] GameObject HideCard;
    int pot = 0;

    void Start()
    {
        DealBtn.onClick.AddListener(() => DealClicked());
        HitBtn.onClick.AddListener(() => HitClicked());
        StandBtn.onClick.AddListener(() => StandClicked());
    }

    private void DealClicked()
    {
        playerScript.ResetHand();
        dealerScript.ResetHand();
        DealerText.gameObject.SetActive(false);
        MainText.gameObject.SetActive(false);
        DealerText.gameObject.SetActive(false);
        GameObject.Find("deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        HandText.text = $"Hand: {playerScript.handValue}";
        DealerText.text = $"Hand: {dealerScript.handValue}";
        HideCard.GetComponent<Renderer>().enabled = true;
        DealBtn.gameObject.SetActive(false);
        HitBtn.gameObject.SetActive(true);
        StandBtn.gameObject.SetActive(true);
        StandBtnText.text = "Stand";
        pot = 100;
        BetText.text = $"Bets: ${pot}";
        playerScript.AdjustMoney(-pot / 2);
        CashText.text = $"${playerScript.GetMoney()}";

    }

    private void HitClicked()
    {
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            HandText.text = $"Hand: {playerScript.handValue}";
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        StandBtnText.text = "Call";
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            DealerText.text = $"Hand: {dealerScript.handValue}";
            if (dealerScript.handValue > 20) RoundOver();
        }
    }
    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        if (playerBust && dealerBust)
        {
            MainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            MainText.text = "Dealer wins!";
        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            MainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            MainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        if (roundOver)
        {
            HitBtn.gameObject.SetActive(false);
            StandBtn.gameObject.SetActive(false);
            DealBtn.gameObject.SetActive(true);
            MainText.gameObject.SetActive(true);
            DealerText.gameObject.SetActive(true);
            HideCard.GetComponent<Renderer>().enabled = false;
            CashText.text = $"${playerScript.GetMoney()}";
            standClicks = 0;
        }
    }
}
