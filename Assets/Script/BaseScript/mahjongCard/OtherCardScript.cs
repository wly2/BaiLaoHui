using UnityEngine;

public class OtherCardScript : MonoBehaviour
{
    private int cardPoint;

    public void SetPoint(int _cardPoint)
    {
        cardPoint = _cardPoint; //设置所有牌指针
    }

    public int GetPoint()
    {
        return cardPoint;
    }
}