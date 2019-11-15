public class RecordItem
{
    public int id;
    public int roomId;
    public string roomOwn;
    public int gameInnings;
    public string gameTime;

    public RecordItem()
    {
    }

    public RecordItem(int id, int roomId, string roomOwn, int gameInnings, string gameTime)
    {
        this.id = id;
        this.roomId = roomId;
        this.roomOwn = roomOwn;
        this.gameInnings = gameInnings;
        this.gameTime = gameTime;
    }
}