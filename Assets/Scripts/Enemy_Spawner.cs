using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    #region 변수들
    public Transform[] spawnPoint;

    float timer;
    #endregion


    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // Enemy_Spawner오브젝트의 자식오브젝트들(소환 장소들) 위치를 싹 다 가져오는 것이므로, GetComponent"s" 를 써야 함
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
        GameObject enemy = GameManager.instance.poolMng.Get(Random.Range(0, 4));        // 생성한 적을 enemy 오브젝트로 지정
        // Random.Range의 범위를 (0, 3)이 아니라 (0, 4)로 적은 이유는, (x, y)의 의미가 "x ~ y" 가 아니라, "x이상 y미만" 이기 때문

        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // 지정한 갓 생성된 적을, 우리가 정해둔 spawnPoint들 중에서 무작위 방향으로 이동시킴
        // GetComponentsInChildern에는 자기자신도 포함된다. 그러므로 spawnPoint에 enemySpawner의 위치도 포함되어 있는데, 여기서는 몬스터 생성할 것이 아니므로 0번은 제외하고 1번부터 소환장소를 랜덤으로 돌림
    }
}