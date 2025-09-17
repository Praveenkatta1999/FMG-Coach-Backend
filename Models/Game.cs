using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("game")]
public class Game
{
    [Key]
    [Column("match_id")]
    public required string matchId { get; init; }

    [Column("league")]
    public string? league { get; set; }

    [Column("home_team")]
    public required string homeTeam { get; init; }

    [Column("away_team")]
    public required string awayTeam { get; init; }

    [Column("result")]
    public required string result { get; init; }

    [Column("home_team_offence_coach")]
    public string? homeTeamOffenceCoach { get; set; }

    [Column("home_team_defence_coach")]
    public string? homeTeamDefenceCoach { get; set; }

    [Column("home_team_special_teams_coach")]
    public string? homeTeamSpecialTeamsCoach { get; set; }

    [Column("away_team_offence_coach")]
    public string? awayTeamOffenceCoach { get; set; }

    [Column("away_team_defence_coach")]
    public string? awayTeamDefenceCoach { get; set; }

    [Column("away_team_special_teams_coach")]
    public string? awayTeamSpecialTeamCoach { get; set; }

    [Column("coach_bonus_applied_percent")]
    public float coachBonusAppliedPercent { get; set; }

    public Game(
    //     string matchId,
    //     string homeTeam,
    //     string awayTeam,
        string league,
        // string result,
        float coachBonusAppliedPercent
    )
    {
        // this.matchId = matchId;
        // this.homeTeam = homeTeam;
        // this.awayTeam = awayTeam;
        this.league = league;
        // this.result = result;
        this.coachBonusAppliedPercent = coachBonusAppliedPercent;
    }
}
