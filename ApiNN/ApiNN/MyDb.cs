using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ApiNN
{
    internal class MyDb:DbContext
    {
        public MyDb()
            : base("Server=COMPONEND\\SQLEXPRESS;Database=MyDb;User Id=sa;Password=123456!Aa")
        {

        }
        public DbSet<SmsMessage> SmsMessages { get; set; }
    }
}
