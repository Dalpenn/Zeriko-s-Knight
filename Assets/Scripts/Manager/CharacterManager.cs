using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region 변수들
    public GameObject[] char_Locked;
    public GameObject[] char_UnLocked;
    public GameObject ui_Notice;

    enum Achievement { UnlockChar1, UnlockChar2 }
    Achievement[] achieves;

    WaitForSecondsRealtime notice_time;    // 레벨업 시에 시간을 멈추는데, RealTime으로 하지 않으면 시간이 멈춰서 알림창이 뜬 채로 사라지지 않으므로 레벨업 시에 알림이 떠도 잠깐 떴다가 사라질 수 있게 RealTime으로 설정
    #endregion

    private void Awake()
    {
        achieves = (Achievement[])Enum.GetValues(typeof(Achievement));      // GetValues는 type이 일반 Array이므로, Achievement 배열로 바꾸기 위해 앞에 (Achievement[])를 붙여줘야 함

        notice_time = new WaitForSecondsRealtime(5);       // 알림창 시간

        if (!PlayerPrefs.HasKey("MyData"))      // MyData라는 키를 갖고 있지 않다면 Init함수 실행 (모든 업적 초기화)
        {
            Init();     // 업적 초기화
        }
    }

    private void Start()
    {
        UnlockChar();   // 게임 타이틀로 돌아갈 시(씬을 다시 불러오는 경우), 캐릭터가 해금되어있는지 확인
    }

    #region 업적 초기화 ~ 이걸 테스트하기 위해서는 Unity - Edit - Clear All PlayerPrefs를 눌러서 모든 데이터를 초기화시켜줘야한다. 한 번이라도 게임하면 MyData에 데이터가 들어가기 때문
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 0);        // 업적 관련 데이터 키 설정

        foreach (Achievement achievement in achieves)       // Achievement배열에 담긴 모든 변수들 초기화
        {
            PlayerPrefs.SetInt(achievement.ToString(), 0);
        }
    }
    #endregion

    private void LateUpdate()       // 업적이 만족되었는지 지속적으로 여기서 확인
    {
        foreach (Achievement achievement in achieves)       // Achievement배열에 담긴 모든 변수들을 조건만족했는지 확인
        {
            CheckAchieve(achievement);
        }
    }

    #region 업적달성 조건 만족 시, 정해놓은 업적 조건에 해당하는 PlayerPrefs를 true로 만드는 함수
    void CheckAchieve(Achievement achieve)
    {
        bool isAchieve = false;

        #region 업적 달성 조건들 설정
        switch (achieve)
        {
            case Achievement.UnlockChar1:
                {
                    isAchieve = GameManager.instance.curKill >= 10;     // 업적조건 달성 시, isAchieve에 true 넣어줌
                    break;
                }
            case Achievement.UnlockChar2:
                {
                    isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                    break;
                }
        }
        #endregion

        #region 업적조건 달성 시 행동
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)       // isAchieve가 true이면서 그 업적이 해금되지 않은 상태일 경우, 업적을 해금
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            #region 업적조건 달성 시 알림창
            for (int i = 0; i < ui_Notice.transform.childCount; i++)        // 알림창의 자식오브젝트들을 순회
            {
                bool isActive = i == (int)achieve;      // 알림창 자식오브젝트들의 i번째 오브젝트가, 위 if문을 만족하면(해금안된상태 -> 해금) isActive에 true를 넣음

                ui_Notice.transform.GetChild(i).gameObject.SetActive(isActive);         // isActive에 따라, i번째 오브젝트를 활성화/비활성화
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

    #region 해당하는 PlayerPrefs가 true이면, 캐릭터 잠금이 열리게 하는 함수
    void UnlockChar()
    {
        for (int i = 0; i < char_Locked.Length; i++)        // 잠금 버튼 배열을 순회하며 인덱스에 해당하는 캐릭터 이름 가져오기
        {
            string achieveName = achieves[i].ToString();

            // bool변수에 PlayerPefs의 원하는 변수가 1이면 true를, 0이면 false를 넣음 (PlayerPrefs.GetInt변수는 정수이므로 이 과정이 필요)
            // PlayerPrefs.GetInt(a)가 1과 동일하면, isUnlock에 true를 넣는다는 의미
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;

            char_Locked[i].SetActive(!isUnlock);
            char_UnLocked[i].SetActive(isUnlock);
        }
    }
    #endregion
}
