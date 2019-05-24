using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.CustomerGroup
{
    public abstract class CustomerGroupCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int CreatedBy { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    
    }
}
