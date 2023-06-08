using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    #region 변수들
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;       // 여러 종류 몬스터의 데이터를 담당 할 것이므로, 배열로 선언
    int Dungeonlevel;
    float timer;
    #endregion

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();      // Enemy_Spawner오브젝트의 자식오브젝트들(소환 장소들) 위치를 싹 다 가져오는 것이므로, GetComponent"s" 를 써야 함
    }


    void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        timer += Time.deltaTime;

        #region 레벨에 따라 적 생성 ~ GameManager의 maxGameTime과 연동되어 조절됨
        //level = Mathf.FloorToInt(GameManager.instance.gameTime / 5f);      // Mathf함수의 FloorToInt : 소수점 아래는 버리고 Int형으로 바꾸는 함수 (반대로 올림은 CeilToInt)
        Dungeonlevel = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 5f), spawnData.Length - 1);      // 최대 level은 이렇게 설정 가능

        if (timer > spawnData[Dungeonlevel].spawnTime)     // Level에 따라 spawnData에 있는 spawnTime마다 적 생성
        {
            Spawn();

            timer = 0f;
        }
        #endregion
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolMng.Get(0);        // 생성한 적을 enemy 오브젝트로 지정

        #region 생성한 적 소환위치 랜덤하게 설정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;     // 지정한 갓 생성된 적을, 우리가 정해둔 spawnPoint들 중에서 무작위 방향으로 이동시킴
        // GetComponentsInChildern에는 자기자신도 포함된다. 그러므로 spawnPoint에 enemySpawner의 위치도 포함되어 있는데, 여기서는 몬스터 생성할 것이 아니므로 0번은 제외하고 1번부터 소환장소를 랜덤으로 돌림
        #endregion

        enemy.GetComponent<Enemy>().Init(spawnData[Dungeonlevel]);
    }
}

#region 소환 데이터 담당 클래스
[System.Serializable]       // SpawnData클래스를 직렬화하여 Inspector창에서 보이게 만듦
public class SpawnData      // 소환 데이터를 담당하는 클래스 선언
{
    public float spawnTime;
    public int spriteType;
    public float hp;
    public float speed;
    public float dmg;
}
#endregion