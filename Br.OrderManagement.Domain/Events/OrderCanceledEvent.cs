using Br.OrderManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br.OrderManagement.Domain.Events
{
    public sealed class OrderCanceledEvent(Guid orderId) : DomainEvent
    {
        public Guid OrderId { get; } = orderId;
    }
}
