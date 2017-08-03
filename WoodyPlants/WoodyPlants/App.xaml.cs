using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using Xamarin.Forms;

namespace WoodyPlants
{
   

    public partial class App : Application
    {
        public static WoodyPlantRepository WoodyPlantRepo { get; private set; }
        public App(ISQLitePlatform sqliteplatform, string dbPath)
        {
            InitializeComponent();

            // Initialize SQLite connection and DBConnection class to hold connection
            SQLiteConnection newConn = new SQLiteConnection(sqliteplatform, dbPath, false);
            DBConnection dbConn = new DBConnection(newConn);

            SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(sqliteplatform, new SQLiteConnectionString(dbPath, false)));
            DBConnection dbConnAsync = new DBConnection(newConnAsync);

            WoodyPlantRepo = new WoodyPlantRepository();

            this.MainPage = new NavigationPage(new MainPage());
        }
    }
}
