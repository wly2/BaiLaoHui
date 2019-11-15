using UnityEngine;
using UnityEngine.UI;

public class DialogPanelScript : MonoBehaviour
{
    public delegate void ButtonOnClick();

    public ButtonOnClick onOkClickListener; //确认键监听
    public ButtonOnClick onCancleClickListener; //取消键监听
    public Text title;
    public Text msg;

    private void SetTitle(string titleStr)
    {
        title.text = titleStr;
    }

    private void SetMsg(string msgStr)
    {
        msg.text = msgStr;
    }

    public void ClickOk()
    {
        onOkClickListener();
        DialogPanelScript self = GetComponent<DialogPanelScript>();
        Destroy(self.title);
        Destroy(self.msg);
        Destroy(this);
        Destroy(gameObject);
    }

    public void ClickCancle()
    {
        onCancleClickListener();
        DialogPanelScript self = GetComponent<DialogPanelScript>();
        Destroy(self.title);
        Destroy(self.msg);
        Destroy(this);
        Destroy(gameObject);
    }

    public void SetContent(string titlestr, string msgstr, bool flag, ButtonOnClick yesCallBack,
        ButtonOnClick noCallBack)
    {
        SetTitle(titlestr);
        SetMsg(msgstr);
        if (yesCallBack != null)
        {
            onOkClickListener += yesCallBack;
        }

        if (noCallBack != null)
        {
            onCancleClickListener += noCallBack;
        }
    }
}