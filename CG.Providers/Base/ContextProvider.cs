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
        public static UserProvider database;

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
}
