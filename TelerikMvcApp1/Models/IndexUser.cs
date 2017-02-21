using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace TelerikMvcApp1.Models
{
    public class IndexUser
    {
        public ObjectId _id { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
    }
}