namespace WebApi.Dtos
{
    public record CreateAccountDto(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber,
        string Address,
        string UserName
        );
   
}
