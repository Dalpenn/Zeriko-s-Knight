using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region ������
    // ��������� ������ ������ �ʿ�
    public GameObject[] enemy_prefs;        // ���� �������� ���� �� �ִ� �迭�� �ʱ�ȭ

    // ������Ʈ Ǯ�� �����ϴ� ����Ʈ�� �ʿ�
    List<GameObject>[] enemy_pools;
    #endregion


    private void Awake()
    {
        enemy_pools = new List<GameObject>[enemy_prefs.Length];     // ����Ʈ�� ������ ������ ����(Length ���)

        for(int i = 0; i < enemy_pools.Length; i++) 
        {
            enemy_pools[i] = new List<GameObject>();
        }
    }
}
