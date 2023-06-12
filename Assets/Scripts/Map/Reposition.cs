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

        else
        {
            // ���� else�� ���� �ʰ� �����ϰ� �ᵵ ��. ������ ������ ���ߵ��� �ƴ� ��쿡�� ������ return���� ���Ͽ� �Լ� �۵��� �����ǹǷ�.
            Vector3 playerPos = GameManager.instance.player.transform.position;     // player�� ��ġ
            Vector3 myPos = transform.position;     // ���� �� ��ġ

            #region �÷��̾ �ν��ϴ� ���� ������ ������ �浹ü�鿡 ���� �ڵ�
            switch (transform.tag)      // �÷��̾ �� ��ũ��Ʈ�� ���� ������Ʈ�� �ݶ��̴����� "����� ���", Ʈ���� �ߵ�
            {
                // ������ : �÷��̾ �ƹ��� �������� ������ ���� ���·� ���Ϳ��� �з��� �̵��� ���, dirX & dirY ���� 0�̾ �Ʒ� �ڵ尡 ���� ���� �̻��ϰ� �̵���
                case "Ground":      // �÷��̾ Ground �±׸� ���� �ݶ��̴����� ��� ���
                    {
                        #region Ÿ���� �̵� ������ ���ϴ� �ڵ�
                        // (�÷��̾� ��ġ - Ÿ�� ��ġ)�� �̿��Ͽ� -���� ������ -1��, +���� ������ +1�� ������ �־ Ÿ���� �÷��̾��� ��/����/��/�Ʒ����� ������ �����ϴ� �ڵ�
                        // -1�� ������ ��� : Ÿ���� �÷��̾�� �����ʿ� �����Ƿ�(Ÿ�� ��ġ���� �÷��̾�� �� ũ��) Ÿ���� ���� ������ �����̴�(-1)
                        float diffX = playerPos.x - myPos.x;
                        float diffY = playerPos.y - myPos.y;

                        // diffX,Y���� �̿��Ͽ� Ÿ�� ��ġ ������ ���� ����
                        float dirX = diffX < 0 ? -1 : 1;      // diffX�� 0���� ���� ��� -1��, 0���� ū ��� 1�� ���� (���׿����� ���)
                        float dirY = diffY < 0 ? -1 : 1;

                        // Ÿ�� ��ġ ������ ��������, �÷��̾�� ���� �Ÿ� �̻� �������� Ÿ���� �̵��� �� �ֵ��� �Ÿ��� ���ϱ� ���� diff���� �ٽ� ���밪���� �ٲ�
                        diffX = Mathf.Abs(diffX);
                        diffY = Mathf.Abs(diffY);
                        #endregion

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
                            Vector3 distance = playerPos - myPos;
                            Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                            
                            // ���Ͱ� ȭ�� ������ ����� �÷��̾� �� ȭ�� ������ ��ġ�� ��ȯ�ǵ��� ����
                            // distance��ŭ�� �����̸� ���Ͱ� �÷��̾� ��ġ�� �� ���̸�ŭ ������ ~ x2�� ���ָ� �� ���̸�ŭ �÷��̾��� ������ ������
                            transform.Translate(ran + distance * 2);
                        }

                        break;
                    }
            }
            #endregion
        }
    }
}
