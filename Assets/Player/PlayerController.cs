using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static string gameState = "playing"; // 게임 상태
    
    Rigidbody2D rbody;  // Rigidbody2D형 변수

    float axisH = 0.0f;         // 입력
    public float speed = 3.0f;
    
    public float jump = 9.0f;       // 점프력
    public LayerMask groundLayer;   // 착지할 수 있는 레이어
    bool goJump = false;            // 점프 개시 플래그
    bool onGround = false;          // 지면에 서 있는 플래그

    bool isMoving = false;
    
    // 애니메이션 처리
    private Animator animator; // 애니메이터
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; // 점수
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2D 가져오기
        rbody = this.GetComponent<Rigidbody2D>();
        // Animator 가져오기
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        if (isMoving == false)
        {
            // 수평방향으로의 입력 확인
            axisH = Input.GetAxisRaw("Horizontal");
        }
        
        // 방향 조절
        if (axisH > 0.0f)
        {
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1, 1);
            
        }
        else if (axisH < 0.0f)
        {
            Debug.Log("왼쪽 이동");
            // 좌우 반전시키기
            transform.localScale = new Vector2(-1, 1);
        }
        
        // 캐릭터 점프하기
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        // 착지 판정
        onGround = Physics2D.Linecast(
            transform.position,
            transform.position - (transform.up * 0.1f),
            groundLayer
        );

        // 지면 위거나 속도가 0이 아님
        if (onGround || axisH != 0)
        {
            // 속도 갱신하기
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            // 지면 위에서 점프 키 눌림
            // 점프하기
            Debug.Log("점프");
            Vector2 jumpPw = new Vector2(0, jump); // 점프를 위한 벡터 생성
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // 순간적인 힘 가하기
            goJump = false; // 점프 플래그 끄기
        }

        if (onGround)
        {
            // 지면 위
            if (axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            // 공중
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // 애니메이션 재생
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            Goal();
        }
        else if (collision.gameObject.CompareTag("Dead"))
        {
            GameOver();
        }
        else if (collision.gameObject.CompareTag("ScoreItem"))
        {
            // 점수 아이템
            // ItemData 가져오기
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            
            // 점수 얻기
            score = item.value;
            
            // 아이템 제거
            Destroy(collision.gameObject);
        }
    }

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop();
    }

    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();
        
        // 플레이어의 충돌 판정 비활성
        GetComponent<CapsuleCollider2D>().enabled = false;
        
        // 플레이어의 위로 튀어 오르게 하는 연출 추가
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void GameStop()
    {
        // Rigidbody2D 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        
        // 속도를 0으로 하여 강제 정지
        rbody.velocity = new Vector2(0, 0);
    }
    public void Jump()
    {
        // 점프 플래그 켜기
        goJump = true;
        Debug.Log("점프 버튼 눌림");
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
}
