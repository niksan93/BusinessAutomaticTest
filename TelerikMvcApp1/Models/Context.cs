using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TelerikMvcApp1.Models
{
    public class Context
    {
        public const string CONNECTION_STRING_NAME = "TestApp";
        public const string DATABASE_NAME = "testApp";
        public const string USERS_COLLECTION_NAME = "users";
        public const string DEPARTMENTS_COLLECTION_NAME = "departments";

        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;
        static Context()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient();
            _database = _client.GetDatabase(DATABASE_NAME);
            if (_database.GetCollection<User>(USERS_COLLECTION_NAME) == null)
            {
                _database.CreateCollection(USERS_COLLECTION_NAME);
               // _database.GetCollection<User>(USERS_COLLECTION_NAME).InsertOne();
            }

            if (_database.GetCollection<Department>(DEPARTMENTS_COLLECTION_NAME) == null)
            {
                _database.CreateCollection(DEPARTMENTS_COLLECTION_NAME);
            }
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<User> Users
        {
            get { return _database.GetCollection<User>(USERS_COLLECTION_NAME); }
        }

        public IMongoCollection<Department> Departments
        {
            get { return _database.GetCollection<Department>(DEPARTMENTS_COLLECTION_NAME); }
        }
    }
}