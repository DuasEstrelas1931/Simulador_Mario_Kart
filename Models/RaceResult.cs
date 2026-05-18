using System.ComponentModel.DataAnnotations;

namespace Simulador_Mario_Kart.Models
{
    public class RaceResult
    {
        public int Id { set; get; }
        public int RaceId { set; get; }
        public Race Race  { set; get; } = null!;

        public int UserId { set; get; }
        public User User { set; get; } = null!;

        [Required]
        public string CharacterName { set; get; } = string.Empty;
        public string CharacterEmoji { set; get; } = "🏎️";
        public int FinalPosition { set; get; }
        public int TotalPoints { set; get; }
        public int Wins { set; get; }
        public int Losses { set; get; }
        public double TotalTime { set; get; } // Time in seconds
        public DateTime CompletedAt { set; get; } = DateTime.UtcNow;
    }
}
