using NotationApp.Pages;
using NotationApp.ViewModels;

namespace NotationApp
{
    public partial class AppShell : Shell
    {
        private readonly AppShellViewModel _viewModel;

        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            // Đăng ký route cho NotePage để có thể điều hướng từ các trang khác
            Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
            Routing.RegisterRoute(nameof(DrawingPage), typeof(DrawingPage));
#if ANDROID
    Routing.RegisterRoute(nameof(Platforms.Android.WelcomePage), typeof(Platforms.Android.WelcomePage));
#endif
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));

            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
        }
    }
}
