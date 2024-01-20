using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;

    public float moveY = 0.0f;

    public float times = 0.0f;

    public float weight = 0.0f;

    public bool isMoveWhenOn = false;

    public bool isCanMove = true;

    float perDX;

    float perDY;

    Vector3 defPos;

    bool isReverse = false;
    
    // Start is called before the first frame update
    void Start()
    {
        defPos = transform.position;
        float timestamp = Time.fixedDeltaTime;
        perDX = moveX / (1.0f / timestamp * times);
        perDY = moveY / (1.0f / timestamp * times);

        if (isMoveWhenOn)
        {
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            float x = transform.position.x;
            float y = transform.position.y;

            bool endX = false;
            bool endY = false;
            if (isReverse)
            {
                // 이동량이 양수고 이동 위치가 초기 위치보다 작거나
                // 이동량이 음수고 이동 위치가 초기 위치보다 큰 경우
                if ((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    endX = true;
                }

                if ((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y))
                {
                    endY = true;
                }
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z));
            }
            else
            {
                // 이동량이 양수고 이동 위치가 초기 위치보다 크거나
                // 이동량이 음수고 이동 위치가 초기 + 이동거리 보다 작은 경우
                if ((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX))
                {
                    endX = true;
                }

                if ((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true;
                }
                // 블록 이동
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if (endX && endY)
            {
                if (isReverse)
                {
                    // 위치가 어긋나는 것을 방지하고자 정면 방향 이동으로 돌아가기 전에 초기 위치로 돌리기
                    transform.position = defPos;
                }

                isReverse = !isReverse;
                isCanMove = false;
                if (isMoveWhenOn == false)
                {
                    //올라갔을 때 움직이는 값이 꺼진 경우 weight 만큼 지연 후 다시 이동
                    Invoke("Move", weight);
                }
            }
        }
    }
    
    // 이동하게 만들기
    public void Move()
    {
        isCanMove = true;
    }
    
    // 이동하지 못하게 만들기
    public void Stop()
    {
        isCanMove = false;
    }
    
    // 접촉 시작
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 접촉한 것이 플레이어라면 이동 블록의 자식으로 만들기
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                // 올라갔을 때 움직인다면 이동하게 만들기
                isCanMove = true;
            }
        }
    }
    
    // 접촉 종료
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 접촉한 것이 플레이어라면 이동 블록의 자식에서 제외시키기
            collision.transform.SetParent(null);
        }
    }
}
