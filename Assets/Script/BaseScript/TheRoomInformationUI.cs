using UnityEngine;
using UnityEngine.UI;

public class TheRoomInformationUI : MonoBehaviour
{
    public Text roomer;
    public Text startTime;
    public Text endTime;
    public Text wanFa;
    public Text xx;
    public Text count;
    public Text payer;
    public TheRoomInformation tri = new TheRoomInformation("asd", "16:59", "17:15", "血流成河", "xx", 4, "斗鱼");

    void Start()
    {
        roomer.text = tri.homeowners;
        startTime.text = tri.startTime;
        endTime.text = tri.endTime;
        wanFa.text = tri.type;
        xx.text = tri.xX;
        count.text = tri.innings.ToString();
        payer.text = tri.thePayer;
    }
}