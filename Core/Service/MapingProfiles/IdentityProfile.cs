using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.IdentityModule;
using Shared.DTO.IdentityModuleDTos;

namespace Service.MapingProfiles
{
    public class IdentityProfile :Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address , AddressDTo>().ReverseMap();
        }
    }
}
