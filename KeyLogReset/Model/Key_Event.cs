using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyLogReset.Model
{
    [Table("Key_Event")]
    public class Key_Event
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("KeyName")]
        public string KeyName { get; set; }
        [Column("ProgramName")]
        public string ProgramName { get; set; }
        [Column("DatetimeEvent")]
        public DateTime DatetimeEvent { get; set; }
    }
}
