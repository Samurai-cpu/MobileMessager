using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CG.Dal
{   

    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
