using SQLite;
////using SQLite.Async;
//using SQLite.Interop;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class App : Application
    {
        public static WoodyPlantRepository WoodyPlantRepo { get; private set; }
        public static WoodyPlantRepositoryLocal WoodyPlantRepoLocal { get; set; }
        public static WoodySettingRepository WoodySettingsRepo { get; private set; }
        public static WoodySearchRepository WoodySearchRepo { get; private set; }

        //public App(ISQLitePlatform sqliteplatform, string dbPath)
        public App(string dbPath)
        {
            InitializeComponent();

            // Initialize SQLite connection and DBConnection class to hold connection
            //SQLiteConnection newConn = new SQLiteConnection(sqliteplatform, dbPath, false);
            //DBConnection dbConn = new DBConnection(newConn);

            //SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(sqliteplatform, new SQLiteConnectionString(dbPath, false)));
            //DBConnection dbConnAsync = new DBConnection(newConnAsync);

            SQLiteConnection newConn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            DBConnection dbConn = new DBConnection(newConn);


            SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            DBConnection dbConnAsync = new DBConnection(newConnAsync);

            WoodyPlantRepo = new WoodyPlantRepository();
            WoodySettingsRepo = new WoodySettingRepository();
            WoodySearchRepo = new WoodySearchRepository();
            WoodyPlantRepoLocal = new WoodyPlantRepositoryLocal(WoodyPlantRepo.GetAllWoodyPlants());

            this.MainPage = new NavigationPage(new MainPage());
        }


    }
}
