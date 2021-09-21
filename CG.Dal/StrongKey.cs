using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Dal
{
    [Table("StrongKeys")]
    public class StrongKey
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public long Id { get; set; }
        public byte[] Cypher { get; set; }
    }
}
