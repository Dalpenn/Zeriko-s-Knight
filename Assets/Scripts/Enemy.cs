using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region ������
    public float speed;
    public float hp;
    public float maxHp;
    public RuntimeAnimatorController[] animCon;     // ���� ������ ���͸� �� ���̹Ƿ� �迭�� ����
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    Animator anim;
    #endregion


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        if (!isLive)
        {
            return;
        }

        Vector2 dirVec = target.position - rigid.position;      // ���Ϳ� �÷��̾��� �Ÿ� ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;      // ������ �������� ����� �޶����� �ʵ��� DeltaTime �����ֱ�
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;      // �÷��̾�� ���� �浹 ��, ���� �з����� �ʵ��� rigid�� �ӵ��� 0���� �����س���
    }

    void LateUpdate()
    {
        if (!isLive)
        {
            return;
        }

        sp.flipX = target.position.x < rigid.position.x;        // ���Ͱ� �ٶ󺸴� ������ �÷��̾� ���̵��� ����
    }

    void OnEnable()     // ���Ͱ� �����Ǿ��� �� �ʱⰪ ����
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        hp = maxHp;
    }

    public void Init(SpawnData data)        // ���� �����͸� Enemy_Spawner��ũ��Ʈ�� SpawnDataŬ������ �����Ͽ� �����ϴ� �Լ�
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];      // runtimeAnimatorController�� �̿��Ͽ� ���� ������ ����
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

        // else ����
        hp -= collision.GetComponent<PlayerAttack>().dmg;

        if (hp > 0)
        {

        }
        else
        {
            Dead();
        }
    }

    void Dead()     // ���Ͱ� �׾��� ���� �Լ�
    {
        gameObject.SetActive(false);
    }
}
