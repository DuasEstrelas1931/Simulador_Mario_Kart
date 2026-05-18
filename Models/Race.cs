using System.ComponentModel.DataAnnotations;

namespace Simulador_Mario_Kart.Models
{
    public class Race
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "O nome da corrida é obrigatório.")]
        public string TrackName { set; get; } = string.Empty;
        public int TotalLaps { set; get; }
        public DateTime StartedAt { set; get; } = DateTime.UtcNow;
        public DateTime? FinishedAt { set; get; }
        public RaceStatus Status { set; get; } = RaceStatus.Pending;

        //NAvigation
        public ICollection<RaceResult> Results { set; get; } = new List<RaceResult>();




    }
}
