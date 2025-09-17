public class GameAddRequest
{
    public string matchId { get; set; }
    public string homeTeam { get; set; }
    public string awayTeam { get; set; }
    public string league { get; set; }
    public string result { get; set; }
    public float coachBonusAppliedPercent { get; set; }
}
