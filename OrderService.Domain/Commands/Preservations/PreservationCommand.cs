using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Preservations
{
    public abstract class PreservationCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public string Description { get; protected set; }
        public int CreatedBy { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    
    }
}
