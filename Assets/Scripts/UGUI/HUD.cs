using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region 변수들
    public enum InfoType { Exp, Level, Kill, Time, Hp }
    public InfoType type;

    Text myTxt;
    Slider mySlider;
    #endregion

    private void Awake()
    {
        myTxt = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()       // Update에서 연산이 다 이루어질때쯤 하는 Update
    {
        switch (type)
        {
            case InfoType.Exp:
                {
                    float curExp = GameManager.instance.curExp;
                    float maxExp = GameManager.instance.nextExp[GameManager.instance.level];

                    mySlider.value = curExp / maxExp;       // 경험치바에 현재 경험치 % 나타내기

                    break;
                }
            case InfoType.Level:
                {
                    //float lvPercent = ((float)GameManager.instance.curExp / (float)GameManager.instance.nextExp[GameManager.instance.level]) * 100;       //경험치 퍼센트 변수

                    // 원하는 변수의 숫자값을 지정된 형태의 문자열로 만들어주는 함수
                    // Format("원하는 문자열 {0:소수점지정}", 넣을 변수)
                    // 넣고싶은 변수를 지정한 소수점 자릿수로 문자열과 함께 { } 안에 넣어줌 ~ F0 : 소수점 0자리 / D0 : 0자리 숫자로 유지 (1일때는 01로 출력됨)
                    myTxt.text = string.Format("Level {0:F0}", GameManager.instance.level);
                    //myTxt.text = string.Format("Level {0:F0}   ({1:F1}%)", GameManager.instance.level, lvPercent);        // 레벨 옆에 경험치 퍼센트도 나타냄

                    break;
                }
            case InfoType.Kill:
                {
                    myTxt.text = string.Format("{0:F0} / {1:F0}", GameManager.instance.curKill, GameManager.instance.nextKill[GameManager.instance.stageLv]);

                    break;
                }
            case InfoType.Time:
                {
                    //float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                    float curTime = GameManager.instance.gameTime;
                    int min = Mathf.FloorToInt(curTime / 60);        // 시간 "분" 구하기
                    int sec = Mathf.FloorToInt(curTime % 60);        // 시간 "초" 구하기 (60으로 나눈 나머지)

                    myTxt.text = string.Format("{0:D2} : {1:D2}", min, sec);

                    break;
                }
            case InfoType.Hp:
                {
                    float curHp = GameManager.instance.curHp;
                    float maxHp = GameManager.instance.maxHp;

                    mySlider.value = curHp / maxHp;       // 경험치바에 현재 경험치 % 나타내기

                    break;
                }
        }
    }
}
