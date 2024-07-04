using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private cardType type;
    private Game game;
    public Image image;


    // Start is called before the first frame update
    void OnEnable()
    {
        image = GetComponent<Image>();
        SetCardSprite("Back1");
    }

    public cardType GetCardType() { return type; }
    public void SetCardType(cardType givenType) {  type = givenType; }

    public void SetGame(Game givenGame) { game = givenGame; }

    public void SetCardSprite(string path) { image.sprite = Resources.Load<Sprite>("Sprites/" + path); }

    public void onClick()
    {
        game.CardTouched(this);
    }

}

public enum cardType
{
    BASKETBALL = 0,
    PAN = 1,
    SUN = 2,
    PIZZA = 3,
    PHONE = 4,
    SPEAKERS = 5

}
