using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MyWeb.Entities;

namespace MyWeb.Repositories
{
    public class UserRepo : IUserRepo
    {
        private const string databaseName = "myweb";
        private const string collectionName = "user";
        private readonly IMongoCollection<User> userCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder =
            Builders<User>.Filter;

        public UserRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            userCollection = database.GetCollection<User>(collectionName);
        }

        public IEnumerable<User> GetAllUser()
        {
            return userCollection.Find(new BsonDocument()).ToList();
        }

        public User GetUserById(Guid id)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
            return userCollection.Find(filter).SingleOrDefault();
        }

        public void CreateNewUser(User user)
        {
            userCollection.InsertOne(user);
        }

        public void UpdateUser(User user)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Username, user.Username);
            userCollection.ReplaceOne(filter, user);
        }

        public void DeleteUser(Guid id)
        {
            var filter = filterBuilder.Eq(existingUser => existingUser.Id, id);
            userCollection.DeleteOne(filter);
        }
    }
}
