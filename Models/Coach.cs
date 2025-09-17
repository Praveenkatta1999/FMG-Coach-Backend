using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.ComponentModel.DataAnnotations;

[Table("coach")]
public class Coach
{
    [Key]
    [Column("coach_id")]
    public required string coachId { get; set; }

    [Column("coach_name")]
    public required string coachName { get; set; }

    [Column("coach_type")]
    public required string coachType { get; set; }

    [Column("experience")]
    public int experience { get; set; }

    [Column("from")]
    public int from { get; set; }

    [Column("to")]
    public int to { get; set; }

    [Column("games_played")]
    public int gamesPlayed { get; set; }

    [Column("games_won")]
    public int gamesWon { get; set; }

    [Column("games_lost")]
    public int gamesLost { get; set; }

    [Column("games_tie")]
    public int gamesTie { get; set; }

    [Column("win_loss_percentage")]
    public float winLossPercentage { get; set; }

    [Column("championship_won")]
    public int championshipWon { get; set; }

    [Column("salary")]
    public float salary { get; set; }

    [Column("contract_length")]
    public int contractLength { get; set; }

    [Column("bonus")]
    public float bonus { get; set; }

    [Column("total_cost")]
    public float totalCost { get; set; }

    [Column("kickoff_instance")]
    public float kickoffDistance { get; set; }

    [Column("return_coverage")]
    public float returnCoverage { get; set; }

    [Column("field_goal_accuracy")]
    public float fieldGoalAccuracy { get; set; }

    [Column("return_speed")]
    public float returnSpeed { get; set; }

    [Column("passing_efficiency")]
    public float passingEfficiency { get; set; }

    [Column("rush")]
    public float rush { get; set; }

    [Column("red_zone_conversion")]
    public float redZoneConversion { get; set; }

    [Column("play_variation")]
    public float playVariation { get; set; }

    [Column("coverage_discipline")]
    public float coverageDiscipline { get; set; }

    [Column("run_defence")]
    public float runDefence { get; set; }

    [Column("turnover")]
    public float turnover { get; set; }

    [Column("pressure_control")]
    public float pressureControl { get; set; }

    [Column("overall_rating")]
    public float overallRating { get; set; }

    [Column("prev_team")]
    public required string prevTeam { get; set; }

    [Column("current_team")]
    public string? currentTeam { get; set; }

    [Column("current_team_games_played")]
    public int current_team_games_played { get; set; }

    [Column("current_team_games_won")]
    public int current_team_games_won { get; set; }

    [Column("current_team_games_lost")]
    public int current_team_games_lost { get; set; }

    [Column("current_team_games_tie")]
    public int current_team_games_tie { get; set; }
}
