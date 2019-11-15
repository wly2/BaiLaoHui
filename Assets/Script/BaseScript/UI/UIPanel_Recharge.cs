using UnityEngine;

public class UIPanel_Recharge : UIWindow
{
    public GameObject obj_ZhuanShi;
    public GameObject obj_YinDing;

    public void Buyclick(int goodId)
    {
        GamePayManager.instance.BuyRoomCard(goodId);
    }

    public void btnYinDing()
    {
        obj_YinDing.SetActive(true);
        obj_ZhuanShi.SetActive(false);
    }

    public void btnZhuanShi()
    {
        obj_YinDing.SetActive(false);
        obj_ZhuanShi.SetActive(true);
    }
}