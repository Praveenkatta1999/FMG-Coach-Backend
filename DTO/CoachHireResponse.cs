public class CoachHireResponse
{
    public string message { get; set; }
    public Team team { get; set; }

    public CoachHireResponse(string message, Team team)
    {
        this.message = message;
        this.team = team;

    }
}