using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using NotationApp.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;
using NotationApp.Services;
using NotationApp.Database;

namespace NotationApp.ViewModels
{
    public partial class ShareDialogViewModel : ObservableObject
    {
        private readonly IFirestoreService _firestoreService;
        private readonly NoteDatabase _database;
        private readonly object _sharedItem;
        private readonly Action _closeDialogAction;
        private string _itemId;
        private string _itemType;

        public ObservableCollection<SharedUserInfo> SharedUsers { get; } = new();
        public List<string> Permissions { get; } = new() { "ReadOnly", "ReadWrite", "Full" };

        [ObservableProperty]
        private string emailInput;

        [ObservableProperty]
        private string selectedPermission = "ReadOnly";

        [ObservableProperty]
        private string shareLink;

        private Dictionary<string, string> currentSharedUsers = new();

        public ShareDialogViewModel(object item, IFirestoreService firestoreService, NoteDatabase database, Action closeDialog)
        {
            _sharedItem = item ?? throw new ArgumentNullException(nameof(item));
            _firestoreService = firestoreService ?? throw new ArgumentNullException(nameof(firestoreService));
            _database = App.Database;  // Luôn sử dụng App.Database
            _closeDialogAction = closeDialog;

            Debug.WriteLine("ShareDialogViewModel initialized");
            Debug.WriteLine($"Database null? {_database == null}");
            Debug.WriteLine($"SharedItem null? {_sharedItem == null}");

            InitializeShareData();
        }

        private void InitializeShareData()
        {
            try
            {
                var currentUserId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    Debug.WriteLine("Warning: No current user ID found");
                    return;
                }

                if (_sharedItem is Note_Realtime note)
                {
                    _itemId = note.Id.ToString();
                    _itemType = "Note";
                    ShareLink = string.IsNullOrEmpty(note.ShareLink)
                        ? $"https://notationapp.com/share/{Guid.NewGuid()}"
                        : note.ShareLink;

                    // Kiểm tra owner
                    if (note.OwnerId != currentUserId)
                    {
                        Debug.WriteLine("Warning: Current user is not the owner of this note");
                        return;
                    }

                    if (!string.IsNullOrEmpty(note.SharedWithUsers))
                    {
                        try
                        {
                            currentSharedUsers = JsonConvert.DeserializeObject<Dictionary<string, string>>(note.SharedWithUsers)
                                               ?? new Dictionary<string, string>();

                            foreach (var pair in currentSharedUsers)
                            {
                                SharedUsers.Add(new SharedUserInfo(pair.Key, pair.Value));
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error parsing SharedWithUsers: {ex.Message}");
                            currentSharedUsers = new Dictionary<string, string>();
                        }
                    }
                }
                else if (_sharedItem is Drawing drawing)
                {
                    _itemId = drawing.Id.ToString();
                    _itemType = "Draw";
                    ShareLink = string.IsNullOrEmpty(drawing.ShareLink)
                        ? $"https://notationapp.com/share/{Guid.NewGuid()}"
                        : drawing.ShareLink;

                    // Kiểm tra owner
                    if (drawing.OwnerId != currentUserId)
                    {
                        Debug.WriteLine("Warning: Current user is not the owner of this drawing");
                        return;
                    }

                    if (!string.IsNullOrEmpty(drawing.SharedWithUsers))
                    {
                        try
                        {
                            currentSharedUsers = JsonConvert.DeserializeObject<Dictionary<string, string>>(drawing.SharedWithUsers)
                                               ?? new Dictionary<string, string>();

                            foreach (var pair in currentSharedUsers)
                            {
                                SharedUsers.Add(new SharedUserInfo(pair.Key, pair.Value));
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error parsing SharedWithUsers: {ex.Message}");
                            currentSharedUsers = new Dictionary<string, string>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing share data: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddEmail()
        {
            if (string.IsNullOrWhiteSpace(EmailInput))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter an email address", "OK");
                return;
            }

            try
            {
                Debug.WriteLine($"Starting AddEmail for: {EmailInput}");
                var database = App.Database;

                // Validate email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(EmailInput, pattern))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address", "OK");
                    return;
                }

                // Check if email already exists
                if (SharedUsers.Any(u => u.Email == EmailInput))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This email has already been added", "OK");
                    return;
                }

                // Thêm người dùng mới vào SharedUsers và dictionary
                var newSharedUser = new SharedUserInfo(EmailInput, SelectedPermission);
                SharedUsers.Add(newSharedUser);
                currentSharedUsers[EmailInput] = SelectedPermission;

                if (_sharedItem is Note_Realtime note)
                {
                    Debug.WriteLine($"Processing Note: ID={note.Id}, OwnerId={note.OwnerId}");

                    // Serialize shared users to JSON properly
                    var sharedUsersJson = JsonConvert.SerializeObject(currentSharedUsers);
                    Debug.WriteLine($"Serialized shared users: {sharedUsersJson}");

                    note.IsShared = true;
                    note.SharedWithUsers = sharedUsersJson;

                    if (database != null)
                    {
                        await database.UpdateNoteAsync(note);
                        Debug.WriteLine("Note update completed successfully");
                    }
                    else
                    {
                        throw new InvalidOperationException("Database instance is not available");
                    }
                }
                else if (_sharedItem is Drawing drawing)
                {
                    var sharedUsersJson = JsonConvert.SerializeObject(currentSharedUsers);
                    drawing.IsShared = true;
                    drawing.SharedWithUsers = sharedUsersJson;

                    if (database != null)
                    {
                        await database.UpdateDrawingAsync(drawing);
                        Debug.WriteLine("Drawing update completed successfully");
                    }
                }

                // Clear input
                EmailInput = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddEmail: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add email: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task RemoveEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return;

            try
            {
                Debug.WriteLine($"Attempting to remove sharing for email: {email}");

                // 1. Remove from SharedUsers collection in UI
                var userToRemove = SharedUsers.FirstOrDefault(u => u.Email == email);
                if (userToRemove != null)
                {
                    SharedUsers.Remove(userToRemove);
                    Debug.WriteLine($"Removed user from SharedUsers collection");
                }

                // 2. Update the main item (Note or Drawing)
                if (_sharedItem is Note_Realtime note)
                {
                    try
                    {
                        // Get current shared users
                        var sharedUsersDict = string.IsNullOrEmpty(note.SharedWithUsers)
                            ? new Dictionary<string, string>()
                            : JsonConvert.DeserializeObject<Dictionary<string, string>>(note.SharedWithUsers);

                        // Remove the email
                        if (sharedUsersDict.ContainsKey(email))
                        {
                            sharedUsersDict.Remove(email);
                            note.SharedWithUsers = JsonConvert.SerializeObject(sharedUsersDict);
                            note.IsShared = sharedUsersDict.Count > 0;

                            Debug.WriteLine($"Updated note SharedWithUsers: {note.SharedWithUsers}");
                            await _database.UpdateNoteAsync(note);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error updating note: {ex.Message}");
                        throw;
                    }
                }
                else if (_sharedItem is Drawing drawing)
                {
                    try
                    {
                        // Get current shared users
                        var sharedUsersDict = string.IsNullOrEmpty(drawing.SharedWithUsers)
                            ? new Dictionary<string, string>()
                            : JsonConvert.DeserializeObject<Dictionary<string, string>>(drawing.SharedWithUsers);

                        // Remove the email
                        if (sharedUsersDict.ContainsKey(email))
                        {
                            sharedUsersDict.Remove(email);
                            drawing.SharedWithUsers = JsonConvert.SerializeObject(sharedUsersDict);
                            drawing.IsShared = sharedUsersDict.Count > 0;

                            Debug.WriteLine($"Updated draw SharedWithUsers: {drawing.SharedWithUsers}");
                            await _database.UpdateDrawingAsync(drawing);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error updating draw: {ex.Message}");
                        throw;
                    }
                }

                // 3. Remove from recipient's SharedWithMe in Firestore
                try
                {
                    var recipientProfile = await _firestoreService.GetUserProfile(email);
                    if (recipientProfile?.SharedWithMe != null)
                    {
                        var itemIdentifier = _itemType == "Note" ? _itemId : $"drawing_{_itemId}";
                        recipientProfile.SharedWithMe.RemoveAll(item => item.Contains(itemIdentifier));

                        var updateResult = await _firestoreService.UpdateUserProfile(recipientProfile);
                        Debug.WriteLine($"Updated recipient profile result: {updateResult}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error updating recipient profile: {ex.Message}");
                    // Don't throw here - we still want to save the local changes
                }

                Debug.WriteLine("Remove sharing completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RemoveEmail: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Failed to remove shared access. Please try again.",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task Share()
        {
            try
            {
                var currentUserId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User not authenticated", "OK");
                    return;
                }

                // Validate all email addresses first
                foreach (var user in SharedUsers)
                {
                    if (!await _firestoreService.IsValidUser(user.Email))
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            $"User with email {user.Email} does not exist in the system",
                            "OK");
                        return;
                    }
                }

                var sharedUsersDict = SharedUsers.ToDictionary(
                    user => user.Email,
                    user => user.Permission
                );

                if (_sharedItem is Note_Realtime note)
                {
                    if (note.OwnerId != currentUserId)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            "You don't have permission to share this note",
                            "OK");
                        return;
                    }

                    // Cập nhật thông tin chia sẻ trong note
                    note.IsShared = SharedUsers.Any();
                    note.ShareLink = ShareLink;
                    note.SharedWithUsers = JsonConvert.SerializeObject(sharedUsersDict);

                    // Lưu vào database local
                    await _database.UpdateNoteAsync(note);

                    // Cập nhật thông tin trong Firestore cho mỗi người nhận
                    foreach (var user in SharedUsers)
                    {
                        var recipientProfile = await _firestoreService.GetUserProfileByEmail(user.Email);
                        if (recipientProfile != null)
                        {
                            await _firestoreService.UpdateSharedItems(
                                recipientProfile.Id,
                                note.Id.ToString(),
                                "note");
                        }
                    }
                }
                // Similar logic for Drawing...

                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    "Share settings updated successfully",
                    "OK");
                _closeDialogAction?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Share: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Failed to update share settings",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task CopyLink()
        {
            if (!string.IsNullOrEmpty(ShareLink))
            {
                await Clipboard.SetTextAsync(ShareLink);
                await Application.Current.MainPage.DisplayAlert("Success", "Share link copied to clipboard", "OK");
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _closeDialogAction?.Invoke();
        }
    }
}