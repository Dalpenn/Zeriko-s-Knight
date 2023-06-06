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
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("hurt"))       // ������� �ʰų� Hit�ִϸ��̼��� ���ư��� �ִ� ��Ȳ���� ���� ���� (�ִϸ����� Ʈ���� �̸��� �ƴ�, "���� �̸�"�� �־�� �Ѵ�)
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

        #region ��Ȱ���� ����, �����Ǿ��� ���� �������� ������ ���� (���س����� ���� �� �س��� ������� �־ �������� �ʱ� ����)
        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        sp.sortingOrder = 2;
        anim.SetBool("isDead", false);
        #endregion

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
        StartCoroutine(KnockBack());

        if (hp > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            #region �������� ������ ���� (���� �ɷ�)
            isLive = false;                 // �������� false��
            col.enabled = false;            // �ݶ��̴� ����
            rigid.simulated = false;        // rigidbody ����

            sp.sortingOrder = 1;            // �׾ �ٸ� ���͸� ������ �ȵǹǷ�, sprite�� order�� 1�� ������

            anim.SetBool("isDead", true);
            #endregion

            // Dead(); �Լ��� ���⼭ �ٷ� �����ϸ� �״� ����� ������ ���� ��Ȱ��ȭ�ǹǷ�, �ִϸ��̼ǿ��� �����ų ����
        }
    }

    void Dead()     // ���Ͱ� �׾��� ���� �Լ�
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;  // �ϳ��� ���� ������ �����̸� �ش�

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 1, ForceMode2D.Impulse);         // �÷��̾�� �ݴ�������� ���͸� �о���� ���� ��
    }
}
