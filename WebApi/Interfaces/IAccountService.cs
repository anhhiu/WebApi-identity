using WebApi.Common;
using WebApi.Dtos;

namespace WebApi.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<dynamic>> CreateAccountAsync(CreateAccountDto payload);
        Task<ServiceResponse<dynamic>> Login(LoginDto payload);
    }
}
