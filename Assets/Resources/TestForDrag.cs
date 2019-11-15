using UnityEngine;
using UnityEngine.EventSystems;
using AssemblyCSharp;

public class TestForDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    //创建一个Gameobject作为拖拽时被拖拽对象的代替品
    private GameObject drag_icon;

    //Box1和Box3里的Image拖拽给他  Box2空的
    public GameObject drag_icon2;


    private void Start()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        MyDebug.Log(eventData.delta.x);
        //并将拖拽时的坐标给予被拖拽对象的代替品
        //Vector3 pos;
        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(drag_icon.GetComponent<RectTransform>(),
        //    eventData.position, Camera.main, out pos))
        //{
        //    drag_icon.transform.position = pos;
        //}

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ////代替品实例化
        //drag_icon = new GameObject("icon");
        //drag_icon.transform.SetParent(GameObject.Find("Canvas").transform, false);
        //drag_icon.AddComponent<RectTransform>();
        //var img = drag_icon.AddComponent<Image>();
        //img.sprite = this.GetComponent<Image>().sprite;

        ////防止拖拽结束时，代替品挡住了准备覆盖的对象而使得 OnDrop（） 无效
        //CanvasGroup group = drag_icon.AddComponent<CanvasGroup>();
        //group.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ////拖拽结束，销毁代替品
        //if (drag_icon)
        //{
        //    Destroy(drag_icon2);
        //    Destroy(drag_icon);
        //}
    }

    public void OnDrop(PointerEventData eventData)
    {
        ////根据代替品的信息，改变当前对象的Sprite。
        //var obj = eventData.pointerDrag;
        //this.GetComponent<Image>().sprite = obj.GetComponent<Image>().sprite;
    }
}