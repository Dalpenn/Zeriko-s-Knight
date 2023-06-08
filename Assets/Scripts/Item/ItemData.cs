using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType 
    { 
        Melee, 
        Range, 
        PlayerPassive_Rate, 
        PlayerPassive_MovSpd, 
        Heal 
    }        // 아이템 유형들

    [Header("# Main Info")]
    public ItemType itemType;       // 위에 선언해놓은 enum을 사용하기 위한 변수
    public int itemID;              // 스킬 구별하는 ID
    public string itemName;         // 스킬 이름
    public string itemDesc;         // 스킬 설명
    public Sprite itemIcon;         // 선택 창에 나올 스킬 아이콘

    [Header("# Level Data")]
    public float baseDmg;
    public float[] dmgs;
    public int baseCount;
    public int[] counts;
    public float baseSpeed;
    public float[] speeds;
    public float[] passiveAmounts;

    [Header("# Weapon")]
    public GameObject projectile;
}
