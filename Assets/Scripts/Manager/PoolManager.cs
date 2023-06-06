using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region 변수들
    // 프리펩들을 보관할 변수
    public GameObject[] enemy_prefs;        // 여러 변수들을 담을 수 있는 배열로 초기화 (유니티에서 프리팹들을 전부 선택해서 스크립트의 enemy_prefs로 드래그하면 자동으로 모두 등록됨)

    // 오브젝트 풀들을 저장할 리스트 (각각의 풀 안에는 여러개의 프리팹들이 들어감)
    List<GameObject>[] enemy_pools;
    #endregion


    private void Awake()
    {
        enemy_pools = new List<GameObject>[enemy_prefs.Length];     // 리스트에 변수들 개수를 넣음 (프리팹의 종류 개수 만큼)

        for(int i = 0; i < enemy_pools.Length; i++) 
        {
            enemy_pools[i] = new List<GameObject>();        // enemy_pools내의 개별 리스트들을 초기화
        }
    }

    #region 풀매니저에 자식오브젝트의 형태로 선택한 게임오브젝트를 생성
    public GameObject Get(int i)
    {
        GameObject select = null;

        // foreach문은 index를 사용하지 않지만, 배열이나 리스트같은 여러개의 데이터를 담을 수 있는 구조체들을 순회하는 for문
        foreach (GameObject enemyObj in enemy_pools[i])     // enemy_pools[i]내에 존재하는 GameObject들에 접근
        {
            if (!enemyObj.activeSelf)       // 선택한 풀의 비활성화된 게임오브젝트에 접근
            {
                // 발견하면 select 변수에 할당
                select = enemyObj;
                select.SetActive(true);     // select 변수에 할당된 GameObject를 활성화

                break;
            }
        }

        // 못 발견하면 새롭게 생성하여 select 변수에 할당
        if (select == null)     // (!select) 도 가능 
        {
            select = Instantiate(enemy_prefs[i], transform);        // PoolManager의 자식 오브젝트로 들어가도록 뒤에 transform을 넣어줌
            enemy_pools[i].Add(select);         // 새롭게 생성된 prefab 게임오브젝트는 오브젝트풀 안에 들어있지 않으므로, enemy_pools[i]에 새롭게 생성한 select 변수를 넣어줌
        }

        return select;
    }
    #endregion
}
