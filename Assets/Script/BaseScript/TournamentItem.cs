public class TournamentItem
{
    public string image;
    public string competitionType;
    public string gameRewards;
    public string raceConditions;
    public int registrationNumber;
    public string registrationFeeImage;
    public int registrationFee;

    public TournamentItem()
    {
    }

    public TournamentItem(string i, string ct, string gr, string rc, int rn, string rfi, int rf)
    {
        image = i;
        competitionType = ct;
        gameRewards = gr;
        raceConditions = rc;
        registrationNumber = rn;
        registrationFeeImage = rfi;
        registrationFee = rf;
    }
}