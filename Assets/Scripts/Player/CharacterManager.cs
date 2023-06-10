using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] char_Locked;
    public GameObject[] char_UnLocked;

    enum Achievement { UnlockChar1, UnlockChar2 }
    Achievement[] achieves;

    private void Awake()
    {
        achieves = (Achievement[])Enum.GetValues(typeof(Achievement));      // GetValues�� type�� �Ϲ� Array�̹Ƿ�, Achievement �迭�� �ٲٱ� ���� �տ� (Achievement[])�� �ٿ���� ��

        if (!PlayerPrefs.HasKey("MyData"))      // MyData��� Ű�� ���� ���� �ʴٸ� Init�Լ� ���� (��� ���� �ʱ�ȭ)
        {
            Init();
        }
    }


    private void Start()
    {
        UnlockChar();
    }

    #region ���� �ʱ�ȭ ~ �̰� �׽�Ʈ�ϱ� ���ؼ��� Unity - Edit - Clear All PlayerPrefs�� ������ ��� �����͸� �ʱ�ȭ��������Ѵ�. �� ���̶� �����ϸ� MyData�� �����Ͱ� ���� ����
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);        // ���� ���� ������ Ű ����

        foreach (Achievement achievement in achieves)       // Achievement�迭�� ��� ��� ������ �ʱ�ȭ
        {
            PlayerPrefs.SetInt(achievement.ToString(), 0);
        }
    }
    #endregion


    void UnlockChar()
    {
        for (int i = 0; i < char_Locked.Length; i++)        // ��� ��ư �迭�� ��ȸ�ϸ� �ε����� �ش��ϴ� ���� �̸� ��������
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;       // bool������ PlayerPefs�� ���ϴ� ������ 1�̸� true��, 0�̸� false�� ���� (PlayerPrefs.GetInt������ �����̹Ƿ� �� ������ �ʿ�)
            char_Locked[i].SetActive(!isUnlock);
            char_UnLocked[i].SetActive(isUnlock);
        }
    }
}
