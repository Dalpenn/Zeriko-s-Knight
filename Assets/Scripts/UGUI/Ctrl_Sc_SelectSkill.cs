using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_Sc_SelectSkill : MonoBehaviour
{
    #region 변수들
    RectTransform rect;
    Item[] items;
    #endregion

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);        // 비활성화된 아이템도 있으니, 모두 일단 활성화되도록 true로 설정
    }

    public void ShowScreen_Skill()
    {
        ShowSkillList();
        rect.localScale = Vector3.one;
        GameManager.instance.GamePause();

        AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);
        AudioManager.instance.EffectBGM(true);         // 레벨업 시에는 배경음악 잠시 멈춤
    }

    public void HideScreen_Skill()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.GameResume();

        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        AudioManager.instance.EffectBGM(false);         // 레벨업 후 스킬선택하면 다시 배경음악 재생
    }

    public void SelectStartSkill(int i)     // 처음 시작하자마자 무기 얻는 함수
    {
        items[i].OnClick();
    }

    void ShowSkillList()
    {
        // 1. 모든 스킬 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중 랜덤 3개 스킬 활성화
        int[] ran = new int[3];

        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2])
            {
                break;
            }
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            // 3. 만렙 스킬은 소비 템으로 대체
            if (ranItem.level == ranItem.data.dmgs.Length || ranItem.level == ranItem.data.passiveAmounts.Length)
            {
                items[4].gameObject.SetActive(true);
                // 소비아이템이 5~7번째 까지 3개인 사람은 items[Random.Range(5, 8)].gameObject.SetActive(true); 이렇게 하면 됨
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
