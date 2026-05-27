using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulador_Mario_Kart.Data;
using Simulador_Mario_Kart.DTOs;

namespace Simulador_Mario_Kart.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LeaderboardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public LeaderboardController(AppDbContext db) => _db = db;

        /// <summary>Ranking global de todos os jogadores por vitórias.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<LeaderboardEntryDto>), 200)]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int top = 10)
        {
            var raceResults = await _db.RaceResults
                .Include(r => r.User)
                .ToListAsync();

            var results = raceResults
                .GroupBy(r => new { r.UserId, r.User.Username })
                .Select(g =>
                {
                    var favorite = g
                        .OrderByDescending(x => x.TotalPoints)
                        .First();

                    return new LeaderboardEntryDto(
                        0,
                        g.Key.UserId,
                        g.Key.Username,
                        favorite.CharacterName,
                        favorite.CharacterEmoji,
                        g.Sum(x => x.Wins),
                        g.Count(),
                        g.Count() == 0
                            ? 0
                            : Math.Round((double)g.Sum(x => x.Wins) / g.Count() * 100, 1),
                        g.Min(x => x.FinalPosition),
                        g.Sum(x => x.TotalPoints)
                    );
                })
                .OrderByDescending(x => x.TotalWins)
                .ThenByDescending(x => x.TotalPoints)
                .Take(top)
                .ToList();

            var ranked = results
                .Select((entry, i) => entry with { Rank = i + 1 })
                .ToList();

            return Ok(ranked);
        }
        

        /// <summary>Histórico de corridas de um usuário específico.</summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserHistory(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var history = await _db.RaceResults
                .Where(r => r.UserId == userId)
                .Include(r => r.Race)
                .OrderByDescending(r => r.CompletedAt)
                .Select(r => new
                {
                    r.Race.Id,
                    r.Race.TrackName,
                    r.CharacterName,
                    r.CharacterEmoji,
                    r.FinalPosition,
                    r.TotalPoints,
                    r.TotalTime,
                    r.CompletedAt
                })
                .ToListAsync();

            var stats = new
            {
                user.Username,
                TotalRaces = history.Count,
                TotalWins = history.Count(h => h.FinalPosition == 1),
                BestTime = history.Any() ? history.Min(h => h.TotalTime) : 0,
                FavoriteCharacter = history
                    .GroupBy(h => h.CharacterName)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault()
            };

            return Ok(new { Stats = stats, History = history });
        }
    }
}

