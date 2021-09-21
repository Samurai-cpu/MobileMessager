using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CG.Providers.Base
{
    public class ContextProvider
    {
        public const string DATABASE_NAME = "users.db";
        public static readonly string DATABASE_PATH = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
        public static UserProvider database;

        public static SQLiteConnection GetDatabase()
        {
            return new SQLiteConnection("");
        }

        public static UserProvider Database
        {
            get
            {
                if (database == null)
                {
                    database = new UserProvider(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }
    }
    public class ProviderFactory
    {
        private static UserProvider userProvider;
        public static UserProvider UserProvider
        {
            get
            {
                if (userProvider == null)
                {
                    userProvider = new UserProvider(ContextProvider.DATABASE_PATH);
                }
                return userProvider;
            }
        }

        private static SessionProvider sessionProvider;
        public static SessionProvider SessionProvider
        {
            get
            {
                if (sessionProvider == null)
                {
                    sessionProvider = new SessionProvider(ContextProvider.DATABASE_PATH);
                }
                return sessionProvider;
            }
        }
        private static StrongKeyProvider strongKeyProvider;
        public static StrongKeyProvider StrongKeyProvider
        {
            get
            {
                if (strongKeyProvider == null)
                {
                    strongKeyProvider = new StrongKeyProvider(ContextProvider.DATABASE_PATH);
                }
                return strongKeyProvider;
            }
        }
    }
}
