using Microsoft.AspNetCore.Identity;
using System.Net;
using WebApi.Common;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtTokenManagerService jwtTokenManagerService;

        public AccountService( UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenManagerService jwtTokenManagerService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenManagerService = jwtTokenManagerService;
        }

        public async Task<ServiceResponse<dynamic>> CreateAccountAsync(CreateAccountDto payload)
        {
            var serviceResponse = new ServiceResponse<dynamic>();

            try
            {

                var isUserExits = await userManager.FindByEmailAsync(payload.Email);

                if(isUserExits is not null)
                {
                    serviceResponse.Data = new { };
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    return serviceResponse;

                }

                var newUser = new User
                {
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    PhoneNumber = payload.PhoneNumber,
                    Email = payload.Email,
                    Address = payload.Address,
                    UserName = payload.UserName,
                    UpdatedAt = DateTime.UtcNow
                   
                };


                var response = await userManager.CreateAsync(newUser, payload.Password);

                if (!response.Succeeded)
                {
                    var errorMsg = string.Join(", ", response.Errors.Select(e => e.Description));
                    serviceResponse.Data = null;
                    serviceResponse.Message = errorMsg;
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    return serviceResponse;
                }

                serviceResponse.Data = payload;
                serviceResponse.Success = true;
                serviceResponse.Message = "Registration Successful";
                serviceResponse.StatusCode = (int)HttpStatusCode.Created;
            }
            catch(Exception ex)
            {
                serviceResponse.Data = payload;
                serviceResponse.Message = "Internal ServerError: " + ex.Message;
                serviceResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<dynamic>> Login(LoginDto payload)
        {
            var serviceResponse = new ServiceResponse<dynamic>();

            try
            {
                var isUserExits = await userManager.FindByNameAsync(payload.UserName);

                if(isUserExits is null)
                {

                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    serviceResponse.Message = "Invalid Email or Password - Mật khẩu hoặc password không hợp lệ";

                    return serviceResponse;
                }

                var signinResponse = await signInManager.PasswordSignInAsync(isUserExits, payload.Password, true, true);

                if (signinResponse.IsLockedOut)
                {
                    await userManager.SetLockoutEndDateAsync(isUserExits, DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(5)));

                    serviceResponse.Success = true;
                    serviceResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    serviceResponse.Message = "You have been lockout - Bạn đã bị khóa ngoài, hãy thử lại sau 5 phút";

                    return serviceResponse;
                }

                if(signinResponse.IsNotAllowed)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Message = "You are not allowed to access this portal - Bạn không được phép truy cập vào cổng thông tin này";
                    serviceResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                    return serviceResponse;
                }

                if (!signinResponse.Succeeded)
                {
                    serviceResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    serviceResponse.Message = "Invalid Email or Password - Mật khẩu hoặc password không hợp lệ";

                    return serviceResponse;
                }
                var token = await jwtTokenManagerService.GenerateJwtToken(new GenerateJwtTokenDto(isUserExits.FirstName, isUserExits.LastName, isUserExits.Email, isUserExits.UserName));
                serviceResponse.StatusCode = (int)HttpStatusCode.OK;
                serviceResponse.Success = true;
                serviceResponse.Data = new { Token = token };
                serviceResponse.Message = "Login sucessful - Đăng nhập thành công.";

            } catch (Exception ex)
            {
                serviceResponse.Data = new { };
                serviceResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                serviceResponse.Message = "Internal erver error - Lỗi máy chủ nội bộ: " + ex.Message;
            }


            return serviceResponse;
        }
    }
}
