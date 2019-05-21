using SQLite;

namespace PortableApp
{
    public class DBConnection
    {
        protected static SQLiteConnection conn { get; set; }
        protected static SQLiteAsyncConnection connAsync { get; set; }

        // Initialize connection if it hasn't already been initialized
        public DBConnection(dynamic newConn = null)
        {
            if (conn == null && newConn.GetType() == typeof(SQLiteConnection)) { conn = newConn; }
            if (connAsync == null && newConn.GetType() == typeof(SQLiteAsyncConnection)) { connAsync = newConn; }
        }

        // Seed database with Plant info
        //public void SeedDB()
        //{
        //    ObservableCollection<WoodyPlant> plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepo.GetAllWoodyPlants());
        //    if (plants.Count == 0)
        //    {
        //        conn.Insert(new WoodyPlant() { commonname = "Test" });
        //    }
        //}

    }
}