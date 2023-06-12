using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 변수들

    [Header("# Singleton")]       // 이런식으로 헤더를 사용해서 깔끔하게 정리 할 수도 있다 ~ Inspector창에도 깔끔하게 나옴
    public static GameManager instance;

    [Header("# GameControl")]
    public float gameTime;
    public float maxGameTime = 9999;
    public bool isGameStarted = false;

    public int stageLv;

    [Header("# Player Info")]
    public int playerID;

    private int curhp_property;
    public int curHp        // 프로퍼티를 사용하여 curHp를 사용 할 때마다, hpController의 HPBarSync로 자동으로 동기화하도록 설정
    {
        get 
        { 
            return curhp_property; 
        }
        set 
        { 
            curhp_property = value;
            hpCtrl.HPBarSync();
        }
    }
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
    public HPController hpCtrl;
    public GameObject enemyCleaner;

    public Ctrl_Sc_SelectSkill ui_SelectSkill;
    public GameResult ui_Result;

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
        if (gameTime > maxGameTime)         // 게임시간이 제한시간이 다되면, 게임승리 창 띄우기
        {
            gameTime = maxGameTime;
            GameWin();
        }
        #endregion
    }

    #region 캐릭터 경험치 획득 함수
    public void GetExp()
    {
        #region 게임이 중지되면 경험치 획득 못하도록 설정
        if (!isGameStarted)
        {
            return;
        }
        #endregion

        curExp++;      // GetExp함수가 실행되면 경험치가 오르도록 설정

        if (curExp == nextExp[Mathf.Min(level, nextExp.Length - 1)])      // 만약 경험치가 올랐는데 레벨이 (만렙 -1) 레벨이었을 시, 계속 레벨업은 가능하나 요구 경험치는 만렙 경험치로 유지되도록 설정
        {
            level++;
            curExp = 0;
            ui_SelectSkill.ShowScreen_Skill();
        }
    }
    #endregion

    #region 게임 시작
    public void GameStart(int id)
    {
        playerID = id;
        curHp = maxHp;              // 시작 시 플레이어 hp 최대로 만들기
        player.gameObject.SetActive(true);

        ui_SelectSkill.SelectStartSkill(playerID % 2);          // 게임 시작 시, 플레이어 직업에 따라 스킬 선택지 주기

        GameResume();               // result 화면이 뜰때 멈춰놨던 timeScale을 다시 1로 만들어야 하므로

        AudioManager.instance.PlayBGM(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
    }
    #endregion

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

    #region 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region 타이틀로 돌아가기
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);          // LoadScene : 이름/인덱스로 장면을 새롭게 호출하는 함수
    }
    #endregion

    #region 게임 오버
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // 먼저 게임을 중지
        isGameStarted = false ;

        // 플레이어 죽는 모션 나오기 기다림
        yield return new WaitForSeconds(1.0f);

        ui_Result.gameObject.SetActive(true);
        ui_Result.DungeonFailed();

        GamePause();

        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Lose);
    }
    #endregion

    #region 게임 승리
    public void GameWin()
    {
        StartCoroutine(GameWinRoutine());
    }

    IEnumerator GameWinRoutine()
    {
        // 먼저 게임을 중지 후, 클리너 활성화
        isGameStarted = false;
        enemyCleaner.SetActive(true);

        // 모든 남아있는 몬스터가 죽는 모션 나오기 기다림
        yield return new WaitForSeconds(1.0f);

        ui_Result.gameObject.SetActive(true);
        ui_Result.DungeonCleared();

        GamePause();

        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Win);
    }
    #endregion
}
