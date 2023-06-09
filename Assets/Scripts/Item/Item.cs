using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    #region ������
    public ItemData data;
    public Weapon weapon;
    public Passives_Player passives;

    // ������ ���� ������
    public int level;

    // ĵ���� ��� ���� ������
    Image icon;
    Text txtLevel;
    Text txtName;
    Text txtDesc;
    #endregion

    private void Awake()
    {
        // �ڽ� ������Ʈ�� �������� ��Ʈ���ؾ��ϱ� ������ ��� �ڽ� ������Ʈ�� �������� GetComponentsInChildern�� ���
        // GetComponents�� �迭 ���� / indexo 0�� �ڱ��ڽ��̹Ƿ�, index 1�� ��ġ�� ������ ������Ʈ�� ������
        icon = GetComponentsInChildren<Image>()[2];
        icon.sprite = data.itemIcon;

        #region �ؽ�Ʈ �ʱ�ȭ ����
        Text[] txts = GetComponentsInChildren<Text>();
        txtLevel = txts[0];         // text������� index�� �ִ´�
        txtName = txts[1];
        txtDesc = txts[2];
        #endregion

        txtName.text = data.itemName;       // ��ų �̸��� ó���� �� �ѹ��� ���� �ǹǷ� ���⼭ ���´�
    }

    void OnEnable()
    {
        txtLevel.text = "Lv." + (level);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                {
                    txtDesc.text = string.Format(data.itemDesc, data.dmgs[level] * 100, data.counts[level]);

                    break;
                }
            case ItemData.ItemType.PlayerPassive_Rate:
            case ItemData.ItemType.PlayerPassive_MovSpd:
                {
                    txtDesc.text = string.Format(data.itemDesc, data.passiveAmounts[level]);

                    break;
                }
            default: 
                {
                    txtDesc.text = string.Format(data.itemDesc);

                    break;
                }
        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
//================================================================ ���� ���� ������ ��ư
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                {
                    #region ���� ���� 0�� ���, ���� ���� �ڵ�
                    if (level == 0)         // ���Ⱑ ���� 0�϶� ����
                    {
                        GameObject newWeapon = new GameObject();                 // ���ο� ���ӿ�����Ʈ�� newWeapon�� ����

                        // ������ newWeapon������Ʈ�� Weapon��ũ��Ʈ�� �߰�
                        // AddComponent�� �ڽ��� ��� �߰��� ��ũ��Ʈ�� ��ȯ���ֱ⵵ �ϹǷ�, �̸� �����س��� Weapon weapon�� �״�� newWeapon�� �־ ��
                        weapon = newWeapon.AddComponent<Weapon>();

                        weapon.Init(data);
                    }
                    #endregion

                    #region ���� ������ �� �ڵ�
                    else
                    {
                        float nextDmg = weapon.dmg * data.dmgs[level];      // ������ ��, (���� ���� dmg) x (data�� �־�� ������ �� ������ġ) ��ŭ dmg���� ~ 0.5�� 50%, 0.7�̸� 70%
                        //Debug.Log("���������� " + data.itemName + " �� ����dmg " + weapon.dmg + " �� " + data.dmgs[level] * 100 + " % ���� = �߰��� ������: " + nextDmg);
                        int nextCount = data.counts[level];                     // ������ ��, counts�� "�߰�"��
                        weapon.LevelUp(nextDmg, nextCount);
                    }
                    #endregion

                    level++;

                    break;
                }
//================================================================ �нú� ���� ������ ��ư    // �ִ�ü������ �ϳ� ������
            case ItemData.ItemType.PlayerPassive_Rate:
            case ItemData.ItemType.PlayerPassive_MovSpd:
                {
                    #region �нú� ���� 0�� ��� �нú� ���� �ڵ�
                    if (level == 0)
                    {
                        GameObject newPassive = new GameObject();
                        passives = newPassive.AddComponent<Passives_Player>();
                        passives.Init(data);
                    }
                    #endregion

                    #region �нú� ������ �� �ڵ�
                    else
                    {
                        float nextRate = data.passiveAmounts[level];

                        passives.LevelUp(nextRate);
                    }
                    #endregion

                    level++;

                    break;
                }

//================================================================ �Ҹ�ǰ�� ���� ��ư ~ Ƚ�������� �θ� �ȵǹǷ� �ʿ��� ������ level++ ���ֱ�
            case ItemData.ItemType.Heal:
                {
                    GameManager.instance.curHp = GameManager.instance.maxHp;
                    //GameManager.instance.hpCtrl.HPBarSync();

                    break;
                }
        }

        #region ��ų�� �ִ뷹�� ���� ��, ��Ȱ��ȭ �Ǵ� �ڵ�
        if (level == data.dmgs.Length)
        {
            GetComponent<Button>().interactable = false;
        }
        #endregion
    }
}
