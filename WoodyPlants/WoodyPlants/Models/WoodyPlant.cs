//using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PortableApp.Models
{
    [Table("woody_plants")]
    public class WoodyPlant
    {
        [PrimaryKey]
        public int plantid { get; set; }

        [Unique]
        public string plantname { get; set; }


        public bool isFavorite { get; set; }
    }

}
