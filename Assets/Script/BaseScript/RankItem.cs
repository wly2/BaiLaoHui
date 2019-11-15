public class RankItem
{
    public UnityEngine.Sprite heard;
    public string name;
    public long winCount;
    public int roomCardCount;

    public RankItem()
    {
    }

    public RankItem(string name, long winCount, int roomCardCount, UnityEngine.Sprite icon = null)
    {
        heard = icon;
        this.name = name;
        this.winCount = winCount;
        this.roomCardCount = roomCardCount;
    }
}