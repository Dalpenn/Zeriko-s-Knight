using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region 변수들
    // 프리펩들을 보관할 변수가 필요
    public GameObject[] enemy_prefs;        // 여러 변수들을 담을 수 있는 배열로 초기화

    // 오브젝트 풀을 구성하는 리스트가 필요
    List<GameObject>[] enemy_pools;
    #endregion


    private void Awake()
    {
        enemy_pools = new List<GameObject>[enemy_prefs.Length];     // 리스트에 변수들 개수를 넣음(Length 사용)

        for(int i = 0; i < enemy_pools.Length; i++) 
        {
            enemy_pools[i] = new List<GameObject>();
        }
    }
}
