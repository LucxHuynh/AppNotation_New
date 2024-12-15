using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using NotationApp.Database;
using NotationApp.Pages;
using NotationApp.Services;
using NotationApp.ViewModels;

namespace NotationApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                   .UseMauiCommunityToolkit()
                   .ConfigureFonts(fonts =>
                   {
                       fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                       fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                   });

            // Configure entry handler for Android
#if ANDROID
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = null;
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register Core Services
            RegisterServices(builder.Services);

            // Register ViewModels and Pages
            RegisterViewModels(builder.Services);
            RegisterPages(builder.Services);

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Singleton Services
            services.AddSingleton<IFirestoreService, FirestoreService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<NoteDatabase>(_ =>
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Notes.db3");
                return new NoteDatabase(dbPath);
            });
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            // Shell ViewModel
            services.AddSingleton<AppShellViewModel>();

            // Transient ViewModels
            var viewModelTypes = new[]
            {
                typeof(HomePageViewModel),
                typeof(SignInViewModel),
                typeof(SignUpViewModel),
                typeof(UserProfileViewModel),
                typeof(ForgotPasswordViewModel),
                typeof(BinPageViewModel),
                typeof(DrawingPageViewModel),
                typeof(NotePageViewModel),
                typeof(ShareDialogViewModel)
            };

            foreach (var type in viewModelTypes)
            {
                services.AddTransient(type);
            }
        }

        private static void RegisterPages(IServiceCollection services)
        {
            // Register AppShell
            services.AddSingleton<AppShell>();

            // Register Pages
            var pageTypes = new[]
            {
                typeof(HomePage),
                typeof(NotePage),
                typeof(DrawingPage),
                typeof(SignInPage),
                typeof(SignUpPage),
                typeof(UserProfilePage),
                typeof(ForgotPasswordPage),
                typeof(BinPage)
            };

            foreach (var type in pageTypes)
            {
                services.AddTransient(type);
            }

#if ANDROID
            services.AddTransient<Platforms.Android.WelcomePage>();
#endif
        }
    }
}