namespace WebApi.Dtos
{
    public record GenerateJwtTokenDto(
        string FirstName, 
        string LastName,
        string Email,
        string UserName,
        List<string>? Roles = null
        );
    
}
