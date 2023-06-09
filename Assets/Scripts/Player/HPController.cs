using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    #region 변수들
    public int curHp;
    public int maxHp;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    #endregion

    #region HP바 동기화 코드
    public void HPBarSync()
    {
        curHp = GameManager.instance.curHp;
        maxHp = GameManager.instance.maxHp;

        if (curHp > maxHp)
        {
            curHp = maxHp;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < curHp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHp)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    #endregion
}