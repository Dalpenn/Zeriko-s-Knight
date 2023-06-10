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
        achieves = (Achievement[])Enum.GetValues(typeof(Achievement));      // GetValues는 type이 일반 Array이므로, Achievement 배열로 바꾸기 위해 앞에 (Achievement[])를 붙여줘야 함

        if (!PlayerPrefs.HasKey("MyData"))      // MyData라는 키를 갖고 있지 않다면 Init함수 실행 (모든 업적 초기화)
        {
            Init();
        }
    }


    private void Start()
    {
        UnlockChar();
    }

    #region 업적 초기화 ~ 이걸 테스트하기 위해서는 Unity - Edit - Clear All PlayerPrefs를 눌러서 모든 데이터를 초기화시켜줘야한다. 한 번이라도 게임하면 MyData에 데이터가 들어가기 때문
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);        // 업적 관련 데이터 키 설정

        foreach (Achievement achievement in achieves)       // Achievement배열에 담긴 모든 변수들 초기화
        {
            PlayerPrefs.SetInt(achievement.ToString(), 0);
        }
    }
    #endregion


    void UnlockChar()
    {
        for (int i = 0; i < char_Locked.Length; i++)        // 잠금 버튼 배열을 순회하며 인덱스에 해당하는 업적 이름 가져오기
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;       // bool변수에 PlayerPefs의 원하는 변수가 1이면 true를, 0이면 false를 넣음 (PlayerPrefs.GetInt변수는 정수이므로 이 과정이 필요)
            char_Locked[i].SetActive(!isUnlock);
            char_UnLocked[i].SetActive(isUnlock);
        }
    }
}
