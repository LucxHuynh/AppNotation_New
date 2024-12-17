using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotationApp.Models;
using NotationApp.Database;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NotationApp.ViewModels
{
    public partial class NotePageViewModel : ObservableObject
    {
        private readonly NoteDatabase _database;
        private readonly Note_Realtime _note;
        private readonly string _currentUserId;
        private readonly string _currentUserEmail;

        public ObservableCollection<Drawing> Drawings { get; private set; } = new();
        public ObservableCollection<Drawing> SharedWithMeDrawings { get; private set; } = new();

        [ObservableProperty] private Drawing selectedDrawing;

        [ObservableProperty] private bool isEditable = true; // Mặc định cho phép edit

        public NotePageViewModel(Note_Realtime note)
        {
            _database = App.Database;
            _note = note ?? new Note_Realtime();
            Title = _note.Title;
            Text = _note.Text;
            _currentUserId = Preferences.Get("UserId", string.Empty);
            _currentUserEmail = Preferences.Get("UserEmail", string.Empty);
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string text;

        // NotePageViewModel.cs
        [RelayCommand]
        public async Task SaveNoteAsync(string htmlContent)
        {
            try
            {
                _note.Title = Title;
                _note.Text = htmlContent;
                _note.UpdateDate = DateTime.Now;

                // Lưu local
                await _database.SaveNoteAsync(_note);

                // Sync Firebase
                await SyncDataToFirebase(_note);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving note: {ex.Message}");
                throw;
            }
        }

        [RelayCommand]
        public async Task CreateDrawingAsync()
        {
            try
            {
                var newDraw = new Drawing
                {
                    Title = "New Drawing",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    OwnerId = _currentUserId ?? "default_user",
                    OwnerEmail = _currentUserEmail ?? "default_user",
                };

                Debug.WriteLine($"New drawing created with initial values: Title={newDraw.Title}, OwnerId={newDraw.OwnerId}");
                Debug.WriteLine("Saving drawing to database...");

                var result = await _database.SaveDrawingAsync(newDraw);
                Debug.WriteLine($"Save result: {result}");

                if (result > 0)
                {
                    Debug.WriteLine($"Drawing saved successfully with ID: {newDraw.Id}");
                    Drawings.Add(newDraw);

                    try
                    {
                        Debug.WriteLine("Attempting to navigate to NotePage...");
                        await Shell.Current.GoToAsync(nameof(Pages.DrawingPage),
                            new Dictionary<string, object> { { "SelectedDrawing", newDraw } });
                    }
                    catch (Exception navEx)
                    {
                        Debug.WriteLine($"Navigation error: {navEx.Message}");
                        throw;
                    }
                }
                else
                {
                    Debug.WriteLine("Failed to save drawing");
                    throw new Exception("Failed to save drawing");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CreateDrawingAsync: {ex.Message}");
                throw;
            }
        }

        // Hàm kiểm tra kết nối Internet
        private bool IsConnectedToInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        // Hàm đồng bộ dữ liệu lên Firebase
        private async Task SyncDataToFirebase(Note_Realtime note)
        {
            // Chỉ đồng bộ nếu có kết nối Internet
            if (!IsConnectedToInternet())
            {
                return;
            }

            // Sử dụng Id của ghi chú để xác định URL cụ thể cho ghi chú đó trên Firebase
            string firebaseUrl = $"https://notationapp-98854-default-rtdb.firebaseio.com/notes/{note.Id}.json";

            using (HttpClient client = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(note);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(firebaseUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    note.IsSynced = true;
                    // Thay vì UpdateNoteAsync, dùng SaveNoteAsync
                    await _database.SaveNoteAsync(note);
                    Console.WriteLine($"Đồng bộ thành công ghi chú với ID: {note.Id}");
                }
                else
                {
                    Console.WriteLine("Lỗi đồng bộ: " + response.ReasonPhrase);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteNoteAsync()
        {
            if (_note != null)
            {
                _note.IsDeleted = true; // Đánh dấu ghi chú là đã xóa
                await _database.UpdateNoteAsync(_note); // Cập nhật ghi chú trong cơ sở dữ liệu
                // Đồng bộ lên Firebase nếu có kết nối Internet
                await SyncDataToFirebase(_note);
                Console.WriteLine("Đã chuyển ghi chú vào thùng rác với ID: " + _note.Id);
            }

        }
        [RelayCommand]
        public async Task TogglePinAsync()
        {
            try
            {
                _note.IsPinned = !_note.IsPinned; // Toggle pin state
                _note.UpdateDate = DateTime.Now;
                await _database.SaveNoteAsync(_note);
                await SyncDataToFirebase(_note);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling pin: {ex.Message}");
                throw;
            }
        }
        [RelayCommand]
        public async Task UpdateTagAsync(string selectedTag)
        {
            try
            {
                _note.TagName = selectedTag;
                _note.UpdateDate = DateTime.Now;
                await _database.SaveNoteAsync(_note);
                await SyncDataToFirebase(_note);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tag: {ex.Message}");
                throw;
            }
        }
    }
}
