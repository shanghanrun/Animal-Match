using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Card> allCards;
    [SerializeField] private float chanceTime =4f; // 처음 보는 시간
    private Card flippedCard;
    private bool isFlipping = false;
    [SerializeField] private Slider timeoutSlider;
    [SerializeField] private float timeLimit = 60f;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timeoutText;
    private int matchesFound =0;
    private int totalMatches =10; //10쌍   (전제 20개카드)

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private bool isGameOver = false;

    

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

        currentTime = timeLimit; // 60초로 시작
        SetCurrentTimeText();

        StartCoroutine(FlipAllCardsRoutine());
    }
    void SetCurrentTimeText(){
        int timeSec = Mathf.CeilToInt(currentTime); //반올림한 정수
        timeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllCardsRoutine(){
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(chanceTime);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f); //뒤집히는 시간 여유
        isFlipping = false;

        //이때부터 시간을 줄여나가는 작업
        yield return StartCoroutine("CountDownTimerRoutine"); // 앞의 것을 끝내고 새로운 코루틴 시작
    }

    IEnumerator CountDownTimerRoutine(){
        while(currentTime >0){
            currentTime -= Time.deltaTime;
            timeoutSlider.value = currentTime /timeLimit;
            SetCurrentTimeText();
            yield return null; // 바로 다음 프레임에 이어서 실행되게 함
        }

        GameOver(false); // 실패로 끝난 게임오버
    }

    void FlipAllCards(){
        foreach(Card card in allCards){
            card.FlipCard();
        }
    }

    public void CardClicked(Card card){
        if(isFlipping || isGameOver){
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
            matchesFound++;
            
            if(matchesFound == totalMatches){
                GameOver(true);
            }
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
    void GameOver(bool success){
        if(!isGameOver){
            isGameOver = true;
            StopCoroutine("CountDownTimerRoutine");
            if(success){
                // Debug.Log("success");
                gameOverText.SetText("Great!!");
                // 성공으로 끝났을 경우에는 타이머를 멈추어야 된다.

            } else{
                gameOverText.SetText("Game Over");
            }
            // gameOverPanel.SetActive(true); 이렇게 하면 너무 급격하게 나타난다.
            Invoke("ShowGameOverPanel",2f);
        }

    }
    void ShowGameOverPanel(){
        gameOverPanel.SetActive(true);
    }
    public void Restart(){
        SceneManager.LoadScene("SampleScene"); 
    }
}
