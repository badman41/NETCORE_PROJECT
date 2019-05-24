using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class UnitModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public UnitModel() { }
        public UnitModel(int id, string name, int? createdBy, DateTime? createdAt, int? modifyBy, DateTime? modifyAt)
        {
            ID = id;
            Name = name;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            ModifyBy = modifyBy;
            ModifyAt = modifyAt;
        }
    }
}
