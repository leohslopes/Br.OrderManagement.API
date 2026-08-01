using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Enums
{
    public enum OrderStatus
    {
        Created = 1,
        Confirmed = 2,
        Canceled = 3,
        Finished = 4
    }
}
