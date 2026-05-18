namespace Simulador_Mario_Kart.DTOs
{
    public record LeaderboardEntryDto(
       int Rank,
       int UserId,
       string Username,
       string FavoriteCharacter,
       string FavoriteEmoji,
       int TotalWins,
       int TotalRaces,
       double WinRate,
       int BestPosition,
       int TotalPoints
   );
}
