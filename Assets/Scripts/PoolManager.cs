using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region ������
    // ��������� ������ ����
    public GameObject[] enemy_prefs;        // ���� �������� ���� �� �ִ� �迭�� �ʱ�ȭ (����Ƽ���� �����յ��� ���� �����ؼ� ��ũ��Ʈ�� enemy_prefs�� �巡���ϸ� �ڵ����� ��� ��ϵ�)

    // ������Ʈ Ǯ���� ������ ����Ʈ (������ Ǯ �ȿ��� �������� �����յ��� ��)
    List<GameObject>[] enemy_pools;
    #endregion


    private void Awake()
    {
        enemy_pools = new List<GameObject>[enemy_prefs.Length];     // ����Ʈ�� ������ ������ ���� (�������� ���� ���� ��ŭ)

        for(int i = 0; i < enemy_pools.Length; i++) 
        {
            enemy_pools[i] = new List<GameObject>();        // enemy_pools���� ���� ����Ʈ���� �ʱ�ȭ
        }
    }

    #region Ǯ�Ŵ����� �ڽĿ�����Ʈ�� ���·� ������ ���ӿ�����Ʈ�� ����
    public GameObject Get(int i)
    {
        GameObject select = null;

        // foreach���� index�� ������� ������, �迭�̳� ����Ʈ���� �������� �����͸� ���� �� �ִ� ����ü���� ��ȸ�ϴ� for��
        foreach (GameObject enemyObj in enemy_pools[i])     // enemy_pools[i]���� �����ϴ� GameObject�鿡 ����
        {
            if (!enemyObj.activeSelf)       // ������ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ�� ����
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = enemyObj;
                select.SetActive(true);     // select ������ �Ҵ�� GameObject�� Ȱ��ȭ

                break;
            }
        }

        // �� �߰��ϸ� ���Ӱ� �����Ͽ� select ������ �Ҵ�
        if (select == null)     // (!select) �� ���� 
        {
            select = Instantiate(enemy_prefs[i], transform);        // PoolManager�� �ڽ� ������Ʈ�� ������ �ڿ� transform�� �־���
            enemy_pools[i].Add(select);         // ���Ӱ� ������ prefab ���ӿ�����Ʈ�� ������ƮǮ �ȿ� ������� �����Ƿ�, enemy_pools[i]�� ���Ӱ� ������ select ������ �־���
        }

        return select;
    }
    #endregion
}
