// using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardSprites;

    private List<int> cardIDList = new List<int>();
    private List<Card> cardList = new List<Card>();
    void Start()
    {
        GenerateCardIDList();
        ShuffleCardIDList();
        InitBoard();
    }

    void GenerateCardIDList(){
        // [0,0,1,1,....19,19]
        for(int i=0; i<cardSprites.Length; i++){
            cardIDList.Add(i);
            cardIDList.Add(i);
        }
    }
    void ShuffleCardIDList(){
        int cardCount = cardIDList.Count; //Count는 List<T>의 길이
        for(int i=0; i< cardCount; i++){
            int randomIndex = Random.Range(i, cardCount);
            int temp = cardIDList[randomIndex];
            cardIDList[randomIndex] = cardIDList[i];
            cardIDList[i] = temp;
        }
    }
    
    void InitBoard(){
        float spaceY = 1.8f;
        // row (-2, -1, 0, 1, 2)
        // 0 -2 = -2 - 1.8f -1.8f = -2 -2 *spaceY = -2 -3.6
        // 1 -2 = -1
        // 2 - ?(2) = 0    중앙이 y=0인 좌표
        // 3 -2 = 1
        // 4 -3 = 2

        // rowCount /2 한 값을 정수로 바꾼다.

        // (row - (int)(rowCount/2)) * spaceY

        float spaceX = 1.3f;
        // col (-1.5, -0.5, 0.5, 1.5)
        // 0 - (colCount/2) +0.5 = -1.5
        // 1
        // 2
        // 3

        // (col - (colCount/2))*spaceX +(spaceX /2);

        int rowCount =5; //세로
        int colCount =4;
        int spriteIndex =0;

        for(int row=0; row < rowCount; row++){
            for(int col=0; col<colCount; col++){

                float posX = (col -(colCount/2))*spaceX +(spaceX/2);
                float posY = (row -(int)(rowCount/2)) * spaceY;
                
                
                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity);
                Card card = cardObject.GetComponent<Card>();
                int cardID = cardIDList[spriteIndex++];
                card.SetCardID(cardID);
                card.SetAnimalSprite(cardSprites[cardID]);
                cardList.Add(card);
            }
        }
    }
    public List<Card> GetCards(){
        return cardList;
    }
}
