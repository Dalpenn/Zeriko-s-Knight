using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    #region ������
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearTarget;
    #endregion

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);     // ������ ������ �浹�ϴ� ��� LayerMask�� ���� ����� ��ȯ�ϴ� �Լ�
        // Physics2D.CircleCastAll(�߽���ġ, �� ���� ~ ������, ���� ��� ���� ~ ���̹Ƿ� ����, ��� ���� ���� ~ ���̹Ƿ� ����, �浹�� ������ ��� ���̾�)

        nearTarget = GetNearest();      // nearTarget���� ������ ���� ���� GetNearest�Լ��� ���� ���� ����� Ÿ���� ��ġ������ ���� �ȴ�
    }

    Transform GetNearest()
    {
        Transform result = null;

        float diff = 100;

        foreach (RaycastHit2D target in targets)        // ���������� �浹�� ������Ʈ��(RaycastHit2D target)�� ���������� ��� �����´�
        {
            Vector3 myPos = transform.position;         // �÷��̾� ��ġ
            Vector3 targetPos = target.transform.position;      // �浹�� ������Ʈ�� ��ġ

            float curDiff = Vector3.Distance(myPos, targetPos);         // �÷��̾� ��ġ�� �浹�� ������Ʈ ��ġ�� �Ÿ� ���̸� ����

            if (curDiff < diff)         // ���� �Ÿ����̰� diff���� ������, �� �Ÿ����̰��� diff���� �ְ� result�� �� Ÿ���� ��ġ������ ���� ~ �̷������� ��� Ÿ���� ���ϸ�, �������� �Ÿ��� ���� ����� Ÿ���� ���Եȴ�.
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
