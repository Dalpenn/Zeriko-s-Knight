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

    public int stageLv;

    [Header("# Player Info")]
    public int curHp;
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
        curExp++;      // GetExp�Լ��� ����Ǹ� ����ġ�� �������� ����

        if (curExp == nextExp[level])      // ���� ����ġ�� �ö��µ� ������ ����ġ���� �������� ��, �������ϰ� ���� ����ġ�� 0�� �ǵ��� ����
        {
            level++;
            curExp = 0;
        }
    }
}
