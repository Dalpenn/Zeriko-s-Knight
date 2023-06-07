using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 변수들

    [Header("# Singleton")]       // 이런식으로 헤더를 사용해서 깔끔하게 정리 할 수도 있다 ~ Inspector창에도 깔끔하게 나옴
    public static GameManager instance;

    [Header("# GameControl")]
    public float gameTime;
    public float maxGameTime = 4 * 10f;

    public int stageLv;

    [Header("# Player Info")]
    public int curHp;
    public int maxHp;
    public int curKill;
    public int[] nextKill;
    public int level;
    public int curExp;
    public int[] nextExp;
    //public int[] nextExp = {3, 5, 10, 100, 150, 210};         // 이렇게 여기에서 배열로 넣어줄 수도 있다

    [Header("# Game Object")]
    public PoolManager poolMng;
    public Player player;
    public Weapon weapon;

    #endregion

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        curHp = maxHp;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        curExp++;      // GetExp함수가 실행되면 경험치가 오르도록 설정

        if (curExp == nextExp[level])      // 만약 경험치가 올랐는데 레벨업 경험치량을 충족했을 시, 레벨업하고 현재 경험치는 0이 되도록 설정
        {
            level++;
            curExp = 0;
        }
    }
}
