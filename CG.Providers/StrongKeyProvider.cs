using CG.Dal;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Providers
{
    public interface IStrongKeyProvider
    {
        IEnumerable<StrongKey> GetItems();
    }
    public class StrongKeyProvider : IStrongKeyProvider
    {
        SQLiteConnection database;
        public StrongKeyProvider(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<StrongKey>();
        }

        public StrongKey GetItem() => database.Table<StrongKey>().FirstOrDefault();

        public IEnumerable<StrongKey> GetItems()
        {
            return database.Table<StrongKey>().ToList();
        }
        public StrongKey GetItem(long id)
        {
            return database.Get<StrongKey>(id);
        }
        public long DeleteItem(long id)
        {
            return database.Delete<StrongKey>(id);
        }
        public long SaveItem(StrongKey item)
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
