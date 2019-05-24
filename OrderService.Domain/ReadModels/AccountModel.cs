using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class AccountModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
        public int Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserName { get; set; }
        public int UserTypeID { get; set; }
        public AccountModel() { }
        
    }
}
