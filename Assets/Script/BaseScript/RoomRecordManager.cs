using System.Collections.Generic;

public class RoomRecordManager
{
    private static RoomRecordManager _instance;

    public static RoomRecordManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomRecordManager();
            }

            return _instance;
        }
    }

    private readonly List<RoomRecordIcon> _list = new List<RoomRecordIcon>();

    public List<RoomRecordIcon> List
    {
        get
        {
            if (_list.Count == 0)
                InitTalkData();
            return _list;
        }
    }

    public void InitTalkData()
    {
        for (int i = 0; i < 4; i++)
        {
            RoomRecordIcon recordItem = new RoomRecordIcon(null, "sad", 8, "+9");
            _list.Add(recordItem);
        }
    }
}