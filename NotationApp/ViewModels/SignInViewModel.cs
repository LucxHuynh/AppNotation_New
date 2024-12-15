using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.DependencyInjection;
using NotationApp.Models;
using NotationApp.Pages;
using NotationApp.Services;
using System;
using System.Threading.Tasks;

namespace NotationApp.ViewModels
{
    internal partial class SignInViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigation _navigation;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string emailError;

        [ObservableProperty]
        private string passwordError;

        [ObservableProperty]
        private bool isValid;

        public IAsyncRelayCommand SignInCommand { get; }
        public IRelayCommand NavigateToRegisterCommand { get; }

        public SignInViewModel(INavigation navigation, IAuthService authService)
        {
            _authService = authService;
            _navigation = navigation;

            // Initialize commands using explicitly defined methods
            SignInCommand = new AsyncRelayCommand(SignInCommandHandler);
            NavigateToRegisterCommand = new RelayCommand(NavigateToRegisterCommandHandler);
        }

        private void ValidateEmail()
        {
            EmailError = string.Empty;
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = ValidationHelper.Errors.RequiredField;
            }
            else if (!ValidationHelper.IsValidEmail(Email))
            {
                EmailError = ValidationHelper.Errors.InvalidEmail;
            }
            UpdateIsValid();
        }

        private void ValidatePassword()
        {
            PasswordError = string.Empty;
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = ValidationHelper.Errors.RequiredField;
            }
            else if (Password.Length < 6)
            {
                PasswordError = ValidationHelper.Errors.PasswordTooShort;
            }
            UpdateIsValid();
        }

        partial void OnEmailChanged(string value)
        {
            ValidateEmail();
        }

        partial void OnPasswordChanged(string value)
        {
            ValidatePassword();
        }

        private void UpdateIsValid()
        {
            IsValid = string.IsNullOrEmpty(EmailError) &&
                     string.IsNullOrEmpty(PasswordError);
        }

        // Explicit method for SignInCommand
        private async Task SignInCommandHandler()
        {
            await SignIn(Email, Password);
        }

        // Explicit method for NavigateToRegisterCommand
        private void NavigateToRegisterCommandHandler()
        {
            NavigateToRegister();
        }

        private async Task SignIn(string email, string password)
        {
            ValidateEmail();
            ValidatePassword();

            if (!IsValid)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Validation Error",
                    "Please fix all validation errors before proceeding.",
                    "OK");
                return;
            }

            try
            {
                IsLoading = true;

                var result = await _authService.SignInWithEmailAndPassword(email, password);
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
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected Error: {ex}");
                await App.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }



        private async void NavigateToRegister()
        {
            try
            {
                var signUpPage = new SignUpPage(
                    new SignUpViewModel(
                        IPlatformApplication.Current.Services.GetService<IAuthService>(),
                        IPlatformApplication.Current.Services.GetService<IFirestoreService>()
                    )
                );

                await _navigation.PushModalAsync(signUpPage);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
