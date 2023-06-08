using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    #region ������
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;       // ���� ���� ������ �����͸� ��� �� ���̹Ƿ�, �迭�� ����
    int Dungeonlevel;
    float timer;
    #endregion

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // Enemy_Spawner������Ʈ�� �ڽĿ�����Ʈ��(��ȯ ��ҵ�) ��ġ�� �� �� �������� ���̹Ƿ�, GetComponent"s" �� ��� ��
    }


    void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        timer += Time.deltaTime;

        #region ������ ���� �� ���� ~ GameManager�� maxGameTime�� �����Ǿ� ������
        //level = Mathf.FloorToInt(GameManager.instance.gameTime / 5f);      // Mathf�Լ��� FloorToInt : �Ҽ��� �Ʒ��� ������ Int������ �ٲٴ� �Լ� (�ݴ�� �ø��� CeilToInt)
        Dungeonlevel = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 5f), spawnData.Length - 1);      // �ִ� level�� �̷��� ���� ����

        if (timer > spawnData[Dungeonlevel].spawnTime)     // Level�� ���� spawnData�� �ִ� spawnTime���� �� ����
        {
            Spawn();

            timer = 0f;
        }
        #endregion
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolMng.Get(0);        // ������ ���� enemy ������Ʈ�� ����

        #region ������ �� ��ȯ��ġ �����ϰ� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // ������ �� ������ ����, �츮�� ���ص� spawnPoint�� �߿��� ������ �������� �̵���Ŵ
        // GetComponentsInChildern���� �ڱ��ڽŵ� ���Եȴ�. �׷��Ƿ� spawnPoint�� enemySpawner�� ��ġ�� ���ԵǾ� �ִµ�, ���⼭�� ���� ������ ���� �ƴϹǷ� 0���� �����ϰ� 1������ ��ȯ��Ҹ� �������� ����
        #endregion

        enemy.GetComponent<Enemy>().Init(spawnData[Dungeonlevel]);
    }
}

#region ��ȯ ������ ��� Ŭ����
[System.Serializable]       // SpawnDataŬ������ ����ȭ�Ͽ� Inspectorâ���� ���̰� ����
public class SpawnData      // ��ȯ �����͸� ����ϴ� Ŭ���� ����
{
    public float spawnTime;
    public int spriteType;
    public float hp;
    public float speed;
    public float dmg;
}
#endregion