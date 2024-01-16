using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;    // 이미지를 담아주는 GameObject
    public Sprite gameOverSpr;      // GAME OVER 이미지
    public Sprite gameClearSpr;     // GAME CLEAR 이미지
    public GameObject panel;        // 패널
    public GameObject restartButton;// RESTART 버튼
    public GameObject nextButton;   // NEXT 버튼

    Image titleImage;               // 이미지를 표시하는 Image 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f);
        // 버튼(패널)을 숨기기
        panel.SetActive(false);
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
        }
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            
        }
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
