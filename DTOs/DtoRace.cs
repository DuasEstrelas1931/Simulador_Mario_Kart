namespace Simulador_Mario_Kart.DTOs
{
    public record StartRaceRequest(
        string TrackName,
        int TotalLaps,
        List<RacePlayerDto> Players
    );

    public  record RacePlayerDto(
        int UserId,
        string CharacterName,
        string CharacterEmoji
    );
     
}
