
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // 다트윈

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite animalSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private SpriteRenderer cardRenderer;

    private bool isFlipped = false;
    private bool isFlipping = false;
    private bool isMatched = false;
    
    public void SetAnimalSprite(Sprite sprite){
        animalSprite = sprite;
    }
    public int cardID;
    public void SetCardID(int id){
        cardID = id;
    }
    public void SetCardMatched(){
        isMatched = true;
    }
    public void FlipCard(){
        isFlipping = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, 0.2f).OnComplete(()=>{
            isFlipped = !isFlipped;
            if (isFlipped){
                cardRenderer.sprite = animalSprite;
            } else{
                cardRenderer.sprite = backSprite;
            }

            transform.DOScale(originalScale, 0.2f).OnComplete(()=>{ isFlipping = false;});
        });
        
    }
    void OnMouseDown(){
        // Debug.Log("mouse down");
        if(!isFlipping && !isMatched){  // 뒤집혀지고 있는 상태가 아닐 경우에만
            // FlipCard();
            GameManager.instance.CardClicked(this); //this 전달 
        }
    }
}
