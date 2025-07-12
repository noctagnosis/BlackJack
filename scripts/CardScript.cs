using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] int value = 0;

    public int GetValueCards()
    {
        return value;
    }

    public void SetValueCards(int newValue)
    {
        value = newValue;
    }

    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void ResetCards()
    {
        Sprite back = GameObject.Find("deck").GetComponent<DeckScript>().GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        value = 0;
    }
}