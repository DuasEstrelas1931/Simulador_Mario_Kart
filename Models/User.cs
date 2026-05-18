using System.ComponentModel.DataAnnotations;

namespace Simulador_Mario_Kart.Models
{
    public class User
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        public string Username { set; get; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo 'Email' deve ser um endereço de email válido.")]
        public string Email { set; get; } = string.Empty;

        [Required(ErrorMessage = "O campo 'PasswordHash' é obrigatório.")]
        public string PasswordHash { set; get; } = string.Empty;

        public string? AvatarUrl { set; get; }
        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;

        // Navigation

        public ICollection<RaceResult> RaceResults { set; get; } = new List<RaceResult>();
    }
}