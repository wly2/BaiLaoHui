using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomRecord : MonoBehaviour
{
    public List<GameObject> list;

    void Start()
    {
        if (RoomRecordManager.Instance.List.Count > 0)
        {
            for (int i = 0; i < RoomRecordManager.Instance.List.Count; i++)
            {
                list[i].GetComponentsInChildren<Text>()[1].text = RoomRecordManager.Instance.List[i].playerName;
                list[i].GetComponentsInChildren<Text>()[2].text =
                    RoomRecordManager.Instance.List[i].playCount.ToString();
                list[i].GetComponentsInChildren<Text>()[3].text = RoomRecordManager.Instance.List[i].integral;
            }
        }
    }
}