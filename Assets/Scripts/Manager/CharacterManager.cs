using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region ������
    public GameObject[] char_Locked;
    public GameObject[] char_UnLocked;
    public GameObject ui_Notice;

    enum Achievement { UnlockChar1, UnlockChar2 }
    Achievement[] achieves;

    WaitForSecondsRealtime notice_time;    // ������ �ÿ� �ð��� ���ߴµ�, RealTime���� ���� ������ �ð��� ���缭 �˸�â�� �� ä�� ������� �����Ƿ� ������ �ÿ� �˸��� ���� ��� ���ٰ� ����� �� �ְ� RealTime���� ����
    #endregion

    private void Awake()
    {
        achieves = (Achievement[])Enum.GetValues(typeof(Achievement));      // GetValues�� type�� �Ϲ� Array�̹Ƿ�, Achievement �迭�� �ٲٱ� ���� �տ� (Achievement[])�� �ٿ���� ��

        notice_time = new WaitForSecondsRealtime(5);       // �˸�â �ð�

        if (!PlayerPrefs.HasKey("MyData"))      // MyData��� Ű�� ���� ���� �ʴٸ� Init�Լ� ���� (��� ���� �ʱ�ȭ)
        {
            Init();     // ���� �ʱ�ȭ
        }
    }

    private void Start()
    {
        UnlockChar();   // ���� Ÿ��Ʋ�� ���ư� ��(���� �ٽ� �ҷ����� ���), ĳ���Ͱ� �رݵǾ��ִ��� Ȯ��
    }

    #region ���� �ʱ�ȭ ~ �̰� �׽�Ʈ�ϱ� ���ؼ��� Unity - Edit - Clear All PlayerPrefs�� ������ ��� �����͸� �ʱ�ȭ��������Ѵ�. �� ���̶� �����ϸ� MyData�� �����Ͱ� ���� ����
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 0);        // ���� ���� ������ Ű ����

        foreach (Achievement achievement in achieves)       // Achievement�迭�� ��� ��� ������ �ʱ�ȭ
        {
            PlayerPrefs.SetInt(achievement.ToString(), 0);
        }
    }
    #endregion

    private void LateUpdate()       // ������ �����Ǿ����� ���������� ���⼭ Ȯ��
    {
        foreach (Achievement achievement in achieves)       // Achievement�迭�� ��� ��� �������� ���Ǹ����ߴ��� Ȯ��
        {
            CheckAchieve(achievement);
        }
    }

    #region �����޼� ���� ���� ��, ���س��� ���� ���ǿ� �ش��ϴ� PlayerPrefs�� true�� ����� �Լ�
    void CheckAchieve(Achievement achieve)
    {
        bool isAchieve = false;

        #region ���� �޼� ���ǵ� ����
        switch (achieve)
        {
            case Achievement.UnlockChar1:
                {
                    isAchieve = GameManager.instance.curKill >= 10;     // �������� �޼� ��, isAchieve�� true �־���
                    break;
                }
            case Achievement.UnlockChar2:
                {
                    isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                    break;
                }
        }
        #endregion

        #region �������� �޼� �� �ൿ
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)       // isAchieve�� true�̸鼭 �� ������ �رݵ��� ���� ������ ���, ������ �ر�
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            #region �������� �޼� �� �˸�â
            for (int i = 0; i < ui_Notice.transform.childCount; i++)        // �˸�â�� �ڽĿ�����Ʈ���� ��ȸ
            {
                bool isActive = i == (int)achieve;      // �˸�â �ڽĿ�����Ʈ���� i��° ������Ʈ��, �� if���� �����ϸ�(�رݾȵȻ��� -> �ر�) isActive�� true�� ����

                ui_Notice.transform.GetChild(i).gameObject.SetActive(isActive);         // isActive�� ����, i��° ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ
            }

            StartCoroutine(NoticeRoutine());
            #endregion
        }
#endregion
    }

    IEnumerator NoticeRoutine()
    {
        ui_Notice.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);

        yield return notice_time;

        ui_Notice.SetActive(false);
    }
    #endregion

    #region �ش��ϴ� PlayerPrefs�� true�̸�, ĳ���� ����� ������ �ϴ� �Լ�
    void UnlockChar()
    {
        for (int i = 0; i < char_Locked.Length; i++)        // ��� ��ư �迭�� ��ȸ�ϸ� �ε����� �ش��ϴ� ĳ���� �̸� ��������
        {
            string achieveName = achieves[i].ToString();

            // bool������ PlayerPefs�� ���ϴ� ������ 1�̸� true��, 0�̸� false�� ���� (PlayerPrefs.GetInt������ �����̹Ƿ� �� ������ �ʿ�)
            // PlayerPrefs.GetInt(a)�� 1�� �����ϸ�, isUnlock�� true�� �ִ´ٴ� �ǹ�
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;

            char_Locked[i].SetActive(!isUnlock);
            char_UnLocked[i].SetActive(isUnlock);
        }
    }
    #endregion
}
