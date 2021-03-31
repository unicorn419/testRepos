using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SlotAPI.Configuration;
using SlotAPI.Models;


namespace SlotAPI.Service
{
    public class UserService
    {
        private MongoClient mongoClient;
        private IMongoDatabase database;
        private SlotMongoDbSettings settings;
        public UserService(SlotMongoDbSettings settings)
        {
            this.settings = settings;
            mongoClient = new MongoClient(settings.ConnectionString);
            database = mongoClient.GetDatabase(settings.DatabaseName);

        }

        public User GetUser(string uid)
        {
            User user = database.GetCollection<User>("Users").Find(c => c.Id == uid).FirstOrDefault();
            if (user == null)
            {
                throw new Exception(string.Format("cannot found user{0}", uid));
            }
            else
            {
                return user;
            }
        }

        public void UpdateUser(string id, User user)
        {
             database.GetCollection<User>("Users").ReplaceOne(c => c.Id == id, user);

        }
    }
}
