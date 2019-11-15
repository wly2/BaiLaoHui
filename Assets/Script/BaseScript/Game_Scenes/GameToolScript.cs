using System;
using System.Collections.Generic;
using UnityEngine;

public class GameToolScript
{
    public GameToolScript()
    {
    }

    /// <summary>
    /// Sets the other card object position.
    /// </summary>
    /// <param name="tempList">Temp list.</param>
    /// <param name="initDiretion">Init diretion.</param> 1- 碰，2-杠
    public void SetOtherCardObjPosition(List<GameObject> tempList, String initDiretion)
    {

        for (int i = 1; i < tempList.Count; i++)
        {

            switch (initDiretion)
            {
                case DirectionEnum.Top: //上
                    tempList[i].transform.localPosition = new Vector3((tempList.Count / 2.0f - i) * 2.16f, 0); //位置
                    break;
                case DirectionEnum.Left: //左
                    tempList[i].transform.localPosition = new Vector3(0, 0, (tempList.Count / 2.0f - i) * 2.16f); //位置
                    break;
                case DirectionEnum.Right: //右
                    tempList[i].transform.localPosition = new Vector3(0, 0, (i - tempList.Count / 2.0f) * 2.16f); //位置
                    break;
            }
        }
    }
}

public class DirectionEnum
{
    public const string Bottom = "B";
    public const string Right = "R";
    public const string Top = "T";
    public const string Left = "L";
}