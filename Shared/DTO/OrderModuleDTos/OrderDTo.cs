using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTO.IdentityModuleDTos;

namespace Shared.DTO.OrderModuleDTos
{
    public class OrderDTo
    {
        public string BasketId { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public AddressDTo Address { get; set; } = default!;
    }
}
