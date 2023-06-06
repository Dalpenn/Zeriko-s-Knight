using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 변수들
    public float speed;
    public float hp;
    public float maxHp;
    public RuntimeAnimatorController[] animCon;     // 여러 종류의 몬스터를 쓸 것이므로 배열로 선언
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    Animator anim;

    WaitForFixedUpdate wait;
    #endregion


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }


    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))       // 살아있지 않거나 Hit애니메이션이 돌아가고 있는 상황에는 동작 정지
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

    void OnEnable()     // 몬스터가 생성되었을 때 초기값 설정
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        hp = maxHp;
    }

    public void Init(SpawnData data)        // 몬스터 데이터를 Enemy_Spawner스크립트의 SpawnData클래스와 연동하여 설정하는 함수
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];      // runtimeAnimatorController를 이용하여 몬스터 종류를 변경
        speed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PlayerAttack"))
        {
            return;
        }

        // else 생략
        hp -= collision.GetComponent<PlayerAttack>().dmg;
        StartCoroutine(KnockBack());

        if (hp > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            Dead();
        }
    }

    void Dead()     // 몬스터가 죽었을 때의 함수
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;  // 하나의 물리 프레임 딜레이를 준다

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 50, ForceMode2D.Impulse);         // 플레이어와 반대방향으로 몬스터를 밀어내도록 힘을 줌
    }
}
