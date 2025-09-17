using Microsoft.EntityFrameworkCore;

public class CoachService
{
    private readonly GameDbContext _db;
    private readonly TeamService _teamService;

    public CoachService(GameDbContext db, TeamService teamService)
    {
        _db = db;
        _teamService = teamService;
    }


    public async Task<IEnumerable<Coach>> GetAllCoaches()
    {
        var coaches = await _db.Coaches.ToListAsync();
        return coaches;


    }

    public async Task<IEnumerable<Coach>> GetCoachByTeam(string teamId)
    {

        var coaches = await _db.Coaches.Where(c => c.currentTeam.Equals(teamId)).ToListAsync();

        Team team = await _teamService.GetTeamById(teamId);

        if (coaches == null || coaches.Count == 0) { throw new NotFoundException($"Coach for {team.teamName}"); }

        return coaches;

    }

    public async Task<Coach> GetCoachById(string coachId)
    {

        var coach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == coachId);
        if (coach == null) { throw new NotFoundException("Coach"); }

        Team teamPrev = await _teamService.GetTeamById(coach.prevTeam);
        coach.prevTeam = teamPrev.teamName;


        if (!string.IsNullOrEmpty(coach.currentTeam))
        {
            Team teamCurr = await _teamService.GetTeamById(coach.currentTeam);
            coach.currentTeam = teamCurr.teamName;
        }


        return coach;

    }

    public async Task<bool> IsCoachAffordableAsync(string coachId, string teamId)
    {
        Coach coach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == coachId);
        if (coach == null) { throw new NotFoundException("Coach"); }

        Team team = await _teamService.GetTeamById(teamId);
        if (team == null) { throw new NotFoundException("Team"); }

        return coach.totalCost <= team.budget;
    }

    public async Task<CoachHireResponse> HireCoach(string coachId, string teamId)
    {
        Team team = await _teamService.GetTeamById(teamId);
        Coach coach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == coachId);

        string coachType = coach.coachType;
        float coachRating = coach.overallRating;
        float team_budget = team.budget;

        string message = "";

        // Check if the coach is affordable
        if (!await IsCoachAffordableAsync(coachId, teamId))
        {
            throw new Exception("Coach is not affordable");
        }

        switch (coachType)
        {
            case "O":
                if (team.offenceCoach != null)
                    throw new Exception("Offence coach already hired");
                team.offenceCoach = coachId;
                message = "Offence coach hired";
                team.offenceRating = coachRating;
                break;

            case "D":
                if (team.defenceCoach != null)
                    throw new Exception("Defence coach already hired");
                team.defenceCoach = coachId;
                message = "Defence coach hired";
                team.defenceRating = coachRating;
                break;

            case "ST":
                if (team.speacialTeamsCoach != null)
                    throw new Exception("Special teams coach already hired");
                team.speacialTeamsCoach = coachId;
                message = "Special teams coach hired";
                team.specialTeamsRating = coachRating;
                break;

            default:
                throw new Exception("Invalid coach type");
        }

        team.budget = team_budget - coach.totalCost;

        float teamRating = GetTeamRating(team);
        team.overallRating = teamRating;

        coach.currentTeam = team.teamId;


        await _db.SaveChangesAsync();


        return new CoachHireResponse(message, team);

    }

    public float GetTeamRating(Team team)
    {
        List<float> activeRatings = new List<float>();

        activeRatings.Add(team.offenceRating);
        activeRatings.Add(team.defenceRating);
        activeRatings.Add(team.specialTeamsRating);

        float teamRating = 1;
        if (activeRatings.Count > 0)
        {
            float average = activeRatings.Average();
            teamRating = (float)Math.Round(1 + (average / 6) * 9);
        }

        return teamRating;
    }

    public async Task<CoachHireResponse> FireCoach(string coachId, string teamId)
    {
        Team team = await _teamService.GetTeamById(teamId);
        Coach coach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == coachId);

        string coachType = coach.coachType;
        float weekly_salary = coach.salary;

        float team_budget = team.budget;

        string message = "";

        switch (coachType)
        {
            case "O":
                team.offenceCoach = null;
                team.offenceRating = 1;
                message = "Offence coach fired";
                break;

            case "D":
                team.defenceCoach = null;
                team.defenceRating = 1;
                message = "Defence coach fired";
                break;

            case "ST":

                team.speacialTeamsCoach = null;
                team.specialTeamsRating = 1;
                message = "Special teams  fired";
                break;

            default:
                throw new Exception("Invalid coach type");
        }

        team.budget = team_budget + weekly_salary;

        float teamRating = GetTeamRating(team);
        team.overallRating = teamRating;

        coach.prevTeam = team.teamId;
        coach.currentTeam = null;
        coach.current_team_games_played = 0;
        coach.current_team_games_won = 0;
        coach.current_team_games_lost = 0;
        coach.current_team_games_tie = 0;

        // TODO: call updateCoach method and include above coach stat updation in it as well

        await _db.SaveChangesAsync();

        return new CoachHireResponse(message, team);

    }


    // TODO: write UpdateCoach method to update his wins-loss, ratings and salary


    public async void UpdateCoachStats(string homeTeamOffenceCoachId, string homeTeamDefenceCoachId, string homeTeamSpecialTeamsCoachId, string awayTeamOffenceCoachId, string awayTeamDefenceCoachId, string awayTeamSpecialTeamsCoachId, string result)
    {
        if (result == "H")
        {
            // Win
            if (homeTeamOffenceCoachId != null)
            {
                var homeTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamOffenceCoachId);
                homeTeamOffenceCoach.current_team_games_played++;
                homeTeamOffenceCoach.current_team_games_won++;
            }

            if (homeTeamDefenceCoachId != null)
            {
                var homeTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamDefenceCoachId);
                homeTeamDefenceCoach.current_team_games_played++;
                homeTeamDefenceCoach.current_team_games_won++;
            }

            if (homeTeamSpecialTeamsCoachId != null)
            {
                var homeTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamSpecialTeamsCoachId);
                homeTeamSpecialTeamsCoach.current_team_games_played++;
                homeTeamSpecialTeamsCoach.current_team_games_won++;
            }

            if (awayTeamOffenceCoachId != null)
            {
                var awayTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamOffenceCoachId);
                awayTeamOffenceCoach.current_team_games_played++;
                awayTeamOffenceCoach.current_team_games_lost++;
            }

            if (awayTeamDefenceCoachId != null)
            {
                var awayTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamDefenceCoachId);
                awayTeamDefenceCoach.current_team_games_played++;
                awayTeamDefenceCoach.current_team_games_lost++;
            }

            if (awayTeamSpecialTeamsCoachId != null)
            {
                var awayTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamSpecialTeamsCoachId);
                awayTeamSpecialTeamsCoach.current_team_games_played++;
                awayTeamSpecialTeamsCoach.current_team_games_lost++;
            }

            await _db.SaveChangesAsync();
        }
        else if (result == "A")
        {
            // Loss
            if (homeTeamOffenceCoachId != null)
            {
                var homeTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamOffenceCoachId);
                homeTeamOffenceCoach.current_team_games_played++;
                homeTeamOffenceCoach.current_team_games_lost++;
            }

            if (homeTeamDefenceCoachId != null)
            {
                var homeTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamDefenceCoachId);
                homeTeamDefenceCoach.current_team_games_played++;
                homeTeamDefenceCoach.current_team_games_lost++;
            }

            if (homeTeamSpecialTeamsCoachId != null)
            {
                var homeTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamSpecialTeamsCoachId);
                homeTeamSpecialTeamsCoach.current_team_games_played++;
                homeTeamSpecialTeamsCoach.current_team_games_lost++;
            }

            if (awayTeamOffenceCoachId != null)
            {
                var awayTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamOffenceCoachId);
                awayTeamOffenceCoach.current_team_games_played++;
                awayTeamOffenceCoach.current_team_games_won++;
            }

            if (awayTeamDefenceCoachId != null)
            {
                var awayTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamDefenceCoachId);
                awayTeamDefenceCoach.current_team_games_played++;
                awayTeamDefenceCoach.current_team_games_won++;
            }

            if (awayTeamSpecialTeamsCoachId != null)
            {
                var awayTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamSpecialTeamsCoachId);
                awayTeamSpecialTeamsCoach.current_team_games_played++;
                awayTeamSpecialTeamsCoach.current_team_games_won++;
            }

            await _db.SaveChangesAsync();

        }
        else
        {
            // Tie
            if (homeTeamOffenceCoachId != null)
            {
                var homeTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamOffenceCoachId);
                homeTeamOffenceCoach.current_team_games_played++;
                homeTeamOffenceCoach.current_team_games_tie++;
            }

            if (homeTeamDefenceCoachId != null)
            {
                var homeTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamDefenceCoachId);
                homeTeamDefenceCoach.current_team_games_played++;
                homeTeamDefenceCoach.current_team_games_tie++;
            }

            if (homeTeamSpecialTeamsCoachId != null)
            {
                var homeTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == homeTeamSpecialTeamsCoachId);
                homeTeamSpecialTeamsCoach.current_team_games_played++;
                homeTeamSpecialTeamsCoach.current_team_games_tie++;
            }

            if (awayTeamOffenceCoachId != null)
            {
                var awayTeamOffenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamOffenceCoachId);
                awayTeamOffenceCoach.current_team_games_played++;
                awayTeamOffenceCoach.current_team_games_tie++;
            }

            if (awayTeamDefenceCoachId != null)
            {
                var awayTeamDefenceCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamDefenceCoachId);
                awayTeamDefenceCoach.current_team_games_played++;
                awayTeamDefenceCoach.current_team_games_tie++;
            }

            if (awayTeamSpecialTeamsCoachId != null)
            {
                var awayTeamSpecialTeamsCoach = await _db.Coaches.FirstOrDefaultAsync(c => c.coachId == awayTeamSpecialTeamsCoachId);
                awayTeamSpecialTeamsCoach.current_team_games_played++;
                awayTeamSpecialTeamsCoach.current_team_games_tie++;
            }

            await _db.SaveChangesAsync();

        }



    }

}