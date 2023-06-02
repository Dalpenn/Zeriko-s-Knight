using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region 변수들
    Vector2 inputVec;
    float player_spd = 3;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    Animator anim;
    #endregion
    

    void Awake()
    {
        #region 가져온 변수들 초기화(반드시 필요)
        rigid = GetComponent<Rigidbody2D>();        // Unity의 Player가 갖고 있는 Rigidbody2D속성을 스크립트로 가져옴.
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        #endregion
    }


    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize는 어느 방향으로든 1만큼 이동시킴(특히 대각선 이동의 경우) ~ 지금은 InputSystem의 normalize기능을 사용하고 있어서 코드에 따로 추가할 필요 없음.
        // DeltaTime은 Update, FixedDeltaTime은 FixedUpdate의 프레임 하나가 소비한 시간

        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude는 순수하게 "크기"만 갖고 있는 성분이다. inputVec의 크기가 0인지 아닌지만 확인하기 위한 용도.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x가 0보다 작으면 true, 크면 false가 들어간다
        }
    }


    // Unity의 Input System 사용을 위한 함수
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
