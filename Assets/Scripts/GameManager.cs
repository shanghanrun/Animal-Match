using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> allCards;
    [SerializeField] private float chanceTime =4f; // 처음 보는 시간
    private Card flippedCard;
    private bool isFlipping = false;

    void Awake(){
        if(instance == null){
            instance = this;
        } else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards();

        StartCoroutine(FlipAllCardsRoutine());
    }

    IEnumerator FlipAllCardsRoutine(){
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(chanceTime);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f); //뒤집히는 시간 여유
        isFlipping = false;
    }

    void FlipAllCards(){
        foreach(Card card in allCards){
            card.FlipCard();
        }
    }

    public void CardClicked(Card card){
        if(isFlipping){
            return;
        }
        card.FlipCard();

        if(flippedCard == null){
            flippedCard = card;
        } else{
            //check match
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2){
        isFlipping = true;

        if(card1.cardID == card2.cardID){ // Card클래스에서 public변수  cardID
            card1.SetCardMatched();
            card2.SetCardMatched();
        } else{
            Debug.Log("Different Card");
            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null; // 다시 초기화
    }
}
