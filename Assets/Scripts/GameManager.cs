using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region º¯¼öµé
    public static GameManager instance;
    public PoolManager poolMng;
    public Player player;
    #endregion

    void Awake()
    {
        instance = this;
    }
}
