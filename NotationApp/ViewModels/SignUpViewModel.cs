using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using NotationApp;
using NotationApp.Models;
using NotationApp.Services;
using NotationApp.ViewModels;
using System.Windows.Input;

public partial class SignUpViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IFirestoreService _firestoreService;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isLoading;

    public IRelayCommand NavigateBackCommand { get; }
    public IAsyncRelayCommand RegisterUserCommand { get; }

    public ICommand GoogleSignInCommand => new Command(async () => await SignInWithGoogle());


    // Bỏ INavigation dependency
    public SignUpViewModel(IAuthService authService, IFirestoreService firestoreService)
    {
        _authService = authService;
        _firestoreService = firestoreService;

        RegisterUserCommand = new AsyncRelayCommand(SignUpCommandHandler);
        NavigateBackCommand = new RelayCommand(NavigateBackCommandHandler);
    }


    private async Task SignUpCommandHandler()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Email và mật khẩu không được để trống.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            StatusMessage = "Mật khẩu không khớp.";
            return;
        }

        await RegisterUserAsync(Email, Password);
    }

    private void NavigateBackCommandHandler()
    {
        Shell.Current.Navigation.PopModalAsync();
    }

    private string ExtractUsernameFromEmail(string email)
    {
        return email.Split('@')[0];
    }

    private async Task RegisterUserAsync(string email, string password)
    {
        try
        {
            StatusMessage = "Signing up...";
            var authResult = await _authService.CreateUserWithEmailAndPassword(email, password);

            if (authResult?.User != null)
            {
                // Save token and user ID in preferences
                Preferences.Set("FirebaseToken", authResult.User.Credential.IdToken);
                Preferences.Set("UserId", authResult.User.Uid);

                // Create and save user profile in Firestore
                var userProfile = new UserProfile
                {
                    Id = authResult.User.Uid,
                    Email = email,
                    DisplayName = ExtractUsernameFromEmail(email),
                    DateJoined = DateTime.UtcNow,
                    PhotoUrl = "default_profile.png"
                };

                bool profileSaved = await _firestoreService.UpdateUserProfile(userProfile);

                if (profileSaved)
                {
                    await App.Current.MainPage.DisplayAlert("Thành công", "Tài khoản được tạo thành công!", "OK");
                    Shell.Current.Navigation.PopModalAsync();
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    StatusMessage = "Đã tạo tài khoản nhưng không lưu được hồ sơ. Vui lòng thử cập nhật hồ sơ của bạn sau.";
                }
            }
            else
            {
                StatusMessage = "Đăng ký không thành công. Vui lòng thử lại.";
            }
        }
        catch (FirebaseAuthException ex)
        {
            StatusMessage = $"Registration failed: {ex.Reason}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"An error occurred: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SignInWithGoogle()
    {
        try
        {
            IsLoading = true;

            var result = await _authService.SignInWithGoogle();
            if (result?.User != null)
            {
                // Save user info
                Preferences.Default.Set("UserId", result.User.Uid);
                Preferences.Default.Set("UserEmail", result.User.Info.Email);
                Preferences.Default.Set("IsLoggedIn", true);

                // Update UI
                var shellViewModel = IPlatformApplication.Current.Services.GetService<AppShellViewModel>();
                if (shellViewModel != null)
                {
                    shellViewModel.IsUserLoggedIn = true;
                    await shellViewModel.UpdateUserData();
                }

                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Lỗi",
                    "Không thể đăng nhập bằng Google",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert(
                "Lỗi",
                    "Không thể đăng nhập bằng Google",
                    "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}