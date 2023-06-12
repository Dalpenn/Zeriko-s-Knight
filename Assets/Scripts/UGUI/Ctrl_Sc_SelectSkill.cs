using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_Sc_SelectSkill : MonoBehaviour
{
    #region ������
    RectTransform rect;
    Item[] items;
    #endregion

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);        // ��Ȱ��ȭ�� �����۵� ������, ��� �ϴ� Ȱ��ȭ�ǵ��� true�� ����
    }

    public void ShowScreen_Skill()
    {
        ShowSkillList();
        rect.localScale = Vector3.one;
        GameManager.instance.GamePause();

        AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);
        AudioManager.instance.EffectBGM(true);         // ������ �ÿ��� ������� ��� ����
    }

    public void HideScreen_Skill()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.GameResume();

        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        AudioManager.instance.EffectBGM(false);         // ������ �� ��ų�����ϸ� �ٽ� ������� ���
    }

    public void SelectStartSkill(int i)     // ó�� �������ڸ��� ���� ��� �Լ�
    {
        items[i].OnClick();
    }

    void ShowSkillList()
    {
        // 1. ��� ��ų ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. �� �� ���� 3�� ��ų Ȱ��ȭ
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

            // 3. ���� ��ų�� �Һ� ������ ��ü
            if (ranItem.level == ranItem.data.dmgs.Length || ranItem.level == ranItem.data.passiveAmounts.Length)
            {
                items[4].gameObject.SetActive(true);
                // �Һ�������� 5~7��° ���� 3���� ����� items[Random.Range(5, 8)].gameObject.SetActive(true); �̷��� �ϸ� ��
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
