using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace KeyLogReset.Model
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Key_Event> Key_Events { get; set; }
        public ApplicationContext() : base(System.IO.Path.GetFullPath(@".\KeyLogDBReset.db"))
        {
        }
    }
}
