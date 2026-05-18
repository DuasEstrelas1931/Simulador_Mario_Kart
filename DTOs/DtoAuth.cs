namespace Simulador_Mario_Kart.DTOs
{
    public record RegisterRequest(
        string Username,
        string Email,
        string Password
     );

    public record LoginRequest(
        string Email,
        string Password
     );

     public record AuthResponse(
        string Token,
        string Username,
        string Email,
        int UserId
        //DateTime Expiration
     );
}
