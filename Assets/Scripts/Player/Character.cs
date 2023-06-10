using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float movSpd
    {
        get { return GameManager.instance.playerID == 0 ? 1.1f : 1f; }      // playerID가 0이면 1.1을, 0이 아니면 1을 넣도록 설정
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
        get { return GameManager.instance.playerID == 3 ? 1 : 0; }          // playerID가 3이면 1을, 아니면 0을 넣도록 설정
    }
}
