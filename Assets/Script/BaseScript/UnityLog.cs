using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using UnityEngine.Events;

public class UnityLog : MonoBehaviour
{
    [SerializeField] ScrollRectList myScrollRect;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject rankItem;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void InitScroll()
    {
        var speakList = new List<string>(); // MyDebug.list;
        for (int i = MyDebug.list.Count - 1; i >= 0; i--)
        {
            speakList.Add(MyDebug.list[i]);
        }

        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (rankItem != null)
            {
                 var cellObj = Instantiate(rankItem.gameObject);
                 var handler = action;
                if (handler != null)
                    handler(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            o.item.GetComponent<Text>().text = speakList[index];
        };
        myScrollRect.Init(speakList.Count);
    }

    void OnValueChange(Vector2 pos)
    {
        myScrollRect.OnValueChange(pos);
    }

    bool isShow;

    public void ShowV()
    {
       // NetUtil.ShotSceneTexture();
        //if (isShow)
        //{
        //    scrollRect.gameObject.SetActive(false);
        //    isShow = false;
        //}
        //else
        //{
        //    scrollRect.gameObject.SetActive(true);
        //    isShow = true;
        //    InitScroll();
        //}
    }
}