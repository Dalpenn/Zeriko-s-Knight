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
        WeaponDead(10);
    }

    #region ���� �ʱ� ���� �� �̵� ����
    public void Init(float dmg, int penetrate, Vector3 dir)
    {
        this.dmg = dmg;
        this.penetrate = penetrate;

        if (penetrate >= 0)     // ������� 0 �̻��� ���⸸ dir�������� ���ư����� ����
        {
            rigid.velocity = dir * bulletSpeed;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region ���Ⱑ �ݶ��̴��� �ε����� ���
        if (!collision.CompareTag("Enemy") || penetrate < 0 )       // ���Ⱑ Enemy�̿��� �±׸� ���� ������Ʈ�� �ε����ų� ������� 0������ ��쿡�� �ƹ��� �ϵ� �Ͼ�� ����
        {
            return;
        }
        else       // ���Ⱑ Enemy�±׸� ���� ���� �ε����ų� ������� 0 �̻��� ���
        {
            penetrate--;        // ����� -1

            // ���� ����� -1������ ������ ���밪�� -1�� ��쿡 �ƹ��ϵ� �Ͼ�� �ʰ� �ϱ� ���� ��
            // �Ʒ� ����� -1������ ���Ⱑ ���� �����ϰ� �������� ���밪�� -1�� �� ��쿡 ��Ȱ��ȭ �ϱ� ���� ��
            if (penetrate < 0)        // ����� ���� 0���� �۾����� ������ �ӵ��� ���ְ� ��Ȱ��ȭ �Ѵ� (������Ʈ Ǯ������ �������̹Ƿ� Destroy�� ���� ����)
            {
                rigid.velocity = Vector2.zero;      // �Ŀ� �ٽ� ��� �� �� �ֵ��� ������ �ӵ��� �ʱ�ȭ���ѳ��´�
                gameObject.SetActive(false);
            }
        }
        #endregion
    }

    #region �÷��̾�� �����Ÿ� �̻� �������� ���� ��Ȱ��ȭ �ϴ� �Լ�
    void WeaponDead(float distance)
    {
        Transform player = GameManager.instance.player.transform;
        Vector3 playerPos = player.position;

        float dir = Vector3.Distance(playerPos, transform.position);

        if (dir > distance)
        {
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}