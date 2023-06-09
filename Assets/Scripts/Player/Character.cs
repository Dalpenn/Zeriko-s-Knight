using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float movSpd
    {
        get { return GameManager.instance.playerID == 0 ? 1.1f : 1f; }
    }
    
    public static float atkSpd
    {
        get { return GameManager.instance.playerID == 1 ? 1.1f : 1f; }
    }
    public static float dmg
    {
        get { return GameManager.instance.playerID == 2 ? 1.1f : 1f; }
    }
    public static float count
    {
        get { return GameManager.instance.playerID == 3 ? 1.1f : 1f; }
    }
}
