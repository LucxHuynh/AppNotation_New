using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotationApp.Services;

namespace NotationApp.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly IFirestoreService _firestoreService;

        [ObservableProperty]
        private string userProfileImage = "default_profile.png";

        [ObservableProperty]
        private string userDisplayName;

        [ObservableProperty]
        private string userEmail;

        [ObservableProperty]
        private bool isUserLoggedIn;



        public AppShellViewModel(IFirestoreService firestoreService)
        {
            _firestoreService = firestoreService;

            // Check if user is logged in from preferences
            var userId = Preferences.Default.Get("UserId", string.Empty);
            IsUserLoggedIn = !string.IsNullOrEmpty(userId);

            if (IsUserLoggedIn)
            {
                LoadUserData();
            }
        }

        private async void LoadUserData()
        {
            try
            {
                var userId = Preferences.Default.Get("UserId", string.Empty);
                if (!string.IsNullOrEmpty(userId))
                {
                    var profile = await _firestoreService.GetUserProfile(userId);
                    if (profile != null)
                    {
                        UserDisplayName = profile.DisplayName;
                        UserEmail = profile.Email;
                        UserProfileImage = !string.IsNullOrEmpty(profile.PhotoUrl)
                            ? profile.PhotoUrl
                            : "default_profile.png";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex}");
            }
        }

        public async Task UpdateUserData()
        {
            LoadUserData();
        }

        [RelayCommand]
        private async Task NavigateToProfile()
        {
            await Shell.Current.GoToAsync("//UserProfilePage");
        }
    }
}