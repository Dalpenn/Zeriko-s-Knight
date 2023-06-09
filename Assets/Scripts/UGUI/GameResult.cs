using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    //public GameObject[] titles;
    public Text titleTxt;

    public void DungeonCleared()
    {
        titleTxt.text = "던전 공략 성공!";
    }

    public void DungeonFailed()
    {
        titleTxt.text = "던전 공략 실패..";
    }
}
