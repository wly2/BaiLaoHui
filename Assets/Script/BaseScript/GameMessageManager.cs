using AssemblyCSharp;

public class GameMessageManager
{                                      
    public delegate void GameSetIntEvent(int _num);

    public static GameSetIntEvent SetDeskDir;
    public static GameSetIntEvent SetDirCount;
    public static GameSetIntEvent SetListIndex;
    public static GameSetIntEvent SetMyGangPoint;
    public static GameSetIntEvent SetRoomNum;
    public static GameSetIntEvent ShowPointPrompt;
    public static GameSetIntEvent ClosePutOutPrompt;
    public static GameSetIntEvent SetBattery;
    public static GameSetIntEvent SetStartCardPoint;
    public static GameEvent ClosePointPrompt;

    public delegate void GameSetStringEvent(string _str);

    public static GameSetStringEvent setPlayersDir;

    public delegate void GameStartEvent(ClientResponse response);

    public static GameStartEvent SceneStartReply;
    public static GameStartEvent OtherPutOutReply;
    public static GameStartEvent PickCardReply;
    public static GameStartEvent OnGamePengReply;
    public static GameStartEvent OnGameChiReply;
    public static GameStartEvent OnGangReply;
    public static GameStartEvent OnOtherGangReply;
    public static GameStartEvent OnGameHuReply;

    public  delegate  void GameEvent();

    public static GameEvent OtherMoPai;
    
    public static GameEvent InitTable;
    public static GameEvent CloseLoading;
}