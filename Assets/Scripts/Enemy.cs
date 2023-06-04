using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 변수들
    float speed = 2.3f;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    #endregion


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        if (!isLive)
        {
            return;
        }

        Vector2 dirVec = target.position - rigid.position;      // 몬스터와 플레이어의 거리 차이
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;      // 프레임 영향으로 결과가 달라지지 않도록 DeltaTime 곱해주기
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;      // 플레이어와 적이 충돌 시, 서로 밀려나지 않도록 rigid의 속도를 0으로 설정해놓음
    }

    void LateUpdate()
    {
        if (!isLive)
        {
            return;
        }

        sp.flipX = target.position.x < rigid.position.x;        // 몬스터가 바라보는 방향이 플레이어 쪽이도록 조정
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); 
    }
}
