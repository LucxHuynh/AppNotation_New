
using Microsoft.Extensions.DependencyInjection;
using NotationApp.Database;
using System.Diagnostics;
using System.IO;

namespace NotationApp
{
    public partial class App : Application
    {
        private static NoteDatabase database;

        public static NoteDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "Notes.db3");
                    database = new NoteDatabase(dbPath);
                    Debug.WriteLine("Database initialized from getter");
                }
                return database;
            }
        }

        public App(IServiceProvider serviceProvider)
        {
            //InitializeComponent();
            //MainPage = new AppShell();

            //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3");
            //Database = new NoteDatabase(dbPath);

            try
            {
                InitializeComponent();
                var appShell = serviceProvider.GetRequiredService<AppShell>();

                MainPage = appShell;

                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3");
                database = new NoteDatabase(dbPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in App constructor: {ex}");
            }

        }
        protected override async void OnStart()
        {
            base.OnStart();

            try
            {
                bool hasSeenWelcome = Preferences.Get("HasSeenWelcomeScreen", false);
                bool isLoggedIn = !string.IsNullOrEmpty(Preferences.Get("UserId", string.Empty));

#if ANDROID
        if (!hasSeenWelcome)
        {
            await Shell.Current.GoToAsync("//WelcomePage");
        }
        else if (isLoggedIn)
        {
            await Shell.Current.GoToAsync("//HomePage");
        }
        else 
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }
#else
                if (isLoggedIn)
                {
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    await Shell.Current.GoToAsync("//SignInPage");
                }
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnStart: {ex.Message}");
                await Shell.Current.GoToAsync("//SignInPage");
            }
        }
    }
}
