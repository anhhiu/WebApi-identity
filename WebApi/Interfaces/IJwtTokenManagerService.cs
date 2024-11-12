using WebApi.Dtos;

namespace WebApi.Interfaces
{
    public interface IJwtTokenManagerService
    {
        Task<string> GenerateJwtToken(GenerateJwtTokenDto payload);
    }
}
