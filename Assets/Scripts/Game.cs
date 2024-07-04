using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Class that represents the game
/// </summary>
public class Game : MonoBehaviour
{

    private int firstPlayerPoints;
    private int secondPlayerPoints;

    [SerializeField]
    private UnityEngine.UI.Text firstPlayerScoreText;

    [SerializeField]
    private UnityEngine.UI.Text secondPlayerScoreText;

    [SerializeField]
    private UnityEngine.UI.Text resultText;

    private Color blue = new Color(0.2f, 0.5f, 0.8f);
    private Color grey = new Color(0.3f, 0.3f, 0.3f);

    private Card currentChosenCard;
    private Boolean firstPlayerTurn;
    private Boolean hold;

    [SerializeField]
    private Card cardPrefab;

    [SerializeField]
    private Transform grid;

    [SerializeField]
    private const int numberOfCardsToMatch = 2;


    private List<cardType> cardDeck;

    // Start is called before the first frame update
    void Start()
    {
        hold = false;
        firstPlayerPoints = 0;
        secondPlayerPoints = 0;
        currentChosenCard = null;
        firstPlayerTurn = true;
        cardDeck = new List<cardType>();

        UpdateScoreTexts();

        foreach (cardType type in (cardType[])Enum.GetValues(typeof(cardType)))
        {
            for (int i = 0; i < numberOfCardsToMatch; i++)
            {
                cardDeck.Add(type);
            }
        }

        cardDeck = shuffled(cardDeck);

        foreach (cardType type in cardDeck)
        {
            Card newCard = Instantiate(cardPrefab, grid);
            newCard.SetCardType(type);
            newCard.SetGame(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Method that Shuffles the given Deck of cards
    /// </summary>
    /// <param name="Deck"> the given Deck </param>
    /// <returns> a shuffled copy of the givenDeck </returns>
    private List<cardType> shuffled(List<cardType> Deck)
    {
        List<cardType> newDeck = new List<cardType>(Deck);
        System.Random rand = new System.Random();
        int n = newDeck.Count;
        while (n > 1)
        {
            n--;
            int i = rand.Next(n + 1);
            cardType temp = newDeck[i];
            newDeck[i] = newDeck[n];
            newDeck[n] = temp;
        }
        return newDeck;
    }

    public void CardTouched(Card touchedCard)
    {
        if (!hold)
        {
            touchedCard.SetCardSprite(touchedCard.GetCardType().GetHashCode().ToString());

            if (currentChosenCard == null)
            {
                currentChosenCard = touchedCard;

            }
            else if (currentChosenCard != touchedCard)
            {
                StartCoroutine(WaitCardPick(touchedCard, 1f));
            }
        }
        

    }

    public void PickResult(Card touchedCard)
    {
        if (touchedCard.GetCardType() == currentChosenCard.GetCardType())
        {
            if (firstPlayerTurn) firstPlayerPoints += 2;
            else secondPlayerPoints += 2;

            Destroy(touchedCard.GetComponent<Button>());
            Destroy(currentChosenCard.GetComponent<Button>());
            touchedCard.SetCardSprite("Invisible");
            currentChosenCard.SetCardSprite("Invisible");

            firstPlayerTurn = !firstPlayerTurn;
            currentChosenCard = null;

        }
        else
        {
            touchedCard.SetCardSprite("Back1");
            currentChosenCard.SetCardSprite("Back1");
            currentChosenCard = null;
            firstPlayerTurn = !firstPlayerTurn;
        }
        UpdateScoreTexts();
        if (firstPlayerPoints + secondPlayerPoints == 12) {
            StartCoroutine(WaitGameEnded(4f));
        }
    }

    private void UpdateScoreTexts()
    {
        firstPlayerScoreText.text = "Player1 : " + firstPlayerPoints.ToString();
        secondPlayerScoreText.text = "Player2 : " + secondPlayerPoints.ToString();
        if (firstPlayerTurn)
        {
            firstPlayerScoreText.color = blue;
            secondPlayerScoreText.color = grey;
        }
        else
        {
            firstPlayerScoreText.color = grey;
            secondPlayerScoreText.color = blue;
        }
    }

    IEnumerator WaitCardPick(Card touchedCard, float seconds)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        hold = true;

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);

        //After we have waited 5 seconds print the time again.
        hold = false;
        PickResult(touchedCard);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator WaitGameEnded(float seconds)
    {
        string displayText = "";
        if (firstPlayerPoints > secondPlayerPoints) displayText = "Player 1 wins";
        else if (firstPlayerPoints < secondPlayerPoints) displayText = "Player 2 wins";
        else displayText = "Draw";

        resultText.text = displayText;

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);

        //After we have waited 5 seconds print the time again.
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
