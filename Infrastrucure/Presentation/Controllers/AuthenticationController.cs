using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ServiceAbstraction;
using Shared.DTO.IdentityModuleDTos;

namespace Presentation.Controllers
{
    
    public class AuthenticationController(IServiceManeger _serviceManeger) : ApiBaseController
    {
        //Login
        [HttpPost("Login")] // POST BaseUrl/api/Authentication/Login
        public async Task<ActionResult<UserDTo>> Login(LoginDTo loginDTo)
        {
            var User = await _serviceManeger.AuthenticationService.LoginAsync(loginDTo);
            return Ok(User);
        }

        //Register
        [HttpPost("Register")] // POST BaseUrl/api/Authentication/Register
        public async Task<ActionResult<UserDTo>> Register(RegisterDTo registerDTo)
        {
            var User = await _serviceManeger.AuthenticationService.ResgisterAsync(registerDTo);
            return Ok(User);
        }

        //Check Email
        [HttpGet("CheckEmail")]  // Get BaseUrl/api/Authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result = await _serviceManeger.AuthenticationService.CheckEmailAsync(Email);
            return Ok(Result);
        }

        //Get Current User
        [Authorize]
        [HttpGet("CurrentUSer")]
        public async Task<ActionResult<UserDTo>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var AppUser = await _serviceManeger.AuthenticationService.GetCurrentUserAsync(email!);
            return Ok(AppUser);
        }

        //Get Current User Address
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTo>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Address = await _serviceManeger.AuthenticationService.GetCurrentUserAddressAsync(email!);
            return Ok(Address);
        }

        //Update Current User Address
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTo>> UpdateCurrentUserAddress(AddressDTo addressDTo)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var UpdateAddress = await _serviceManeger.AuthenticationService.UpdateCurrentUserAddressAsync(email, addressDTo);
            return Ok(UpdateAddress);
        }
    }
}
