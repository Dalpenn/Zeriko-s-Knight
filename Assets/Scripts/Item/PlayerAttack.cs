using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region ������
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

        if (penetrate > -1)     // ������� 0 �̻��� ���⸸ dir�������� ���ư����� ����
        {
            rigid.velocity = dir * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetrate == -1)       // ���Ⱑ Enemy�̿��� �±׸� ���� ������Ʈ�� �ε����ų� ������� -1�� ��쿡�� �ƹ��� �ϵ� �Ͼ�� ����
        {
            return;
        }
        else       // ���Ⱑ Enemy�±׸� ���� ���� �ε����ų� ������� 0 �̻��� ���
        {
            penetrate--;        // ����� -1

            // ���� ����� -1������ ������ ���밪�� -1�� ��쿡 �ƹ��ϵ� �Ͼ�� �ʰ� �ϱ� ���� ��
            // �Ʒ� ����� -1������ ���Ⱑ ���� �����ϰ� �������� ���밪�� -1�� �� ��쿡 ��Ȱ��ȭ �ϱ� ���� ��
            if (penetrate == -1)        // ����� ���� -1�� �Ǹ� ������ �ӵ��� ���ְ� ��Ȱ��ȭ �Ѵ� (������Ʈ Ǯ������ �������̹Ƿ� Destroy�� ���� ����)
            {
                rigid.velocity = Vector2.zero;      // �Ŀ� �ٽ� ��� �� �� �ֵ��� ������ �ӵ��� �ʱ�ȭ���ѳ��´�
                gameObject.SetActive(false);
            }
        }
    }

    #region �÷��̾�� �����Ÿ� �̻� �������� ���� ��Ȱ��ȭ �ϴ� �Լ�
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