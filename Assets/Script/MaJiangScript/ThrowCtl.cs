using System.Collections.Generic;
using UnityEngine;

public class ThrowCtl : MonoBehaviour
{
    private readonly List<ThrowItem> list = new List<ThrowItem>();
    public int dir;

    public void AddItem(ThrowItem item)
    {
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        list.Add(item);
        GameObjectPosition();
    }

    public int Count
    {
        get { return list.Count; }
    }

    public void GameObjectPosition()
    {
        switch (dir)
        {
            case 0:
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                    {
                        list[i].transform.localPosition =
                            new Vector3(list[i - 1].transform.localPosition.x - list[i - 1].gameObjectSize - 1, 0, 0);
                    }
                    else
                    {
                        list[i].transform.localPosition = Vector3.zero;
                    }
                }

                break;
            case 1:
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                    {
                        list[i].transform.localPosition = new Vector3(0, 0,
                            list[i - 1].transform.localPosition.z - list[i - 1].gameObjectSize - 1);
                    }
                    else
                    {
                        list[i].transform.localPosition = Vector3.zero;
                    }
                }

                break;
            case 2:
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                    {
                        list[i].transform.localPosition =
                            new Vector3(list[i - 1].transform.localPosition.x + list[i - 1].gameObjectSize + 1, 0, 0);
                    }
                    else
                    {
                        list[i].transform.localPosition = Vector3.zero;
                    }
                }

                break;
            case 3:
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                    {
                        list[i].transform.localPosition = new Vector3(0, 0,
                            list[i - 1].transform.localPosition.z + list[i - 1].gameObjectSize + 1);
                    }
                    else
                    {
                        list[i].transform.localPosition = Vector3.zero;
                    }
                }

                break;
        }
    }

    public int GetPengCard(int cardPoint)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].pengCardPoint == cardPoint)
                return i;
        }

        return -1;
    }

    public void RefreshGang(int index)
    {
        list[index].AddCard();
        GameObjectPosition();
    }

    public void Clear()
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }

        list.Clear();
    }
}