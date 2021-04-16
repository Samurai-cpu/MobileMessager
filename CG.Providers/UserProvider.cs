using CG.Dal;
using SQLite;
using System;
using System.Collections.Generic;

namespace CG.Providers
{
    public interface IUserRepository
    {
         IEnumerable<User> GetItems();
    }
    public class UserProvider:IUserRepository
    {
        SQLiteConnection database;
        public UserProvider(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<User>();
        }

        public User GetItem() => database.Table<User>().FirstOrDefault();

        public IEnumerable<User> GetItems()
        {
            return database.Table<User>().ToList();
        }
        public User GetItem(int id)
        {
            return database.Get<User>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<User>(id);
        }
        public int SaveItem(User item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
        public int UpdateItem(int id)
        {
            return database.Update(id);
        }
    }
}
