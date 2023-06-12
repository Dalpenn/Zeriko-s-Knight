using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region ������

    [Header("# Singleton")]       // �̷������� ����� ����ؼ� ����ϰ� ���� �� ���� �ִ� ~ Inspectorâ���� ����ϰ� ����
    public static GameManager instance;

    [Header("# GameControl")]
    public float gameTime;
    public float maxGameTime = 9999;
    public bool isGameStarted = false;

    public int stageLv;

    [Header("# Player Info")]
    public int playerID;

    private int curhp_property;
    public int curHp        // ������Ƽ�� ����Ͽ� curHp�� ��� �� ������, hpController�� HPBarSync�� �ڵ����� ����ȭ�ϵ��� ����
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
    //public int[] nextExp = {3, 5, 10, 100, 150, 210};         // �̷��� ���⿡�� �迭�� �־��� ���� �ִ�

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

        #region ���� ����ð� ǥ��
        if (gameTime > maxGameTime)         // ���ӽð��� ���ѽð��� �ٵǸ�, ���ӽ¸� â ����
        {
            gameTime = maxGameTime;
            GameWin();
        }
        #endregion
    }

    #region ĳ���� ����ġ ȹ�� �Լ�
    public void GetExp()
    {
        #region ������ �����Ǹ� ����ġ ȹ�� ���ϵ��� ����
        if (!isGameStarted)
        {
            return;
        }
        #endregion

        curExp++;      // GetExp�Լ��� ����Ǹ� ����ġ�� �������� ����

        if (curExp == nextExp[Mathf.Min(level, nextExp.Length - 1)])      // ���� ����ġ�� �ö��µ� ������ (���� -1) �����̾��� ��, ��� �������� �����ϳ� �䱸 ����ġ�� ���� ����ġ�� �����ǵ��� ����
        {
            level++;
            curExp = 0;
            ui_SelectSkill.ShowScreen_Skill();
        }
    }
    #endregion

    #region ���� ����
    public void GameStart(int id)
    {
        playerID = id;
        curHp = maxHp;              // ���� �� �÷��̾� hp �ִ�� �����
        player.gameObject.SetActive(true);

        ui_SelectSkill.SelectStartSkill(playerID % 2);          // ���� ���� ��, �÷��̾� ������ ���� ��ų ������ �ֱ�

        GameResume();               // result ȭ���� �㶧 ������� timeScale�� �ٽ� 1�� ������ �ϹǷ�

        AudioManager.instance.PlayBGM(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
    }
    #endregion

    #region ���� �Ͻ����� / �簳
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

    #region ���� ����
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Ÿ��Ʋ�� ���ư���
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);          // LoadScene : �̸�/�ε����� ����� ���Ӱ� ȣ���ϴ� �Լ�
    }
    #endregion

    #region ���� ����
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // ���� ������ ����
        isGameStarted = false ;

        // �÷��̾� �״� ��� ������ ��ٸ�
        yield return new WaitForSeconds(1.0f);

        ui_Result.gameObject.SetActive(true);
        ui_Result.DungeonFailed();

        GamePause();

        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Lose);
    }
    #endregion

    #region ���� �¸�
    public void GameWin()
    {
        StartCoroutine(GameWinRoutine());
    }

    IEnumerator GameWinRoutine()
    {
        // ���� ������ ���� ��, Ŭ���� Ȱ��ȭ
        isGameStarted = false;
        enemyCleaner.SetActive(true);

        // ��� �����ִ� ���Ͱ� �״� ��� ������ ��ٸ�
        yield return new WaitForSeconds(1.0f);

        ui_Result.gameObject.SetActive(true);
        ui_Result.DungeonCleared();

        GamePause();

        AudioManager.instance.PlayBGM(false);
        AudioManager.instance.PlaySFX(AudioManager.SFX.Win);
    }
    #endregion
}
