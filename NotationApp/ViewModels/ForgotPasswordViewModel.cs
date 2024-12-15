using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using NotationApp.Services;
using System.Text.RegularExpressions;

namespace NotationApp.ViewModels
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private string email;
        private string statusMessage;
        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public IAsyncRelayCommand ResetPasswordCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }

        public ForgotPasswordViewModel(IAuthService authService)
        {
            _authService = authService;
            ResetPasswordCommand = new AsyncRelayCommand(ResetPasswordHandler);
            NavigateBackCommand = new RelayCommand(NavigateBackHandler);
        }

        private async Task ResetPasswordHandler()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                StatusMessage = "Please enter your email address.";
                return;
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, pattern))
            {
                StatusMessage = "Please enter a valid email address.";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Sending reset password email...";

                //await _authService.SendPasswordResetEmail(Email);

                await App.Current.MainPage.DisplayAlert(
                    "Success",
                    "Password reset email has been sent. Please check your inbox.",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Failed to send reset email";

                switch (ex.Reason)
                {
                    case AuthErrorReason.InvalidEmailAddress:
                        errorMessage = "Please enter a valid email address";
                        break;
                    case AuthErrorReason.UserNotFound:
                        errorMessage = "No account found with this email";
                        break;
                    default:
                        errorMessage = "Failed to send reset email: " + ex.Message;
                        break;
                }

                StatusMessage = errorMessage;
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NavigateBackHandler()
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}