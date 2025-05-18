using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO.IdentityModuleDTos;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        //login
        //This EndPoint Will Handle User Login Take Email and Password Then Return Token ,  Email and DisplayName
        Task<UserDTo> LoginAsync(LoginDTo loginDTo);

        //Register
        //This EndPoint Will Handle User Registration Will Take Email , Password  , UserName , Display Name And Phone Number Then
        //Return Token , Email and Display Name
        Task<UserDTo> ResgisterAsync(RegisterDTo resgisterDTo);

        //Check Emai
        //This EndPoint Will Handle Checking if User Email Is Exists Or Not Will Take string Email
        //Then Return boolean 
        Task<bool> CheckEmailAsync(string Email);

        //Get Current User Address 
        //This EndPoint Will Take string Email
        //Then Return Address of Current Logged in User 
        Task<AddressDTo> GetCurrentUserAddressAsync(string Email);

        //Update Current User Address 
        //This EndPoint Will Handle Updating User Address Take Updated Address and Email
        //Then Return Address after Update 
        Task<AddressDTo> UpdateCurrentUserAddressAsync(string Email , AddressDTo addressDTo);

        //Get Current User 
        //This EndPoint Will Take Email
        //Then Return Token , Email and Display Name 
        Task<UserDTo> GetCurrentUserAsync(string Email);

    }
}
