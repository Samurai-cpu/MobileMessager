using CG.Dal;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Providers
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetItems();
    }
    public class SessionProvider : ISessionRepository
    {
        SQLiteConnection database;
        public SessionProvider(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Session>();
        }

        public Session GetItem() => database.Table<Session>().FirstOrDefault();

        public IEnumerable<Session> GetItems()
        {
            return database.Table<Session>().ToList();
        }
        public Session GetItem(long id)
        {
            return database.Get<Session>(id);
        }
        public long DeleteItem(long id)
        {
            return database.Delete<Session>(id);
        }
        public long SaveItem(Session item)
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
        public long UpdateItem(long id)
        {
            return database.Update(id);
        }
    }
}
