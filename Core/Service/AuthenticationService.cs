using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTO.IdentityModuleDTos;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager , IConfiguration _configuration ,
        IMapper _mapper ) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email);
            return User is not null;
        }
        public async Task<UserDTo> GetCurrentUserAsync(string Email)
        {
            var User = await _userManager.FindByEmailAsync(Email) ?? throw new UserNotFoundException(Email);
            return new UserDTo()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await CreateTokenAsync(User),
            };
        }
        public async Task<AddressDTo> GetCurrentUserAddressAsync(string Email)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                          .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);

            if (User.Address is not null)
                return _mapper.Map<Address, AddressDTo>(User.Address);
            else
                throw new AddressNotFoundException(User.UserName);
        }
        public async Task<AddressDTo> UpdateCurrentUserAddressAsync(string Email, AddressDTo addressDTo)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                          .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);

            if(User.Address is not null) //Update Address
            {
                User.Address.FirstName = addressDTo.FirstName;
                User.Address.LastName = addressDTo.LastName;
                User.Address.City = addressDTo.City;
                User.Address.Country = addressDTo.Country;
                User.Address.Street = addressDTo.Street;
            }
            else //Add New Address
            {
                User.Address = _mapper.Map<AddressDTo, Address>(addressDTo);
            }

            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDTo>(User.Address);
        }
        public async Task<UserDTo> LoginAsync(LoginDTo loginDTo)
        {
            //check if email is exists
            var User = await _userManager.FindByEmailAsync(loginDTo.Email) ?? throw new UserNotFoundException(loginDTo.Email);
            //check password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User , loginDTo.Password);
            if (IsPasswordValid)
            {
                //return password
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
                throw new UnauthorizedException();
        }

        public async Task<UserDTo> ResgisterAsync(RegisterDTo resgisterDTo)
        {
            //mapping RegisterDTo => ApplicationUser
            var User = new ApplicationUser()
            {
                DisplayName = resgisterDTo.DisplayName,
                Email = resgisterDTo.Email,
                PhoneNumber = resgisterDTo.PhoneNumber,
                UserName = resgisterDTo.UserName,
            };

            //Create User [ApplicationUser]
            var Resualt = await _userManager.CreateAsync(User, resgisterDTo.Password);
            if(Resualt.Succeeded)
            {
                //Return UserDTo
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
            {
                //Throw BadRequest Exception
                var Errors = Resualt.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name , user.UserName!),
                new Claim(ClaimTypes.NameIdentifier , user.Id)
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach(var role  in Roles) 
                Claims.Add(new Claim(ClaimTypes.Role, role));

            //2.محتاح احدد مين security Key , Algorithm
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Creds = new SigningCredentials(Key , SecurityAlgorithms.HmacSha256);

            //Create Token
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"], //token اللي رايح ليه ال 
                audience: _configuration["JWTOptions:Audience"], //token اللي راجع ليه من ال 
                claims: Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
