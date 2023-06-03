using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }
        // �� ��ũ��Ʈ�� ���� ������Ʈ�� ���࿡ Area�±׸� ���� ������Ʈ�� �浹���� �ʴ´ٸ� return(��ȯ), �� �Լ��� ���� return���� ���⵵�� �س���.
        // else {} �ȿ� �Ʒ� ������ ���� ���ε�, ���� else�� ���� �ʰ� �����ϰ� �Ʒ�ó�� �ٷ� �ᵵ ��. ������ ������ ���ߵ��� �ƴ� ��쿡�� ������ return���� ���Ͽ� �Լ� �۵��� �����ǹǷ�.

        Vector3 playerPos = GameManager.instance.player.transform.position;     // player�� ��ġ
        Vector3 myPos = transform.position;     // ���� �� ��ġ

        float diffX = Mathf.Abs(playerPos.x - myPos.x);     // Mathf.Abs �� ����Ͽ� player�� ���� Tile�� x�� ���̰��� ���밪�� ����
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;       // player�� ���ϴ� ����
        float dirX = playerDir.x < 0 ? -1 : 1;      // playerDir.x�� 0���� ���� ��� -1��, 0���� ū ��� 1�� ���� (���׿����� ���)
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)      // �÷��̾ �� ��ũ��Ʈ�� ���� ������Ʈ�� �ݶ��̴����� "����� ���", Ʈ���� �ߵ�
        {
            // ������ : �÷��̾ �ƹ��� �������� ������ ���� ���·� ���Ϳ��� �з��� �̵��� ���, dirX & dirY ���� 0�̾ �Ʒ� �ڵ尡 ���� ���� �̻��ϰ� �̵���
            case "Ground":      // �÷��̾ Ground �±׸� ���� �ݶ��̴����� ��� ���
                {
                    if (diffX > diffY)      // ���� diffX�� diffY���� ũ�ٸ� Ÿ���� �̵�.
                    {
                        transform.Translate(Vector3.right * dirX * 40);     // Tile �Ѱ��� 20ĭ, �� 4���� ��������Ƿ� �̵� �ÿ��� �ѹ��� 40ĭ�� �������� ���� Ÿ�ϰ� ��ġ�� ����.
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }

                    break;
                }

            case "Enemy":      // �÷��̾ Enemy �±׸� ���� �ݶ��̴����� ��� ���
                {
                    if (col.enabled)
                    {
                        transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));        // ���Ͱ� ȭ�� ������ ����� �÷��̾� �� ȭ�� ������ ��ġ�� ��ȯ�ǵ��� ����
                    }

                    break;
                }
        }
    }
}
