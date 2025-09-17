using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMG_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly GameDbContext _db;
    private readonly TeamService _teamService;
    public TeamController(GameDbContext db, TeamService teamService)
    {
        _db = db;
        _teamService = teamService;

    }


    [HttpPut("")]
    public async Task<ActionResult<CoachHireResponse>> AddTeam([FromBody] TeamAddRequest request)
    {

        try
        {
            var team = await _teamService.CreateTeam(request);
            return Ok(new CoachHireResponse("Team created successfully", team));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }

    }
}