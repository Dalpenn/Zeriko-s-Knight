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
        titleTxt.text = "���� ���� ����!";
    }

    public void DungeonFailed()
    {
        titleTxt.text = "���� ���� ����..";
    }
}
