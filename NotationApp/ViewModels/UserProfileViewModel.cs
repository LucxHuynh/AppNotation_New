using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotationApp.Models;
using NotationApp.Services;
using System.Diagnostics;

namespace NotationApp.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject
    {
        private readonly IFirestoreService _firestoreService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private UserProfile profile;

        [ObservableProperty]
        private string profileImage = "default_profile.png";

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        public UserProfileViewModel(IFirestoreService firestoreService, IAuthService authService)
        {
            _firestoreService = firestoreService;
            _authService = authService;
            Profile = new UserProfile();
        }

        [RelayCommand]
        private async Task LoadProfile()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Đang tải...";

                // Kiểm tra UserId
                var userId = Preferences.Default.Get("UserId", string.Empty);
                var userEmail = Preferences.Default.Get("UserEmail", string.Empty);

                System.Diagnostics.Debug.WriteLine($"Loading profile for UserId: {userId}");
                System.Diagnostics.Debug.WriteLine($"User Email from Preferences: {userEmail}");

                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "Vui lòng đăng nhập để xem hồ sơ";
                    return;
                }

                var loadedProfile = await _firestoreService.GetUserProfile(userId);

                if (loadedProfile != null)
                {
                    Profile = loadedProfile;
                    System.Diagnostics.Debug.WriteLine($"Loaded profile - Name: {Profile.DisplayName}, Email: {Profile.Email}");
                }
                else
                {
                    // Tạo profile mới với email đã lưu
                    Profile = new UserProfile
                    {
                        Id = userId,
                        Email = userEmail,
                        DisplayName = userEmail?.Split('@')[0], // Tạo display name từ email
                        DateJoined = DateTime.UtcNow
                    };

                    System.Diagnostics.Debug.WriteLine("Creating new profile");
                    await SaveChanges();
                }

                // Set profile image
                ProfileImage = !string.IsNullOrEmpty(Profile.PhotoUrl)
                    ? Profile.PhotoUrl
                    : "default_profile.png";

                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = "Không tải được hồ sơ";
                System.Diagnostics.Debug.WriteLine($"LoadProfile Error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveChanges()
        {
            if (IsBusy || Profile == null)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Đang lưu...";

                string userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "Không tìm thấy ID người dùng";
                    return;
                }

                Profile.Id = userId;
                bool success = await _firestoreService.UpdateUserProfile(Profile);

                if (success)
                {
                    StatusMessage = "Hồ sơ được cập nhật thành công";
                    await Task.Delay(2000);
                    StatusMessage = string.Empty;
                    var shellViewModel = IPlatformApplication.Current.Services.GetService<AppShellViewModel>();
                    await shellViewModel?.UpdateUserData();
                }
                else
                {
                    StatusMessage = "Không thể cập nhật hồ sơ";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Lỗi lưu hồ sơ";
                System.Diagnostics.Debug.WriteLine($"Error in SaveChanges: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ChangePhoto()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Chọn ảnh hồ sơ"
                });

                if (photo != null)
                {
                    StatusMessage = "Đang tải ảnh...";
                    var userId = Preferences.Get("UserId", string.Empty);

                    if (string.IsNullOrEmpty(userId))
                    {
                        StatusMessage = "Không tìm thấy ID người dùng";
                        return;
                    }

                    using var stream = await photo.OpenReadAsync();
                    string photoUrl = await _firestoreService.UploadProfileImage(stream, userId);

                    if (!string.IsNullOrEmpty(photoUrl))
                    {
                        Profile.PhotoUrl = photoUrl;
                        ProfileImage = photoUrl;
                        await SaveChanges();
                    }
                    else
                    {
                        StatusMessage = "Không thể tải ảnh lên";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Error changing photo";
                System.Diagnostics.Debug.WriteLine($"Error in ChangePhoto: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SignOut()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Clear preferences
                Preferences.Clear();

                // Reset profile
                Profile = new UserProfile();
                ProfileImage = "default_profile.png";

                // Reset AppShell data
                var shellViewModel = IPlatformApplication.Current.Services.GetService<AppShellViewModel>();
                if (shellViewModel != null)
                {
                    shellViewModel.UserDisplayName = string.Empty;
                    shellViewModel.UserEmail = string.Empty;
                    shellViewModel.UserProfileImage = "default_profile.png";
                    shellViewModel.IsUserLoggedIn = false;
                }

                // Navigate to sign in page
                await Shell.Current.GoToAsync("//SignInPage");
            }
            catch (Exception ex)
            {
                StatusMessage = "Lỗi đăng xuất";
                Debug.WriteLine($"Error in SignOut: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}