using Microsoft.EntityFrameworkCore;
using System.Transactions;

public class GameService
{
    private readonly GameDbContext _db;

    private readonly TeamService _teamService;

    private readonly CoachService _coachService;

    public GameService(GameDbContext db, TeamService teamService, CoachService coachService)
    {
        _db = db;
        _teamService = teamService;
        _coachService = coachService;
    }

    // get all games by team id
    public async Task<IEnumerable<Game>> GetGamesByTeam(string teamId)
    {
        var games = await _db
            .Games.Where(g => g.homeTeam.Equals(teamId) || g.awayTeam.Equals(teamId))
            .ToListAsync();
        return games;
    }

    // create a game
    public async Task<Game> CreateGame(GameAddRequest request)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var game = new Game(request.league, request.coachBonusAppliedPercent)
            {
                matchId = request.matchId,
                homeTeam = request.homeTeam,
                awayTeam = request.awayTeam,
                result = request.result,
            };

            var homeTeam = await _teamService.GetTeamById(request.homeTeam);

            game.homeTeamOffenceCoach = homeTeam.offenceCoach;
            game.homeTeamDefenceCoach = homeTeam.defenceCoach;
            game.homeTeamSpecialTeamsCoach = homeTeam.speacialTeamsCoach;

            var awayTeam = await _teamService.GetTeamById(request.awayTeam);

            game.awayTeamOffenceCoach = awayTeam.offenceCoach;
            game.awayTeamDefenceCoach = awayTeam.defenceCoach;
            game.awayTeamSpecialTeamCoach = awayTeam.speacialTeamsCoach;

            // write updateCoachStat method in coach_service

            await _coachService.UpdateCoachStats(
             game.homeTeamOffenceCoach,
             game.homeTeamDefenceCoach,
             game.homeTeamSpecialTeamsCoach,
             game.awayTeamOffenceCoach,
             game.awayTeamDefenceCoach,
             game.awayTeamSpecialTeamCoach,
             game.result
         );


            // update Teams record for games won and lost along with the coach stats

            _db.Add(game);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();
            return game;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
