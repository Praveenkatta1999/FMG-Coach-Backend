using Microsoft.EntityFrameworkCore;

public class TeamService
{
    private readonly GameDbContext _db;

    public TeamService(GameDbContext db)
    {
        _db = db;
    }

    public async Task<Team> GetTeamById(string teamId)
    {
        var team = await _db.Teams.FirstOrDefaultAsync(t => t.teamId == teamId);

        if (team == null)
        {
            throw new NotFoundException($"Team with ID {teamId}");
        }

        return team;
    }

    public async Task<Team> CreateTeam(TeamAddRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "request cannot be null");
        }

        string sql = "INSERT INTO Team (team_id, team_name) VALUES (@p0, @p1)";
        await _db.Database.ExecuteSqlRawAsync(sql, request.teamId, request.teamName);

        await _db.SaveChangesAsync();

        var team = await GetTeamById(request.teamId);

        return team;
    }
}
