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
    }        // ������ ������

    [Header("# Main Info")]
    public ItemType itemType;       // ���� �����س��� enum�� ����ϱ� ���� ����
    public int itemID;              // ��ų �����ϴ� ID
    public string itemName;         // ��ų �̸�
    public string itemDesc;         // ��ų ����
    public Sprite itemIcon;         // ���� â�� ���� ��ų ������

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
