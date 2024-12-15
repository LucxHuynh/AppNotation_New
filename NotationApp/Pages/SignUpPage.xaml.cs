using NotationApp.Pages;
using NotationApp.Services;
using NotationApp.ViewModels;

namespace NotationApp.Pages;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    //private async void Login_Clicked(object sender, EventArgs e)
    //{
    //    var signInPage = IPlatformApplication.Current.Services.GetService<SignInPage>();

    //    if (signInPage != null)
    //    {
    //        await Navigation.PushModalAsync(signInPage);
    //    }
    //    else
    //    {
    //        await DisplayAlert("Error", "Unable to navigate to the Sign-In page.", "OK");
    //    }
    //}
}
