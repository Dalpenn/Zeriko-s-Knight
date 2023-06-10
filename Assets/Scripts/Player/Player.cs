using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region 변수들
    public Vector2 inputVec;
    public float player_spd = 3;
    public float player_initSpd;
    bool isDamaged = false;

    public Scanner scanner;
    public RuntimeAnimatorController[] animCtrl;

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
    }

    private void OnEnable()
    {
        #region 플레이어 직업에 따른 초기 이동속도 설정
        player_spd *= Character.movSpd;         // 직업특성에 따른 이동속도 증감 (없으면 1, 있으면 그 수치가 들어감)
        #endregion

        player_initSpd = player_spd;

        anim.runtimeAnimatorController = animCtrl[GameManager.instance.playerID];       // 선언해준 runtimeAnimCtrl에 플레이어 아이디에 맞는 애니컨트롤러를 넣어줌
    }

    #region InputSystem 사용않을 경우 플레이어 이동 코드
    //void Update()
    //{
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}
    #endregion

    private void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
    }


    void FixedUpdate()
    {
        #region 플레이어 이동 키 활성화 조건
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") || GameManager.instance.curHp < 1)
        {
            return;
        }
        #endregion

        #region 플레이어 이동 코드
        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize는 어느 방향으로든 1만큼 이동시킴(특히 대각선 이동의 경우) ~ 지금은 InputSystem의 normalize기능을 사용하고 있어서 코드에 따로 추가할 필요 없음.
        // DeltaTime은 Update, FixedDeltaTime은 FixedUpdate의 프레임 하나가 소비한 시간

        rigid.MovePosition(rigid.position + nextVec);
        #endregion
    }


    void LateUpdate()
    {
        #region 플레이어 이동 애니메이션 및 좌우반전 활성화 조건
        if (!GameManager.instance.isGameStarted || anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude는 순수하게 "크기"만 갖고 있는 성분이다. inputVec의 크기가 0인지 아닌지만 확인하기 위한 용도.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x가 0보다 작으면 true, 크면 false가 들어간다
        }
        #endregion
    }

    #region 플레이어 피격 판정 코드
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isGameStarted || collision.transform.CompareTag("PlayerAttack") || isDamaged)
        {
            return;
        }
        else
        {
            StartCoroutine(OnDamaged());      // 플레이어가 몬스터와 충돌 시 함수
        }
    }

    #region 플레이어 피격 시 데미지 및 이펙트 함수
    IEnumerator OnDamaged()
    {
        // 체력 감소
        GameManager.instance.curHp--;
        //GameManager.instance.hpCtrl.HPBarSync();

        if (GameManager.instance.curHp < 1)     // 플레이어 사망 시 코드
        {
            for (int i = 2; i < transform.childCount; i++)      // player자식 중, Shadow와 Area 이후 것들을 다 비활성화 하기 위하여 0, 1빼고 2부터 index 시작
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Death");

            GameManager.instance.GameOver();
        }
        else
        {

            // 잠시 무적
            isDamaged = true;

            anim.SetTrigger("Hurt");

            // 피격 이펙트
            for (float i = 0; i < 0.5f; i += 0.1f)
            {
                sp.color = new Color(0, 0, 0, 0.3f);
                yield return new WaitForSeconds(0.1f);
                sp.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.1f);
            }

            isDamaged = false;
        }
    }
    #endregion
    #endregion

    #region InputSystem 함수
    // Unity의 Input System 사용을 위한 함수
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    #endregion
}
