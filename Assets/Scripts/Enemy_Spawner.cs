using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    #region ������
    public Transform[] spawnPoint;

    float timer;
    #endregion


    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // Enemy_Spawner������Ʈ�� �ڽĿ�����Ʈ��(��ȯ ��ҵ�) ��ġ�� �� �� �������� ���̹Ƿ�, GetComponent"s" �� ��� ��
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            Spawn();

            timer = 0f;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolMng.Get(Random.Range(0, 4));        // ������ ���� enemy ������Ʈ�� ����
        // Random.Range�� ������ (0, 3)�� �ƴ϶� (0, 4)�� ���� ������, (x, y)�� �ǹ̰� "x ~ y" �� �ƴ϶�, "x�̻� y�̸�" �̱� ����

        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // ������ �� ������ ����, �츮�� ���ص� spawnPoint�� �߿��� ������ �������� �̵���Ŵ
        // GetComponentsInChildern���� �ڱ��ڽŵ� ���Եȴ�. �׷��Ƿ� spawnPoint�� enemySpawner�� ��ġ�� ���ԵǾ� �ִµ�, ���⼭�� ���� ������ ���� �ƴϹǷ� 0���� �����ϰ� 1������ ��ȯ��Ҹ� �������� ����
    }
}