using SQLite;
using System;

namespace PortableApp.Models
{
    [Table("woody_settings")]
    public class WoodySetting
    {
        [PrimaryKey, AutoIncrement]
        public int settingid { get; set; }
        public string name { get; set; }
        public DateTime? valuetimestamp { get; set; }
        public string valuetext { get; set; }
        public decimal? valueamount { get; set; }
        public bool? valuebool { get; set; }
        public long? valueint { get; set; }

    }
}
