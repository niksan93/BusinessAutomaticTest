using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelerikMvcApp1.Models
{
    public class Department
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        public int DepartmentId { get; set; }
        public string Title { get; set; }
    }
}