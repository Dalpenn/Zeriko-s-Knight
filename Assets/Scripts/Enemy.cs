using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region ������
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

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); 
    }
}
