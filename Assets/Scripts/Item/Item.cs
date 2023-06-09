using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    #region 변수들
    public ItemData data;
    public Weapon weapon;
    public Passives_Player passives;

    // 아이템 관련 변수들
    public int level;

    // 캔버스 출력 관련 변수들
    Image icon;
    Text txtLevel;
    Text txtName;
    Text txtDesc;
    #endregion

    private void Awake()
    {
        // 자식 오브젝트의 아이콘을 컨트롤해야하기 때문에 모든 자식 컴포넌트를 가져오는 GetComponentsInChildern을 사용
        // GetComponents는 배열 형태 / indexo 0은 자기자신이므로, index 1에 위치한 아이콘 컴포넌트를 가져옴
        icon = GetComponentsInChildren<Image>()[2];
        icon.sprite = data.itemIcon;

        #region 텍스트 초기화 관련
        Text[] txts = GetComponentsInChildren<Text>();
        txtLevel = txts[0];         // text순서대로 index를 넣는다
        txtName = txts[1];
        txtDesc = txts[2];
        #endregion

        txtName.text = data.itemName;       // 스킬 이름은 처음에 딱 한번만 쓰면 되므로 여기서 적는다
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
//================================================================ 무기 관련 레벨업 버튼
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                {
                    #region 무기 레벨 0인 경우, 무기 생성 코드
                    if (level == 0)         // 무기가 레벨 0일때 생성
                    {
                        GameObject newWeapon = new GameObject();                 // 새로운 게임오브젝트인 newWeapon을 생성

                        // 생성한 newWeapon오브젝트에 Weapon스크립트를 추가
                        // AddComponent는 자신이 방금 추가한 스크립트를 반환해주기도 하므로, 미리 생성해놓은 Weapon weapon에 그대로 newWeapon을 넣어도 됨
                        weapon = newWeapon.AddComponent<Weapon>();

                        weapon.Init(data);
                    }
                    #endregion

                    #region 무기 레벨업 시 코드
                    else
                    {
                        float nextDmg = weapon.dmg * data.dmgs[level];      // 레벨업 시, (현재 무기 dmg) x (data에 넣어둔 레벨업 시 증가수치) 만큼 dmg증가 ~ 0.5면 50%, 0.7이면 70%
                        //Debug.Log("레벨업으로 " + data.itemName + " 의 현재dmg " + weapon.dmg + " 가 " + data.dmgs[level] * 100 + " % 증가 = 추가된 데미지: " + nextDmg);
                        int nextCount = data.counts[level];                     // 레벨업 시, counts가 "추가"됨
                        weapon.LevelUp(nextDmg, nextCount);
                    }
                    #endregion

                    level++;

                    break;
                }
//================================================================ 패시브 관련 레벨업 버튼    // 최대체력증가 하나 만들어보자
            case ItemData.ItemType.PlayerPassive_Rate:
            case ItemData.ItemType.PlayerPassive_MovSpd:
                {
                    #region 패시브 레벨 0인 경우 패시브 생성 코드
                    if (level == 0)
                    {
                        GameObject newPassive = new GameObject();
                        passives = newPassive.AddComponent<Passives_Player>();
                        passives.Init(data);
                    }
                    #endregion

                    #region 패시브 레벨업 시 코드
                    else
                    {
                        float nextRate = data.passiveAmounts[level];

                        passives.LevelUp(nextRate);
                    }
                    #endregion

                    level++;

                    break;
                }

//================================================================ 소모품은 관련 버튼 ~ 횟수제한을 두면 안되므로 필요한 곳에만 level++ 써주기
            case ItemData.ItemType.Heal:
                {
                    GameManager.instance.curHp = GameManager.instance.maxHp;
                    //GameManager.instance.hpCtrl.HPBarSync();

                    break;
                }
        }

        #region 스킬이 최대레벨 도달 시, 비활성화 되는 코드
        if (level == data.dmgs.Length)
        {
            GetComponent<Button>().interactable = false;
        }
        #endregion
    }
}
