using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region 변수들
    public float dmg;
    public int penetrate;
    public float bulletSpeed;

    Rigidbody2D rigid;
    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        WeaponDead();
    }

    public void Init(float dmg, int penetrate, Vector3 dir)
    {
        this.dmg = dmg;
        this.penetrate = penetrate;

        if (penetrate > -1)     // 관통력이 0 이상인 무기만 dir방향으로 날아가도록 설정
        {
            rigid.velocity = dir * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetrate == -1)       // 무기가 Enemy이외의 태그를 가진 오브젝트와 부딪히거나 관통력이 -1인 경우에는 아무런 일도 일어나지 않음
        {
            return;
        }
        else       // 무기가 Enemy태그를 가진 적과 부딪히거나 관통력이 0 이상인 경우
        {
            penetrate--;        // 관통력 -1

            // 위의 관통력 -1조건은 무기의 관통값이 -1인 경우에 아무일도 일어나지 않게 하기 위한 것
            // 아래 관통력 -1조건은 무기가 적을 관통하고 지나가며 관통값이 -1이 된 경우에 비활성화 하기 위한 것
            if (penetrate == -1)        // 관통력 값이 -1이 되면 무기의 속도를 없애고 비활성화 한다 (오브젝트 풀링으로 관리중이므로 Destroy는 쓰지 말자)
            {
                rigid.velocity = Vector2.zero;      // 후에 다시 사용 할 수 있도록 무기의 속도는 초기화시켜놓는다
                gameObject.SetActive(false);
            }
        }
    }

    #region 플레이어와 일정거리 이상 떨어지면 무기 비활성화 하는 함수
    void WeaponDead()
    {
        Transform player = GameManager.instance.player.transform;
        Vector3 playerPos = player.position;

        float dir = Vector3.Distance(playerPos, transform.position);

        if (dir > 10f)
        {
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}