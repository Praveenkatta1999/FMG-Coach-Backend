using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("team")]
public class Team
{
    [Key]
    [Column("team_id")]
    public required string teamId { get; set; }

    [Column("team_name")]
    public required string teamName { get; set; }

    [Column("team_type")]
    public string? teamType { get; set; }

    [Column("league")]
    public string? league { get; set; }

    [Column("description")]
    public string? description { get; set; }

    [Column("offence_coach")]
    public string? offenceCoach { get; set; }

    [Column("defence_coach")]
    public string? defenceCoach { get; set; }

    [Column("speacial_teams_coach")]
    public string? speacialTeamsCoach { get; set; }

    [Column("qb")]
    public string? qb { get; set; }

    [Column("rb")]
    public string? rb { get; set; }

    [Column("ol1")]
    public string? ol1 { get; set; }

    [Column("ol2")]
    public string? ol2 { get; set; }

    [Column("ol3")]
    public string? ol3 { get; set; }

    [Column("r1")]
    public string? r1 { get; set; }

    [Column("r2")]
    public string? r2 { get; set; }

    [Column("s1")]
    public string? s1 { get; set; }

    [Column("s2")]
    public string? s2 { get; set; }

    [Column("dl1")]
    public string? dl1 { get; set; }

    [Column("dl2")]
    public string? dl2 { get; set; }

    [Column("dl3")]
    public string? dl3 { get; set; }

    [Column("db1")]
    public string? db1 { get; set; }

    [Column("db2")]
    public string? db2 { get; set; }

    [Column("budget")]
    public float budget { get; set; }

    [Column("offence_rating")]
    public float offenceRating { get; set; }

    [Column("defence_rating")]
    public float defenceRating { get; set; }

    [Column("special_teams_rating")]
    public float specialTeamsRating { get; set; }

    [Column("overall_rating")]
    public float overallRating { get; set; }

    [Column("games_played")]
    public int gamesPlayed { get; set; }

    [Column("games_won")]
    public int gamesWon { get; set; }

    [Column("games_lost")]
    public int gamesLost { get; set; }

    // public Team(string teamId, string teamName)
    // {
    //     this.teamId = teamId;
    //     this.teamName = teamName;
    // }

}