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
                    break;
                }
            case InfoType.Level:
                {
                    break;
                }
            case InfoType.Kill:
                {
                    break;
                }
            case InfoType.Time:
                {
                    break;
                }
            case InfoType.Hp:
                {
                    break;
                }
        }
    }
}
