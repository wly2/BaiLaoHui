using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    ScrollRect rect;

    //页面：0，1，2，3  索引从0开始
    //每页占的比列：0/3=0  1/3=0.333  2/3=0.6666 3/3=1
    //float[] pages = { 0f, 0.333f, 0.6666f, 1f };
    readonly
        //页面：0，1，2，3  索引从0开始
        //每页占的比列：0/3=0  1/3=0.333  2/3=0.6666 3/3=1
        //float[] pages = { 0f, 0.333f, 0.6666f, 1f };
        List<float> pages = new List<float>();

    int currentPageIndex = -1;

    float time;

    //滑动速度
    float smooting = 4;

    //滑动的起始坐标
    float targethorizontal;

    //是否拖拽结束
    bool isDrag;

    /// <summary>
    /// 用于返回一个页码，-1说明page的数据为0
    /// </summary>
    // public System.Action<int, int> OnPageChanged;
    float startime;

    readonly float delay = 0.1f;
    float autoTime;

    void Start()
    {
        rect = transform.GetComponent<ScrollRect>();
        //rect.horizontalNormalizedPosition = 0;
        //UpdatePages();
        startime = Time.time;
    }

    void Update()
    {
        autoTime += Time.deltaTime;
        if (autoTime > 3)
        {
            smooting = 4;
            autoTime = 0;
            currentPageIndex++;
            if (currentPageIndex >= pages.Count)
            {
                smooting = 50;
                currentPageIndex = 0;
            }

            OnScroll();
        }

        if (Time.time < startime + delay) return;
        UpdatePages();
        //如果不判断。当在拖拽的时候要也会执行插值，所以会出现闪烁的效果
        //这里只要在拖动结束的时候。在进行插值
        if (!isDrag && pages.Count > 0)
        {
            if (currentPageIndex == 0)
                rect.horizontalNormalizedPosition = targethorizontal;
            else
                rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal,
                    Time.deltaTime * smooting);
        }

        time += Time.deltaTime;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        time = 0;
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        time = 0;
        var posX = rect.horizontalNormalizedPosition;
        var index = 0;
        //假设离第一位最近
        var offset = Mathf.Abs(pages[index] - posX);
        for (int i = 1; i < pages.Count; i++)
        {
            float temp = Mathf.Abs(pages[i] - posX);
            if (temp < offset)
            {
                index = i;
                //保存当前的偏移量
                //如果到最后一页。反翻页。所以要保存该值，如果不保存。你试试效果就知道
                offset = temp;
            }
        }

        if (index != currentPageIndex)
        {
            currentPageIndex = index;
            OnScroll();
        }

        /*
         因为这样效果不好。没有滑动效果。比较死板。所以改为插值
         */
        //rect.horizontalNormalizedPosition = page[index];
    }

    private void OnScroll()
    {
        autoTime = 0;
        targethorizontal = pages[currentPageIndex];
    }

    public void TimePages()
    {
        //for (int i = 1; i < pages.Count; i++)
        //{
        //        index = i;
        //        //保存当前的偏移量
        //        //如果到最后一页。反翻页。所以要保存该值，如果不保存。你试试效果就知道
        //}
        if (time >= 1)
        {
            var posX = rect.horizontalNormalizedPosition;
            var index = 0;
            //假设离第一位最近
            var offset = Mathf.Abs(pages[index] - posX);
            for (int i = 1; i < pages.Count; i++)
            {
                var temp = Mathf.Abs(pages[i] - posX);
                if (temp < offset)
                {
                    index = i;
                    //保存当前的偏移量
                    //如果到最后一页。反翻页。所以要保存该值，如果不保存。你试试效果就知道
                    offset = temp;
                }
            }

            if (index != currentPageIndex)
            {
                currentPageIndex = index;
                OnScroll();
            }

            time = 0;
        }

        /*
         因为这样效果不好。没有滑动效果。比较死板。所以改为插值
         */
        //rect.horizontalNormalizedPosition = page[index];
    }

    void UpdatePages()
    {
        // 获取子对象的数量
        var count = rect.content.childCount;
        var temp = 0;
        for (int i = 0; i < count; i++)
        {
            if (rect.content.GetChild(i).gameObject.activeSelf)
            {
                temp++;
            }
        }

        count = temp;
        if (pages.Count != count)
        {
            if (count != 0)
            {
                pages.Clear();
                for (int i = 0; i < count; i++)
                {
                    float page = 0;
                    if (count != 1)
                        page = i / ((float) (count - 1));
                    pages.Add(page);
                    //Debug.Log(i.ToString() + " page:" + page.ToString());
                }
            }

            OnEndDrag(null);
            //if (!isDrag)
            //{
            //    TimePages();
            //}
        }
    }
}