using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; // 이미지를 담아주는 GameObject
    public Sprite gameOverSpr; // GAME OVER 이미지
    public Sprite gameClearSpr; // GAME CLEAR 이미지
    public GameObject panel; // 패널
    public GameObject restartButton; // RESTART 버튼
    public GameObject nextButton; // NEXT 버튼

    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    public GameObject scoreText;
    public static int totalScore;
    public int stageScore = 0;

    public AudioClip meGameOver;    // 게임 오버
    public AudioClip meGameClear;   // 게임 클리어
    


    Image titleImage; // 이미지를 표시하는 Image 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f);
        // 버튼(패널)을 숨기기
        panel.SetActive(false);
        
        timeCnt = GetComponent<TimeController>();

        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                // 시간 제한이 없으면 시간 표시바 숨김처리
                timeBar.SetActive(false);
            }
        }

        UpdateScore();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);
            // RESTART 버튼 무효화
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지
            }

            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            
            // 시간 갱신
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    // 정수에 할당하여 소수점 이하를 버림
                    int time = (int)timeCnt.displayTime;
                    // 시간 갱신
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    // 타임 오버
                    if (time == 0)
                    {
                        playerCnt.GameOver();
                    }
                }
            }

            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
        
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    void UpdateScore()
    {
        int score = stageScore + totalScore;
        Debug.Log(score);
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}