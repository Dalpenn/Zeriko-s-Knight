using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region 변수들
    public Vector2 inputVec;
    public float player_spd = 3;
    public float player_initSpd;

    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    Animator anim;
    #endregion
    

    void Awake()
    {
        #region 가져온 변수들 초기화(반드시 필요)
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();        // Unity의 Player가 갖고 있는 Rigidbody2D속성을 스크립트로 가져옴.
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        #endregion

        player_initSpd = player_spd;
    }


    // InputSystem을 사용하지 않는 경우, 플레이어 이동 코드
    //void Update()
    //{
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}

    private void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
    }


    void FixedUpdate()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize는 어느 방향으로든 1만큼 이동시킴(특히 대각선 이동의 경우) ~ 지금은 InputSystem의 normalize기능을 사용하고 있어서 코드에 따로 추가할 필요 없음.
        // DeltaTime은 Update, FixedDeltaTime은 FixedUpdate의 프레임 하나가 소비한 시간

        rigid.MovePosition(rigid.position + nextVec);
    }


    void LateUpdate()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude는 순수하게 "크기"만 갖고 있는 성분이다. inputVec의 크기가 0인지 아닌지만 확인하기 위한 용도.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x가 0보다 작으면 true, 크면 false가 들어간다
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            float dmg = collision.transform.GetComponent<Enemy>().dmg;
            
            GameManager.instance.curHp -= Time.deltaTime * dmg;
            //GameManager.instance.curHp -= Time.deltaTime * 10;      // 플레이어가 받는 데미지 10

            if (GameManager.instance.curHp < 0)     // 플레이어 사망 시 코드
            {
                for (int i = 2; i < transform.childCount; i++)      // player자식 중, Shadow와 Area 이후 것들을 다 비활성화 하기 위하여 0, 1빼고 2부터 index 시작
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                anim.SetTrigger("Death");
            }
        }
    }

    #region InputSystem 함수
    // Unity의 Input System 사용을 위한 함수
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    #endregion
}
