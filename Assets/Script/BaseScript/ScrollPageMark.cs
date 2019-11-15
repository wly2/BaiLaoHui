using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollPageMark : MonoBehaviour
{
    public ScrollPage scrollPage;
    public ToggleGroup toggleGroup;
    public Toggle togglePrefab;
    public List<Toggle> toggleList = new List<Toggle>();

    public void OnScrollPageChanged(int pageCount, int currentPageIndex)
    {
        if (pageCount != toggleList.Count)
        {
            if (pageCount > toggleList.Count)
            {
                int cc = pageCount - toggleList.Count;
                for (int i = 0; i < cc; i++)
                {
                    toggleList.Add(CreateToggle());
                }
            }
            else if (pageCount < toggleList.Count)
            {
                while (toggleList.Count > pageCount)
                {
                    Toggle t = toggleList[toggleList.Count - 1];
                    toggleList.Remove(t);
                    DestroyImmediate(t.gameObject);
                }
            }
        }

        if (currentPageIndex >= 0)
        {
            toggleList[currentPageIndex].isOn = true;
        }
    }

    Toggle CreateToggle()
    {
        Toggle t = Instantiate(togglePrefab);
        t.gameObject.SetActive(true);
        t.transform.SetParent(toggleGroup.transform);
        t.transform.localScale = Vector3.one;
        t.transform.localPosition = Vector3.zero;
        return t;
    }
}