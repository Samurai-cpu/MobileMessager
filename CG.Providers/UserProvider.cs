using CG.Dal;
using CG.Providers.Base;
using SQLite;
using System;
using System.Collections.Generic;

namespace CG.Providers
{
    public class BaseProvider<T> where T:class
    {
        protected ContextProvider contextProvider;

        public BaseProvider()
        {
            contextProvider = new ContextProvider();
        }

        //protected SQLiteConnection GetContextProvider() => new SQLiteConnection();
    }
    public interface IUserProvider
    {
         IEnumerable<User> GetItems();
    }
    public class UserProvider:BaseProvider<User> ,IUserProvider
    {
        SQLiteConnection database;
        public UserProvider(string databasePath) : base()
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<User>();
        }

        public User GetItem() => database.Table<User>().FirstOrDefault();

        public IEnumerable<User> GetItems()
        {
           // using (var db = GetContextProvider())
           // {
                return database.Table<User>().ToList();
           // }
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
