using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CG.Models
{   
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Login { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
