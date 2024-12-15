using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotationApp.Models;
using NotationApp.Database;
using NotationApp.Pages;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Net.Http.Json;
using NotationApp.Services;
using CommunityToolkit.Maui.Views;
using System.Globalization;

namespace NotationApp.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly NoteDatabase _database;
        private readonly IFirestoreService _firestoreService;
        private readonly string _currentUserId;

        private const int MaxRetries = 3;
        private const int InitialDelayMs = 1000;

        public ObservableCollection<Note_Realtime> Notes { get; private set; } = new();
        public ObservableCollection<Note_Realtime> SharedWithMeNotes { get; private set; } = new();
        public ObservableCollection<Drawing> Drawings { get; private set; } = new();
        public ObservableCollection<Drawing> SharedWithMeDrawings { get; private set; } = new();
        public string SelectedTag { get; private set; } = "All";

        [ObservableProperty] private Note_Realtime selectedNote;
        [ObservableProperty] private Drawing selectedDrawing;
        [ObservableProperty] private string searchQuery;
        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string statusMessage;

        public HomePageViewModel(IFirestoreService firestoreService = null)
        {
            _database = App.Database;
            _firestoreService = firestoreService;
            _currentUserId = Preferences.Get("UserId", string.Empty);
        }


        #region Note Loading and Management

        private async Task SyncToRemoteDatabase(Note_Realtime note)
        {
            if (!IsConnectedToInternet()) return;

            try
            {
                Debug.WriteLine($"Starting sync to Firebase for note ID: {note.Id}");
                Debug.WriteLine($"SharedWithUsers before sync: {note.SharedWithUsers}");

                string firebaseUrl = $"https://notationapp-98854-default-rtdb.firebaseio.com/notes/{note.Id}.json";
                using var client = new HttpClient();

                // Đảm bảo SharedWithUsers không null trước khi sync
                if (string.IsNullOrEmpty(note.SharedWithUsers))
                {
                    note.SharedWithUsers = JsonConvert.SerializeObject(new Dictionary<string, string>());
                }

                // Tạo object để sync
                var noteToSync = new
                {
                    Id = note.Id,
                    Title = note.Title,
                    Text = note.Text,
                    CreateDate = note.CreateDate,
                    UpdateDate = note.UpdateDate,
                    IsDeleted = note.IsDeleted,
                    IsSynced = note.IsSynced,
                    TagName = note.TagName,
                    IsPinned = note.IsPinned,
                    OwnerId = note.OwnerId,
                    IsShared = note.IsShared,
                    Permission = note.Permission,
                    ShareLink = note.ShareLink,
                    SharedWithUsers = note.SharedWithUsers  // Ensure this is included
                };

                var jsonData = JsonConvert.SerializeObject(noteToSync);
                Debug.WriteLine($"Data being sent to Firebase: {jsonData}");

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    note.IsSynced = true;
                    await _database.UpdateNoteAsync(note);
                    Debug.WriteLine($"Successfully synced note {note.Id} to Firebase");

                    // Verify the sync
                    var verifyResponse = await client.GetAsync(firebaseUrl);
                    if (verifyResponse.IsSuccessStatusCode)
                    {
                        var verifyData = await verifyResponse.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Verification data from Firebase: {verifyData}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Failed to sync to Firebase. Status: {response.StatusCode}");
                    Debug.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during Firebase sync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        [RelayCommand]
        public async Task LoadAllContentAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading content...";

                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "User not logged in";
                    return;
                }

                await Task.WhenAll(
                    LoadNotesAsync(),
                    LoadDrawingsAsync()
                );

                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = "Error loading content";
                Debug.WriteLine($"Error in LoadAllContentAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task LoadNotesAsync()
        {
            try
            {
                IsLoading = true;
                Notes.Clear();
                SharedWithMeNotes.Clear();

                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "User not logged in";
                    return;
                }

                // Load owned notes
                var ownedNotes = await _database.GetOwnedNotesAsync(userId);
                foreach (var note in ownedNotes.Where(n => !n.IsDeleted))
                {
                    Notes.Add(note);
                }

                // Load shared notes
                var sharedNotes = await _database.GetSharedNotesAsync(userId);
                foreach (var note in sharedNotes.Where(n => !n.IsDeleted && n.OwnerId != userId))
                {
                    SharedWithMeNotes.Add(note);
                }

                Debug.WriteLine($"Loaded {Notes.Count} owned notes and {SharedWithMeNotes.Count} shared notes");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading notes: {ex.Message}";
                Debug.WriteLine($"Error in LoadNotesAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        //// Tải danh sách ghi chú
        //public async Task LoadNotesAsync()
        //{
        //    Notes.Clear();
        //    var notes = await _database.GetNotesAsync();
        //    Console.WriteLine($"Loading {notes.Count} notes to view");
        //    foreach (var note in notes.Where(n => !n.IsDeleted))
        //    {
        //        Notes.Add(note);
        //        Console.WriteLine($"Added note: {note.Id} - {note.Title}");
        //    }
        //}

        //// Tải danh sách hình vẽ
        //public async Task LoadDrawingsAsync()
        //{
        //    Drawings.Clear();
        //    var drawings = await _database.GetDrawingsAsync();
        //    foreach (var drawing in drawings)
        //    {
        //        if (!drawing.IsDeleted)
        //        {
        //            Drawings.Add(drawing);
        //        }
        //    }
        //}

        [RelayCommand]
        public async Task LoadDrawingsAsync()
        {
            try
            {
                IsLoading = true;
                Drawings.Clear();
                SharedWithMeDrawings.Clear();

                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "User not logged in";
                    return;
                }

                // Load owned drawings
                var ownedDrawings = await _database.GetOwnedDrawingsAsync(userId);
                foreach (var drawing in ownedDrawings.Where(d => !d.IsDeleted))
                {
                    Drawings.Add(drawing);
                }

                // Load shared drawings
                var sharedDrawings = await _database.GetSharedDrawingsAsync(userId);
                foreach (var drawing in sharedDrawings.Where(d => !d.IsDeleted && d.OwnerId != userId))
                {
                    SharedWithMeDrawings.Add(drawing);
                }

                Debug.WriteLine($"Loaded {Drawings.Count} owned drawings and {SharedWithMeDrawings.Count} shared drawings");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading drawings: {ex.Message}";
                Debug.WriteLine($"Error in LoadDrawingsAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddNote()
        {
            try
            {
                IsLoading = true;
                Debug.WriteLine("Creating new note...");

                var newNote = new Note_Realtime
                {
                    Title = "New Note",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                Debug.WriteLine($"New note created with initial values: Title={newNote.Title}, OwnerId={newNote.OwnerId}");

                Debug.WriteLine("Saving note to database...");
                var  result = await _database.SaveNoteAsync(newNote);
                Debug.WriteLine($"Save result: {result}");

                if (result > 0)
                {
                    Debug.WriteLine($"Note saved successfully with ID: {newNote.Id}");
                    Notes.Add(newNote);

                    try
                    {
                        Debug.WriteLine("Attempting to navigate to NotePage...");
                        await Shell.Current.GoToAsync(nameof(NotePage),
                            new Dictionary<string, object> { { "SelectedNote", newNote } });
                    }
                    catch (Exception navEx)
                    {
                        Debug.WriteLine($"Navigation error: {navEx.Message}");
                        StatusMessage = "Could not open new note page.";
                    }
                }
                else
                {
                    Debug.WriteLine("Failed to save note");
                    StatusMessage = "Failed to create new note.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddNote: {ex}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                StatusMessage = $"Error creating note: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateToNoteAsync(Note_Realtime note)
        {
            if (note == null) return;

            try
            {
                await Shell.Current.GoToAsync(nameof(NotePage),
                    new Dictionary<string, object> { { "SelectedNote", note } });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error navigating to note: {ex.Message}";
                Debug.WriteLine($"Error in NavigateToNoteAsync: {ex}");
            }
        }

        private bool IsConnectedToInternet()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        [RelayCommand]
        public async Task TapNote(Note_Realtime note)
        {
            if (note != null)
            {
                // Điều hướng đến NotePage với ghi chú đã chọn dựa trên ID
                await Shell.Current.GoToAsync(nameof(NotePage), new Dictionary<string, object>
                {
                    { "SelectedNote", note }
                });
            }
        }

        [RelayCommand]
        private async Task SelectNote()
        {
            if (SelectedNote != null)
            {
                // Điều hướng đến trang NotePage với SelectedNote
                await Shell.Current.GoToAsync(nameof(NotePage), new Dictionary<string, object>
                {
                    { "SelectedNote", SelectedNote }
                });
            }
        }
        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName)
        {
            for (int attempt = 0; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    if (attempt > 0)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(2, attempt - 1);
                        Debug.WriteLine($"Retrying {operationName} - Attempt {attempt} of {MaxRetries} after {delayMs}ms delay");
                        await Task.Delay(delayMs);
                    }

                    return await operation();
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Network error during {operationName}: {ex.Message}");
                    if (attempt == MaxRetries) throw;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during {operationName}: {ex.Message}");
                    throw;
                }
            }

            throw new Exception($"Failed to execute {operationName} after {MaxRetries} attempts");
        }

        public async Task SyncDataFromFirebaseAsync()
        {
            if (!IsConnectedToInternet())
            {
                StatusMessage = "No internet connection available";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Synchronizing with cloud...";

                // Sync notes
                await SyncNotesFromFirebaseAsync();

                // Sync drawings
                await SyncDrawingsFromFirebaseAsync();

                StatusMessage = "Sync completed successfully";
                Debug.WriteLine("Firebase sync completed successfully");
            }
            catch (Exception ex)
            {
                StatusMessage = "Sync failed. Please try again later";
                Debug.WriteLine($"Firebase sync failed: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Sync Error",
                    "Failed to sync with cloud. Some changes may not be saved.",
                    "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }


        private async Task SyncNotesFromFirebaseAsync()
        {
            using var client = new HttpClient();

            // Get notes keys with retry
            var keys = await ExecuteWithRetryAsync(async () =>
            {
                var keysUrl = "https://notationapp-98854-default-rtdb.firebaseio.com/notes.json?shallow=true";
                var response = await client.GetAsync(keysUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            }, "GetNotesKeys");

            if (keys == null || !keys.Any()) return;

            // Process each note
            foreach (var key in keys.Keys)
            {
                try
                {
                    var note = await ExecuteWithRetryAsync(async () =>
                    {
                        var noteUrl = $"https://notationapp-98854-default-rtdb.firebaseio.com/notes/{key}.json";
                        var response = await client.GetAsync(noteUrl);
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadFromJsonAsync<Note_Realtime>();
                    }, $"GetNote_{key}");

                    if (note != null)
                    {
                        await SyncNoteToLocalDatabase(note);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to sync note {key}: {ex.Message}");
                    // Continue with next note instead of failing entire sync
                    continue;
                }
            }
        }

        private async Task SyncDrawingsFromFirebaseAsync()
        {
            using var client = new HttpClient();

            // Get drawings keys with retry
            var keys = await ExecuteWithRetryAsync(async () =>
            {
                var keysUrl = "https://notationapp-98854-default-rtdb.firebaseio.com/drawings.json?shallow=true";
                var response = await client.GetAsync(keysUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            }, "GetDrawingsKeys");

            if (keys == null || !keys.Any()) return;

            // Process each drawing
            foreach (var key in keys.Keys)
            {
                try
                {
                    var drawing = await ExecuteWithRetryAsync(async () =>
                    {
                        var drawingUrl = $"https://notationapp-98854-default-rtdb.firebaseio.com/drawings/{key}.json";
                        var response = await client.GetAsync(drawingUrl);
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadFromJsonAsync<Drawing>();
                    }, $"GetDrawing_{key}");

                    if (drawing != null)
                    {
                        await SyncDrawingToLocalDatabase(drawing);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to sync drawing {key}: {ex.Message}");
                    // Continue with next drawing instead of failing entire sync
                    continue;
                }
            }
        }


        // Hàm đồng bộ một ghi chú từ Firebase vào cơ sở dữ liệu cục bộ
        public async Task<int> SyncNoteToLocalDatabase(Note_Realtime firebaseNote)
        {
            var localNote = await _database.GetNoteByIdAsync(firebaseNote.Id);

            if (localNote != null)
            {
                // Cập nhật nếu dữ liệu Firebase mới hơn
                if (firebaseNote.UpdateDate > localNote.UpdateDate)
                {
                    await _database.SaveNoteAsync(firebaseNote);
                    return firebaseNote.Id;
                }
            }
            else
            {
                // Tạo mới nếu chưa tồn tại
                await _database.SaveNoteAsync(firebaseNote);
                return firebaseNote.Id;
            }
            return 0;
        }

        // Hàm đồng bộ một hình vẽ từ Firebase vào cơ sở dữ liệu cục bộ
        private async Task SyncDrawingToLocalDatabase(Drawing drawing)
        {
            var existingDrawing = await _database.GetDrawingByIdAsync(drawing.Id);
            if (existingDrawing == null)
            {
                await _database.SaveDrawingAsync(drawing); // Lưu mới
            }
            else if (existingDrawing.UpdateDate < drawing.UpdateDate)
            {
                await _database.UpdateDrawingAsync(drawing); // Cập nhật nếu dữ liệu từ Firebase mới hơn
            }
        }


        #endregion

        [RelayCommand]
        private async Task FilterByTag(string tag)
        {
            try
            {
                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "User not logged in";
                    return;
                }

                SelectedTag = tag ?? "All";
                var allNotes = await _database.GetNotesAsync();

                var filteredNotes = tag switch
                {
                    "All" => allNotes.Where(n => !n.IsDeleted && n.OwnerId == userId),
                    "Pinned" => allNotes.Where(n => n.IsPinned && !n.IsDeleted && n.OwnerId == userId),
                    "Shared" => allNotes.Where(n => n.IsShared && !n.IsDeleted && (JsonConvert.DeserializeObject<string[]>(n.SharedWithUsers)?.Contains(userId) ?? false)),
                    "Work" => allNotes.Where(n => n.TagName == "Work" && !n.IsDeleted && n.OwnerId == userId),
                    "Personal" => allNotes.Where(n => n.TagName == "Personal" && !n.IsDeleted && n.OwnerId == userId),
                    "Todo" => allNotes.Where(n => n.TagName == "Todo" && !n.IsDeleted && n.OwnerId == userId),
                    "Study" => allNotes.Where(n => n.TagName == "Study" && !n.IsDeleted && n.OwnerId == userId),
                    "Other" => allNotes.Where(n => n.TagName == "Other" && !n.IsDeleted && n.OwnerId == userId),
                    _ => allNotes.Where(n => !n.IsDeleted && n.OwnerId == userId)
                };

                Notes.Clear();
                foreach (var note in filteredNotes)
                {
                    Notes.Add(note);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterByTag error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to filter notes", "OK");
            }
        }
        [RelayCommand]
        private async Task ShareItemAsync(object item)
        {
            if (item == null) return;

            try
            {
                IsLoading = true;

                var shareDialog = new ShareDialog(item, _firestoreService, _database);
                await Application.Current.MainPage.ShowPopupAsync(shareDialog);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error sharing item: {ex.Message}";
                Debug.WriteLine($"Error in ShareItemAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateSharePermissionsAsync(object item)
        {
            if (item == null) return;

            try
            {
                IsLoading = true;
                if (item is Note_Realtime note)
                {
                    // Toggle between different permission levels
                    note.Permission = note.Permission switch
                    {
                        Note_Realtime.SharePermission.ReadOnly => Note_Realtime.SharePermission.ReadWrite,
                        Note_Realtime.SharePermission.ReadWrite => Note_Realtime.SharePermission.Full,
                        Note_Realtime.SharePermission.Full => Note_Realtime.SharePermission.ReadOnly,
                        _ => Note_Realtime.SharePermission.ReadOnly
                    };
                    await _database.UpdateNoteAsync(note);
                }
                else if (item is Drawing drawing)
                {
                    drawing.Permission = drawing.Permission switch
                    {
                        Drawing.SharePermission.ReadOnly => Drawing.SharePermission.ReadWrite,
                        Drawing.SharePermission.ReadWrite => Drawing.SharePermission.Full,
                        Drawing.SharePermission.Full => Drawing.SharePermission.ReadOnly,
                        _ => Drawing.SharePermission.ReadOnly
                    };
                    await _database.UpdateDrawingAsync(drawing);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating share permissions: {ex.Message}";
                Debug.WriteLine($"Error in UpdateSharePermissionsAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task StopSharingAsync(object item)
        {
            if (item == null) return;

            try
            {
                IsLoading = true;
                if (item is Note_Realtime note)
                {
                    note.IsShared = false;
                    note.SharedWithUsers = JsonConvert.SerializeObject(Array.Empty<string>());
                    note.ShareLink = string.Empty;
                    await _database.UpdateNoteAsync(note);
                }
                else if (item is Drawing drawing)
                {
                    drawing.IsShared = false;
                    drawing.SharedWithUsers = JsonConvert.SerializeObject(Array.Empty<string>());
                    drawing.ShareLink = string.Empty;
                    await _database.UpdateDrawingAsync(drawing);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error stopping sharing: {ex.Message}";
                Debug.WriteLine($"Error in StopSharingAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterItems();
        }

        private void FilterItems()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchQuery))
                {
                    // Reset về danh sách gốc
                    Task.WhenAll(
                        LoadNotesAsync(),
                        LoadDrawingsAsync()
                    );
                    return;
                }

                var query = SearchQuery.ToLower();

                // Lọc ghi chú
                var filteredNotes = Notes.Where(n =>
                    (n.Title?.ToLower().Contains(query) ?? false) ||
                    (n.PlainText?.ToLower().Contains(query) ?? false))
                    .ToList();
                Notes.Clear();
                foreach (var note in filteredNotes)
                {
                    Notes.Add(note);
                }

                // Lọc hình vẽ
                var filteredDrawings = Drawings.Where(d =>
                    d.Title?.ToLower().Contains(query) ?? false)
                    .ToList();
                Drawings.Clear();
                foreach (var drawing in filteredDrawings)
                {
                    Drawings.Add(drawing);
                }

                // Lọc ghi chú được chia sẻ
                if (SharedWithMeNotes != null)
                {
                    var filteredSharedNotes = SharedWithMeNotes.Where(n =>
                        (n.Title?.ToLower().Contains(query) ?? false) ||
                        (n.PlainText?.ToLower().Contains(query) ?? false))
                        .ToList();
                    SharedWithMeNotes.Clear();
                    foreach (var note in filteredSharedNotes)
                    {
                        SharedWithMeNotes.Add(note);
                    }
                }

                // Lọc hình vẽ được chia sẻ
                if (SharedWithMeDrawings != null)
                {
                    var filteredSharedDrawings = SharedWithMeDrawings.Where(d =>
                        d.Title?.ToLower().Contains(query) ?? false)
                        .ToList();
                    SharedWithMeDrawings.Clear();
                    foreach (var drawing in filteredSharedDrawings)
                    {
                        SharedWithMeDrawings.Add(drawing);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in FilterItems: {ex.Message}");
                StatusMessage = "Error filtering items";
            }
        }
    }
}

