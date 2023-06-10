using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float movSpd
    {
        get { return GameManager.instance.playerID == 0 ? 1.1f : 1f; }      // playerID�� 0�̸� 1.1��, 0�� �ƴϸ� 1�� �ֵ��� ����
    }
    
    public static float RotSpd
    {
        get { return GameManager.instance.playerID == 1 ? 1.1f : 1f; }
    }

    public static float atkSpd
    {
        get { return GameManager.instance.playerID == 1 ? 0.9f : 1f; }
    }

    public static float dmg
    {
        get { return GameManager.instance.playerID == 2 ? 1.1f : 1f; }
    }

    public static int count
    {
        get { return GameManager.instance.playerID == 3 ? 1 : 0; }          // playerID�� 3�̸� 1��, �ƴϸ� 0�� �ֵ��� ����
    }
}
