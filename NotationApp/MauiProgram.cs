using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using NotationApp.Database;
using NotationApp.Extensions;
using NotationApp.Pages;
using NotationApp.Services;
using NotationApp.ViewModels;
using Plugin.Maui.Audio;

namespace NotationApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                   .UseMauiCommunityToolkit()
                   .UseAudioManager()
                   .ConfigureFonts(fonts =>
                   {
                       fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                       fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                   });

            // Configure entry handler
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
            handler.PlatformView.Background = null;
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register Database Service
            builder.Services.AddSingleton<NoteDatabase>(_ =>
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Notes.db3");
                return new NoteDatabase(dbPath);
            });

            // Register Services
            RegisterServices(builder.Services);

            // Register ViewModels
            RegisterViewModels(builder.Services);

            // Register Pages
            RegisterPages(builder.Services);

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Core Services
            services.AddSingleton<IFirestoreService, FirestoreService>();
            services.AddSingleton<IAuthService, AuthService>();

            // Additional Services
            services.AddSingleton<IAudioManager>(AudioManager.Current);

            // Add any other services your app needs
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            // Shell ViewModel
            services.AddSingleton<AppShellViewModel>();

            // Main ViewModels
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<UserProfileViewModel>();
            services.AddTransient<ForgotPasswordViewModel>();
            services.AddTransient<BinPageViewModel>();
            services.AddTransient<DrawingPageViewModel>();
            services.AddTransient<NotePageViewModel>();
            services.AddTransient<ShareDialogViewModel>();

            // Add any other ViewModels
        }

        private static void RegisterPages(IServiceCollection services)
        {
            // Register AppShell
            services.AddSingleton<AppShell>();

            // Main Pages
            services.AddTransient<HomePage>();
            services.AddTransient<NotePage>();
            services.AddTransient<DrawingPage>();
            services.AddTransient<SignInPage>();
            services.AddTransient<SignUpPage>();
            services.AddTransient<UserProfilePage>();
            services.AddTransient<ForgotPasswordPage>();
            services.AddTransient<BinPage>();

#if ANDROID
        services.AddTransient<Platforms.Android.WelcomePage>();
#endif

            // Add any other pages
        }
    }
}
