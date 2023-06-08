using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region ������
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

    private void LateUpdate()       // Update���� ������ �� �̷�������� �ϴ� Update
    {
        switch (type)
        {
            case InfoType.Exp:
                {
                    float curExp = GameManager.instance.curExp;
                    float maxExp = GameManager.instance.nextExp[GameManager.instance.level];

                    mySlider.value = curExp / maxExp;       // ����ġ�ٿ� ���� ����ġ % ��Ÿ����

                    break;
                }
            case InfoType.Level:
                {
                    //float lvPercent = ((float)GameManager.instance.curExp / (float)GameManager.instance.nextExp[GameManager.instance.level]) * 100;       //����ġ �ۼ�Ʈ ����

                    // ���ϴ� ������ ���ڰ��� ������ ������ ���ڿ��� ������ִ� �Լ�
                    // Format("���ϴ� ���ڿ� {0:�Ҽ�������}", ���� ����)
                    // �ְ���� ������ ������ �Ҽ��� �ڸ����� ���ڿ��� �Բ� { } �ȿ� �־��� ~ F0 : �Ҽ��� 0�ڸ� / D0 : 0�ڸ� ���ڷ� ���� (1�϶��� 01�� ��µ�)
                    myTxt.text = string.Format("Level {0:F0}", GameManager.instance.level);
                    //myTxt.text = string.Format("Level {0:F0}   ({1:F1}%)", GameManager.instance.level, lvPercent);        // ���� ���� ����ġ �ۼ�Ʈ�� ��Ÿ��

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
                    int min = Mathf.FloorToInt(curTime / 60);        // �ð� "��" ���ϱ�
                    int sec = Mathf.FloorToInt(curTime % 60);        // �ð� "��" ���ϱ� (60���� ���� ������)

                    myTxt.text = string.Format("{0:D2} : {1:D2}", min, sec);

                    break;
                }
            case InfoType.Hp:
                {
                    float curHp = GameManager.instance.curHp;
                    float maxHp = GameManager.instance.maxHp;

                    mySlider.value = curHp / maxHp;       // ����ġ�ٿ� ���� ����ġ % ��Ÿ����

                    break;
                }
        }
    }
}
