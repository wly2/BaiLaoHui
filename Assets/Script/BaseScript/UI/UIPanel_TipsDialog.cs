using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel_TipsDialog : UIWindow
{
	public Text tipsText;
    public delegate void GameEvent();
    public GameEvent BtnOnClick;
    public void SetMes(string mes, GameEvent ClickOk=null)
    {
        BtnOnClick = ClickOk;

        tipsText.text = mes;
	}
    public override void CloseUI()
    {

        if (BtnOnClick != null)
            BtnOnClick();
        base.CloseUI();
    }
}