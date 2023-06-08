using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ������

    [Header("# Singleton")]       // �̷������� ����� ����ؼ� ����ϰ� ���� �� ���� �ִ� ~ Inspectorâ���� ����ϰ� ����
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
    //public int[] nextExp = {3, 5, 10, 100, 150, 210};         // �̷��� ���⿡�� �迭�� �־��� ���� �ִ�

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

        #region ���� ����ð� ǥ��
        if (gameTime > maxGameTime)         // ���ӽð��� ���ѽð��� �ٵǸ�, �� �̻� �귯���� �ʰ� ���ѽð��� ǥ�õǾ��ְ��ϱ�
        {
            gameTime = maxGameTime;
        }
        #endregion
    }

    #region ���� ����
    public void GameStart()
    {
        curHp = maxHp;      // ���� �� �÷��̾� hp �ִ�� �����

        ui_SelectSkill.SelectStartSkill(0);
        isGameStarted = true;
    }
    #endregion

    public void GetExp()
    {
        curExp++;      // GetExp�Լ��� ����Ǹ� ����ġ�� �������� ����

        if (curExp == nextExp[Mathf.Min(level, nextExp.Length - 1)])      // ���� ����ġ�� �ö��µ� ������ (���� -1) �����̾��� ��, ��� �������� �����ϳ� �䱸 ����ġ�� ���� ����ġ�� �����ǵ��� ����
        {
            level++;
            curExp = 0;
            ui_SelectSkill.ShowScreen_Skill();
        }
    }

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
}
