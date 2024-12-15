using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using NotationApp;
using NotationApp.Models;
using NotationApp.Services;

public class SignUpViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IFirestoreService _firestoreService;

    private string email;
    private string password;
    private string confirmPassword;
    private string statusMessage;

    public string Email
    {
        get => email;
        set => SetProperty(ref email, value);
    }

    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    public string ConfirmPassword
    {
        get => confirmPassword;
        set => SetProperty(ref confirmPassword, value);
    }

    public string StatusMessage
    {
        get => statusMessage;
        set => SetProperty(ref statusMessage, value);
    }

    public IRelayCommand NavigateBackCommand { get; }
    public IAsyncRelayCommand RegisterUserCommand { get; }


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
            StatusMessage = "Email and password cannot be empty.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            StatusMessage = "Passwords do not match.";
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
                    await App.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
                    Shell.Current.Navigation.PopModalAsync();
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    StatusMessage = "Account created but failed to save profile. Please try updating your profile later.";
                }
            }
            else
            {
                StatusMessage = "Registration failed. Please try again.";
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
}