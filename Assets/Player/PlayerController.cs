using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;  // Rigidbody2D형 변수

    float axisH = 0.0f;         // 입력
    public float speed = 3.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2D 가져오기
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 수평방향으로의 입력 확인
        axisH = Input.GetAxisRaw("Horizontal");
        
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
    }

    void FixedUpdate()
    {
        // 속도 갱신하기
        rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
    }
}
