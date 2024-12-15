using NotationApp.Pages;
using NotationApp.Services;
using NotationApp.ViewModels;

namespace NotationApp.Pages;

public partial class SignInPage : ContentPage
{
    public SignInPage(IAuthService authService)
    {
        InitializeComponent();
        BindingContext = new SignInViewModel(Navigation, authService);
    }

}
