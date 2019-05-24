using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class PreservationModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PreservationModel() { }
        public PreservationModel(int id, string description, DateTime createdAt, DateTime updatedAt)
        {
            ID = id;
            Description = description;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
