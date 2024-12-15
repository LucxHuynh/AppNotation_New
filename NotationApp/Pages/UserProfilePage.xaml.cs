using NotationApp.ViewModels;

namespace NotationApp.Pages;

public partial class UserProfilePage : ContentPage
{
    private readonly UserProfileViewModel _viewModel;

    public UserProfilePage(UserProfileViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Debug output
        System.Diagnostics.Debug.WriteLine($"ViewModel initialized. Profile is null? {_viewModel.Profile == null}");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Kiểm tra thông tin đăng nhập
        var userId = Preferences.Default.Get("UserId", string.Empty);
        var userEmail = Preferences.Default.Get("UserEmail", string.Empty);

        System.Diagnostics.Debug.WriteLine($"UserProfilePage - Current UserId: {userId}");
        System.Diagnostics.Debug.WriteLine($"UserProfilePage - Current UserEmail: {userEmail}");

        var vm = (UserProfileViewModel)BindingContext;
        await vm.LoadProfileCommand.ExecuteAsync(null);
    }
}