using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class CustomerGroupModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Created_At { get; set; }
        public int Update_At { get; set; }
        public CustomerGroupModel() { }
        public CustomerGroupModel(int id, string name, DateTime createdAt, DateTime updateAt)
        {
            ID = id;
            Name = name;
            CreatedAt = createdAt;
            UpdateAt = updateAt;
        }
    }
}
