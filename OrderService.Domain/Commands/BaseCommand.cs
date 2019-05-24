using System;
using FluentValidation.Results;
using MediatR;

namespace OrderService.Domain.Commands
{
    public abstract class BaseCommand : IRequest<object>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }
        protected BaseCommand()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool IsValid(object repository = null);
    }
}
