﻿using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class App : Application
    {
        public static WoodyPlantRepository WoodyPlantRepo { get; private set; }
        public static WoodyPlantRepositoryLocal WoodyPlantRepoLocal { get; set; }
        public static WoodyPlantImageRepository WoodyPlantImageRepo { get; private set; }
        public static WoodySettingRepository WoodySettingsRepo { get; private set; }
        public static WoodySearchRepository WoodySearchRepo { get; private set; }
        public static WoodyPlantImageRepositoryLocal WoodyPlantImageRepoLocal { get; set; }

        public App(ISQLitePlatform sqliteplatform, string dbPath)
        {
            InitializeComponent();

            // Initialize SQLite connection and DBConnection class to hold connection
            SQLiteConnection newConn = new SQLiteConnection(sqliteplatform, dbPath, false);
            DBConnection dbConn = new DBConnection(newConn);

            SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(sqliteplatform, new SQLiteConnectionString(dbPath, false)));
            DBConnection dbConnAsync = new DBConnection(newConnAsync);

            WoodyPlantImageRepo = new WoodyPlantImageRepository();
            WoodyPlantRepo = new WoodyPlantRepository();
            WoodySettingsRepo = new WoodySettingRepository();
            WoodySearchRepo = new WoodySearchRepository();


            WoodyPlantRepoLocal = new WoodyPlantRepositoryLocal(WoodyPlantRepo.GetAllWoodyPlants());
            WoodyPlantImageRepoLocal = new WoodyPlantImageRepositoryLocal(WoodyPlantImageRepo.GetAllWetlandPlantImages());

            this.MainPage = new NavigationPage(new MainPage());
        }


    }
}
