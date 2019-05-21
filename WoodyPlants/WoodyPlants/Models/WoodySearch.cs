using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("woody_search")]
    public class WoodySearch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Characteristic { get; set; }
        public string Name { get; set; }
        public bool? Query { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string SearchString1 { get; set; }
        public string SearchString2 { get; set; }
        public string SearchString3 { get; set; }
        public string SearchString4 { get; set; }
        public string SearchString5 { get; set; }
        public string SearchString6 { get; set; }
        public string SearchString7 { get; set; }
        public string SearchString8 { get; set; }
        public string SearchString9 { get; set; }
        public string SearchString10 { get; set; }

        public string IconFileName { get; set; }
    }
}
