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

    #region 패시브 초기 설정값
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

    #region 패시브 레벨업
    public void LevelUp(float rate)
    {
        #region 패시브 레벨업 마다 복리로 속도 계산 (증가, 감소)
        rate_final_plus *= (1 + (rate / 100));
        rate_final_minus *= (1 - (rate / 100));
        #endregion

        this.rate = rate;
        Apply_PlayerPassive();      // 레벨업 시, 다시 모든 갖고있는 무기에 변경사항 적용
    }
    #endregion

    #region 플레이어 패시브를 타입에 맞게 적용하는 코드
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

    #region 플레이어 패시브들 ~ 종류는 ItemData의 enum에서 추가 / rate을 각각 원하는 방식으로 미리 설정
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

    #region 이동속도 증가
    void MoveSpdUp()
    {
        GameManager.instance.player.player_spd = GameManager.instance.player.player_initSpd * rate_final_plus;
    }
    #endregion
    #endregion
}
