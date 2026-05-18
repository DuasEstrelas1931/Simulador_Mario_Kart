namespace Simulador_Mario_Kart.DTOs
{
    public record LapResultDto(
        string CharacterName,
        string CharacterEmoji,
        int UserId,
        int LapNumber,
        int Points,          //  Points earned in this lap
        int TotalPoints,     //  Total points after this lap
        int Position,        //  Position in this lap
        double LapTime       //  Time taken for this lap in seconds
    );

    public record RaceStartDto(
        int RaceId,
        string TrackName,
        int CurrentLap,
        int TotalLaps,
        RaceStatus Status,
        List<LapResultDto> Standings
    );

    public enum RaceStatus
    {
        Pending,
        Running,
        Finished
    }

    public record RaceFinishDto(
        int RaceId,
        string TrackName,
        string WinnerName,
        string WinnerEmoji,
        int WinnerUserId,
        List<FinalStandingDto> FinalStandings
    );

    public record FinalStandingDto(
        int Position,
        string CharacterName,
        string CharacterEmoji,
        string Username,
        int UserId,
        int TotalPoints,
        int Wins,
        int Losses,
        double TotalTime
    );
}
