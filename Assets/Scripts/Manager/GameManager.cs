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
    public bool isGameStarted = false;

    public int stageLv;

    [Header("# Player Info")]
    public float curHp;
    public float maxHp;
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
    public Ctrl_Sc_SelectSkill ui_SelectSkill;

    #endregion

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        gameTime += Time.deltaTime;

        #region 게임 진행시간 표시
        if (gameTime > maxGameTime)         // 게임시간이 제한시간이 다되면, 더 이상 흘러가지 않고 제한시간이 표시되어있게하기
        {
            gameTime = maxGameTime;
        }
        #endregion
    }

    #region 게임 시작
    public void GameStart()
    {
        curHp = maxHp;      // 시작 시 플레이어 hp 최대로 만들기

        ui_SelectSkill.SelectStartSkill(0);
        isGameStarted = true;
    }
    #endregion

    public void GetExp()
    {
        curExp++;      // GetExp함수가 실행되면 경험치가 오르도록 설정

        if (curExp == nextExp[Mathf.Min(level, nextExp.Length - 1)])      // 만약 경험치가 올랐는데 레벨이 (만렙 -1) 레벨이었을 시, 계속 레벨업은 가능하나 요구 경험치는 만렙 경험치로 유지되도록 설정
        {
            level++;
            curExp = 0;
            ui_SelectSkill.ShowScreen_Skill();
        }
    }

    #region 게임 일시정지 / 재개
    public void GamePause()
    {
        isGameStarted = false;
        Time.timeScale = 0;
    }

    public void GameResume()
    {

        isGameStarted = true;
        Time.timeScale = 1;
    }
    #endregion
}
