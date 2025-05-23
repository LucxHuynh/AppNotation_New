﻿using System.Collections.ObjectModel;
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
using System.Linq;

namespace NotationApp.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly NoteDatabase _database;
        private readonly IFirestoreService _firestoreService;
        private readonly string _currentUserId;
        private readonly string _currentUserEmail;
        private const int MaxRetries = 3;
        private const int InitialDelayMs = 1000;

        public ObservableCollection<Note_Realtime> Notes { get; private set; } = new();
        public ObservableCollection<Note_Realtime> SharedWithMeNotes { get; private set; } = new();
        public ObservableCollection<Drawing> Drawings { get; private set; } = new();
        public ObservableCollection<Drawing> SharedWithMeDrawings { get; private set; } = new();

        public ObservableCollection<HomeItem> Items { get; } = new();
        public string SelectedTag { get; private set; } = "ALL";

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
            _currentUserEmail = Preferences.Get("UserEmail", string.Empty);
        }


        #region Note Loading and Management

        [RelayCommand]
        public async Task LoadAllContentAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải dữ liệu...";

                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "Người dùng chưa đăng nhập";
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

        public async Task LoadNotesAsync()
        {
            try
            {
                IsLoading = true;
                Notes.Clear();
                SharedWithMeNotes.Clear();

                var userId = Preferences.Get("UserId", string.Empty);
                var userEmail = Preferences.Get("UserEmail", string.Empty);

                if (string.IsNullOrEmpty(userId))
                {
                    StatusMessage = "Người dùng chưa đăng nhập";
                    return;
                }

                // Load owned notes - bao gồm cả notes chưa share
                var ownedNotes = await _database.GetOwnedNotesAsync(userId);
                foreach (var note in ownedNotes.Where(n => !n.IsDeleted))
                {
                    Notes.Add(note);
                }

                // Load shared notes - chỉ load những note được share với user
                var sharedNotes = await _database.GetSharedNotesAsync(userEmail);
                foreach (var note in sharedNotes.Where(n => !n.IsDeleted && n.OwnerId != userId && n.IsSharedWithUser(userEmail)))
                {
                    SharedWithMeNotes.Add(note);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading notes: {ex.Message}";
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
                    StatusMessage = "Người dùng chưa đăng nhập";
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
                Debug.WriteLine("Đang tạo ghi chú...");

                var newNote = new Note_Realtime
                {
                    Title = "Ghi chú mới",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    OwnerId = _currentUserId ?? "default_user", // Đảm bảo OwnerId không null
                    OwnerEmail = _currentUserEmail ?? "default_user",
                };

                Debug.WriteLine($"New note created with initial values: Title={newNote.Title}, OwnerId={newNote.OwnerId}, OwnerEmail={newNote.OwnerEmail}");

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
                    StatusMessage = "Lỗi trong quá trình tạo ghi chú.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddNote: {ex}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                StatusMessage = $"Lỗi: {ex.Message}";
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
                StatusMessage = "Không truy cập được internet";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Đồng bộ hóa với đám mây...";

                // Sync notes
                await SyncNotesFromFirebaseAsync();

                // Sync drawings
                await SyncDrawingsFromFirebaseAsync();

                StatusMessage = "Đồng bộ hoàn tất";
                Debug.WriteLine("Firebase sync completed successfully");
            }
            catch (Exception ex)
            {
                StatusMessage = "Sync failed. Please try again later";
                Debug.WriteLine($"Firebase sync failed: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Đồng bộ thất bại",
                    "Không thể đồng bộ hóa với đám mây. Một số thay đổi có thể không được lưu.",
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
                var keysUrl = "https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/notes.json?shallow=true";
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
                        var noteUrl = $"https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/notes/{key}.json";
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
                var keysUrl = "https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/drawings.json?shallow=true";
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
                        var drawingUrl = $"https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/drawings/{key}.json";
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
                // Also sync if Firebase note is deleted while local isn't
                if (firebaseNote.UpdateDate > localNote.UpdateDate ||
                    (firebaseNote.IsDeleted && !localNote.IsDeleted))
                {
                    await _database.SaveNoteAsync(firebaseNote);
                    return firebaseNote.Id;
                }
            }
            else
            {
                await _database.SaveNoteAsync(firebaseNote);
                return firebaseNote.Id;
            }
            return 0;
        }

        // Hàm đồng bộ một hình vẽ từ Firebase vào cơ sở dữ liệu cục bộ
        public async Task<int> SyncDrawingToLocalDatabase(Drawing drawing)
        {
            var localDraw = await _database.GetDrawingByIdAsync(drawing.Id);

            if (localDraw != null)
            {
                // Also sync if Firebase note is deleted while local isn't
                if (drawing.UpdateDate > localDraw.UpdateDate ||
                    (drawing.IsDeleted && !localDraw.IsDeleted))
                {
                    await _database.SaveDrawingAsync(drawing);
                    return drawing.Id;
                }
            }
            else
            {
                await _database.SaveDrawingAsync(drawing);
                return drawing.Id;
            }
            return 0;
        }


        #endregion

        [RelayCommand]
        private async Task FilterByTag(string tag)
        {
            try
            {
                var userId = Preferences.Get("UserId", string.Empty);
                var userEmail = Preferences.Get("UserEmail", string.Empty);

                Debug.WriteLine($"FilterByTag called with tag: {tag}");
                Debug.WriteLine($"UserId: {userId}");
                Debug.WriteLine($"UserEmail: {userEmail}");

                // Load data
                var ownedNotes = await _database.GetOwnedNotesAsync(userId);
                Debug.WriteLine($"Loaded {ownedNotes.Count} owned notes");

                var ownedDrawings = await _database.GetOwnedDrawingsAsync(userId);
                Debug.WriteLine($"Loaded {ownedDrawings.Count} owned drawings");

                // Kiểm tra dữ liệu mẫu
                foreach (var note in ownedNotes)
                {
                    Debug.WriteLine($"Note: Id={note.Id}, Title={note.Title}, OwnerId={note.OwnerId}, IsDeleted={note.IsDeleted}");
                }

                foreach (var drawing in ownedDrawings)
                {
                    Debug.WriteLine($"Drawing: Id={drawing.Id}, Title={drawing.Title}, OwnerId={drawing.OwnerId}, IsDeleted={drawing.IsDeleted}");
                }

                var sharedNotes = await _database.GetSharedNotesAsync(userEmail);
                var sharedDrawings = await _database.GetSharedDrawingsAsync(userEmail);

                var filteredNotes = tag switch
                {
                    "ALL" => ownedNotes.Where(n => !n.IsDeleted),
                    "Pinned" => ownedNotes.Where(n => !n.IsDeleted && n.IsPinned),
                    "Shared" => sharedNotes.Where(n => !n.IsDeleted && n.IsSharedWithUser(userEmail)),
                    _ => ownedNotes.Where(n => !n.IsDeleted && n.TagName == tag),
                };

                var filteredDrawings = tag switch
                {
                    "ALL" => ownedDrawings.Where(d => !d.IsDeleted),
                    "Pinned" => ownedDrawings.Where(d => !d.IsDeleted && d.IsPinned),
                    "Shared" => ownedDrawings.Where(d => !d.IsDeleted && d.IsShared)
                            .Concat(sharedDrawings.Where(d => !d.IsDeleted && d.IsSharedWithUser(userEmail))),
                    _ => ownedDrawings.Where(d => !d.IsDeleted && d.TagName == tag),
                };

                Debug.WriteLine($"Filtered notes count: {filteredNotes.Count()}");
                Debug.WriteLine($"Filtered drawings count: {filteredDrawings.Count()}");

                // Convert to HomeItems
                var homeItems = filteredNotes.Select(n => new HomeItem
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.PlainText,
                    UpdateDate = n.UpdateDate,
                    TagName = n.TagName,
                    IsPinned = n.IsPinned,
                    IsShared = n.IsShared,
                    ItemType = "Ghi chú",
                    OriginalItem = n
                })
                .Concat(filteredDrawings.Select(d => new HomeItem
                {
                    Id = d.Id,
                    Title = d.Title,
                    UpdateDate = d.UpdateDate,
                    TagName = d.TagName,
                    IsPinned = d.IsPinned,
                    IsShared = d.IsShared,
                    ItemType = "Hình vẽ",
                    OriginalItem = d
                }))
                .OrderByDescending(i => i.UpdateDate);

                var homeItemsList = homeItems.ToList(); // Chuyển sang list để đếm
                Debug.WriteLine($"Total home items after conversion: {homeItemsList.Count}");

                Items.Clear();
                foreach (var item in homeItemsList)
                {
                    Items.Add(item);
                    Debug.WriteLine($"Added item: ID={item.Id}, Type={item.ItemType}, Title={item.Title}");
                }

                Debug.WriteLine($"Final Items collection count: {Items.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in FilterByTag: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await Shell.Current.DisplayAlert("Lỗi", "Không thể lọc các mục", "OK");
            }
        }

        [RelayCommand]
        private async Task TapItem(HomeItem item)
        {
            var navigationParameter = new Dictionary<string, object>();
            if (item.ItemType == "Ghi chú")
            {
                navigationParameter.Add("SelectedNote", item.OriginalItem);
                await Shell.Current.GoToAsync("NotePage", navigationParameter);
            }
            else if (item.ItemType == "Hình vẽ")
            {
                navigationParameter.Add("SelectedDrawing", item.OriginalItem);
                await Shell.Current.GoToAsync("DrawingPage", navigationParameter);
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

        //[RelayCommand]
        //private async Task UpdateSharePermissionsAsync(object item)
        //{
        //    if (item == null) return;

        //    try
        //    {
        //        IsLoading = true;
        //        if (item is Note_Realtime note)
        //        {
        //            // Toggle between different permission levels
        //            note.Permission = note.Permission switch
        //            {
        //                Note_Realtime.SharePermission.ReadOnly => Note_Realtime.SharePermission.ReadWrite,
        //                Note_Realtime.SharePermission.ReadWrite => Note_Realtime.SharePermission.Full,
        //                Note_Realtime.SharePermission.Full => Note_Realtime.SharePermission.ReadOnly,
        //                _ => Note_Realtime.SharePermission.ReadOnly
        //            };
        //            await _database.UpdateNoteAsync(note);
        //        }
        //        else if (item is Drawing drawing)
        //        {
        //            drawing.Permission = drawing.Permission switch
        //            {
        //                Drawing.SharePermission.ReadOnly => Drawing.SharePermission.ReadWrite,
        //                Drawing.SharePermission.ReadWrite => Drawing.SharePermission.Full,
        //                Drawing.SharePermission.Full => Drawing.SharePermission.ReadOnly,
        //                _ => Drawing.SharePermission.ReadOnly
        //            };
        //            await _database.UpdateDrawingAsync(drawing);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = $"Error updating share permissions: {ex.Message}";
        //        Debug.WriteLine($"Error in UpdateSharePermissionsAsync: {ex}");
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}

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

