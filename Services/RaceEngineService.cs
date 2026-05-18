using Simulador_Mario_Kart.Models;
using Simulador_Mario_Kart.DTOs;

namespace Simulador_Mario_Kart.Services
{
    /// <summary>
    /// Simulates the race logic: dice rolls, special powers, lap results.
    /// Mirrors the original Node.js logic but structured for .NET.
    /// </summary>
    public class RaceEngineService
    {
        private readonly Random _rng = new();

        // Characters with their base stats
        private static readonly Dictionary<string, CharacterStats> Characters = new()
        {
            ["Mario"] = new("Mario", "🍄", Speed: 4, Maneuverability: 3, Power: 3),
            ["Luigi"] = new("Luigi", "💚", Speed: 3, Maneuverability: 4, Power: 3),
            ["Peach"] = new("Peach", "🌸", Speed: 3, Maneuverability: 3, Power: 4),
            ["Yoshi"] = new("Yoshi", "🦕", Speed: 4, Maneuverability: 4, Power: 2),
            ["Bowser"] = new("Bowser", "🐢", Speed: 2, Maneuverability: 2, Power: 5),
            ["DonkeyKong"] = new("DonkeyKong", "🦍", Speed: 2, Maneuverability: 3, Power: 5),
        };

        public static IEnumerable<object> GetAvailableCharacters() =>
            Characters.Values.Select(c => new { c.Name, c.Emoji, c.Speed, c.Maneuverability, c.Power });

        /// <summary>
        /// Runs a complete race simulation and returns all lap results + final standings.
        /// </summary>
        public RaceSimulationResult SimulateRace(StartRaceRequest request)
        {
            var playerStates = request.Players.Select(p => new PlayerState
            {
                UserId = p.UserId,
                CharacterName = p.CharacterName,
                CharacterEmoji = p.CharacterEmoji,
                Stats = Characters.TryGetValue(p.CharacterName, out var stats) ? stats
                        : new CharacterStats(p.CharacterName, p.CharacterEmoji, 3, 3, 3)
            }).ToList();

            var laps = new List<List<LapResultDto>>();

            for (int lap = 1; lap <= request.TotalLaps; lap++)
            {
                var lapResults = SimulateLap(lap, playerStates);
                laps.Add(lapResults);
            }

            // Rank players by total points (desc), then by total time (asc)
            var ordered = playerStates
                .OrderByDescending(p => p.TotalPoints)
                .ThenBy(p => p.TotalTime)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
                ordered[i].FinalPosition = i + 1;

            // Wins/Losses per player (1st = win, others = loss)
            ordered[0].Wins++;
            for (int i = 1; i < ordered.Count; i++)
                ordered[i].Losses++;

            var finalStandings = ordered.Select(p => new FinalStandingDto(
                p.FinalPosition,
                p.CharacterName,
                p.CharacterEmoji,
                p.CharacterName,  // username filled by controller from DB
                p.UserId,
                p.TotalPoints,
                p.Wins,
                p.Losses,
                Math.Round(p.TotalTime, 2)
            )).ToList();

            return new RaceSimulationResult(laps, finalStandings, playerStates);
        }

        private List<LapResultDto> SimulateLap(int lapNumber, List<PlayerState> players)
        {
            var blocks = new[] { "STRAIGHT", "CURVE", "OBSTACLE" };

            foreach (var player in players)
            {
                int blockScore = 0;
                string blockType = blocks[_rng.Next(blocks.Length)];

                int die = RollDie();
                int stat = blockType switch
                {
                    "STRAIGHT" => player.Stats.Speed,
                    "CURVE" => player.Stats.Maneuverability,
                    "OBSTACLE" => player.Stats.Power,
                    _ => 3
                };

                blockScore = die + stat;

                // Special power roll
                if (die == 6)
                {
                    int bonusItem = RollDie();
                    blockScore += bonusItem;
                }

                int lapTime = _rng.Next(28, 45); // seconds per lap

                player.LapPoints.Add(blockScore);
                player.TotalPoints += blockScore;
                player.TotalTime += lapTime;
            }

            // Update positions this lap
            var ranked = players.OrderByDescending(p => p.TotalPoints).ToList();
            for (int i = 0; i < ranked.Count; i++)
                ranked[i].CurrentPosition = i + 1;

            return ranked.Select(p => new LapResultDto(
                p.CharacterName,
                p.CharacterEmoji,
                p.UserId,
                lapNumber,
                p.LapPoints.Last(),
                p.TotalPoints,
                p.CurrentPosition,
                p.TotalTime
            )).ToList();
        }

        private int RollDie() => _rng.Next(1, 7);
    }

    public record CharacterStats(
        string Name,
        string Emoji,
        int Speed,
        int Maneuverability,
        int Power
    );

    public class PlayerState
    {
        public int UserId { get; set; }
        public string CharacterName { get; set; } = "";
        public string CharacterEmoji { get; set; } = "";
        public CharacterStats Stats { get; set; } = null!;
        public int TotalPoints { get; set; }
        public int CurrentPosition { get; set; }
        public int FinalPosition { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double TotalTime { get; set; }
        public List<int> LapPoints { get; set; } = new();
    }

    public record RaceSimulationResult(
        List<List<LapResultDto>> LapHistory,
        List<FinalStandingDto> FinalStandings,
        List<PlayerState> PlayerStates
    );
}

