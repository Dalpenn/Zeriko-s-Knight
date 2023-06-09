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
    Collider2D col;
    SpriteRenderer sp;
    Animator anim;

    WaitForFixedUpdate wait;
    #endregion


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }


    void FixedUpdate()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("hurt"))       // 살아있지 않거나 Hit애니메이션이 돌아가고 있는 상황에는 동작 정지 (애니메이터 트리거 이름이 아닌, "상태 이름"을 넣어야 한다)
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
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        if (!isLive)
        {
            return;
        }

        sp.flipX = target.position.x < rigid.position.x;        // 몬스터가 바라보는 방향이 플레이어 쪽이도록 조정
    }

    void OnEnable()     // 몬스터가 생성되었을 때 초기값 설정
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();

        #region 재활용을 위해, 생성되었을 때에 생존관련 변수들 설정 (안해놓으면 죽을 때 해놨던 설정들로 있어서 동작하지 않기 때문)
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        sp.sortingOrder = 2;
        anim.SetBool("isDead", false);
        #endregion

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
        if (!collision.CompareTag("PlayerAttack") || !isLive)       // 몬스터가 PlayerAttack이외 태그의 콜리젼과 충돌하거나, 죽어있는 경우에는 무시
        {
            return;
        }

        // else 생략 ~ 살아있을 때에만 아래 코드가 실행됨
        hp -= collision.GetComponent<PlayerAttack>().dmg;
        StartCoroutine(KnockBack());

        if (hp > 0)
        {
            anim.SetTrigger("Hit");

            AudioManager.instance.PlaySFX(AudioManager.SFX.Hit);
        }
        else
        {
            #region 생존관련 변수들 설정 (죽은 걸로)
            isLive = false;                 // 생존변수 false로
            col.enabled = false;            // 콜라이더 끄기
            rigid.simulated = false;        // rigidbody 끄기

            sp.sortingOrder = 1;            // 죽어서 다른 몬스터를 가리면 안되므로, sprite의 order를 1로 내린다

            anim.SetBool("isDead", true);
            #endregion

            #region 몬스터 처치 관련 변수들 설정
            GameManager.instance.curKill++;
            GameManager.instance.GetExp();
            #endregion

            if (GameManager.instance.isGameStarted)     // 게임이 진행중일 때만 재생 ~ 게임 끝나면 재생X (게임 이기면 모든 몬스터들 다 죽이는데, 이 때 한번에 모든 몬스터들의 죽는 사운드 재생되는 것을 방지)
            {
                AudioManager.instance.PlaySFX(AudioManager.SFX.Dead);
            }

            // Dead(); 함수는 여기서 바로 실행하면 죽는 모션이 나오기 전에 비활성화되므로, 애니메이션에서 Event추가로 실행시킬 것임
        }
    }

    void Dead()     // 몬스터가 죽었을 때의 함수 ~ 애니메이션 Event에서 추가해놓은 함수
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;  // 하나의 물리 프레임 딜레이를 준다

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 1, ForceMode2D.Impulse);         // 플레이어와 반대방향으로 몬스터를 밀어내도록 힘을 줌
    }
}
