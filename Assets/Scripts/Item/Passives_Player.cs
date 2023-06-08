using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Passives_Player : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public float rate_final_plus;
    public float rate_final_minus;

    #region �нú� �ʱ� ������
    public void Init(ItemData data)
    {
        #region Basic Setting
        //====================================================================================
        name = "Passives_Player" + data.itemName;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        //====================================================================================
        #endregion

        #region Property Setting
        //====================================================================================
        type = data.itemType;
        rate = data.passiveAmounts[0];
        rate_final_plus = 1 + (rate / 100);
        rate_final_minus = 1 - (rate / 100);
        //====================================================================================
        #endregion

        Apply_PlayerPassive();
    }
    #endregion

    #region �нú� ������
    public void LevelUp(float rate)
    {
        #region �нú� ������ ���� ������ �ӵ� ��� (����, ����)
        rate_final_plus *= (1 + (rate / 100));
        rate_final_minus *= (1 - (rate / 100));
        #endregion

        this.rate = rate;
        Apply_PlayerPassive();      // ������ ��, �ٽ� ��� �����ִ� ���⿡ ������� ����
    }
    #endregion

    #region �÷��̾� �нú긦 Ÿ�Կ� �°� �����ϴ� �ڵ�
    void Apply_PlayerPassive()
    {
        switch (type) 
        {
            case ItemData.ItemType.PlayerPassive_Rate:
                {
                    AtkRateUp();

                    break;
                }
            case ItemData.ItemType.PlayerPassive_MovSpd:
                {
                    MoveSpdUp(); 
                    
                    break;
                }
        }
    }
    #endregion

    #region �÷��̾� �нú�� ~ ������ ItemData�� enum���� �߰� / rate�� ���� ���ϴ� ������� �̸� ����
    void AtkRateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons) 
        { 
            switch(weapon.id)
            {
                case 0:
                    {
                        weapon.speed = weapon.spd_init * rate_final_plus;

                        break;
                    }
                case 1:
                    {
                        weapon.speed = weapon.spd_init * rate_final_minus;

                        break;
                    }

                default: { break; }
            }
        }
    }

    #region �̵��ӵ� ����
    void MoveSpdUp()
    {
        GameManager.instance.player.player_spd = GameManager.instance.player.player_initSpd * rate_final_plus;
    }
    #endregion
    #endregion
}
