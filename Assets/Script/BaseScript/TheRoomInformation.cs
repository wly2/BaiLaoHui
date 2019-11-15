public class TheRoomInformation
{
    public string homeowners;
    public string startTime;
    public string endTime;
    public string type;
    public string xX;
    public int innings;
    public string thePayer;

    public TheRoomInformation()
    {
    }

    public TheRoomInformation(string h, string st, string et, string t, string x, int i, string tp)
    {
        this.homeowners = h;
        this.startTime = st;
        this.endTime = et;
        this.type = t;
        this.xX = x;
        this.innings = i;
        this.thePayer = tp;
    }
}