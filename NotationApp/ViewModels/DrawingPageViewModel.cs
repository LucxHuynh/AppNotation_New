using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotationApp.Models;
using NotationApp.Database;
using Newtonsoft.Json;
using System.Text;

namespace NotationApp.ViewModels
{
    public partial class DrawingPageViewModel : ObservableObject
    {
        private readonly NoteDatabase _database;
        private readonly Drawing _drawing;

        public DrawingPageViewModel(Drawing drawing)
        {
            _database = App.Database;
            _drawing = drawing ?? new Drawing();
            Title = _drawing.Title;
            ImagePath = _drawing.ImagePath;
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string imagePath;

        [RelayCommand]
        public async Task SaveDrawingAsync(string imagePathContent)
        {
            try
            {
                _drawing.Title = Title;
                _drawing.ImagePath = imagePathContent;
                _drawing.UpdateDate = DateTime.Now;

                if (_drawing.Id == 0)
                {
                    var drawings = await _database.GetDrawingsAsync();
                    var maxId = drawings.Any() ? drawings.Max(d => d.Id) : 0;
                    _drawing.Id = maxId + 1;
                }

                await _database.SaveDrawingAsync(_drawing);
                await SyncDataToFirebase(_drawing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving drawing: {ex.Message}");
                throw;
            }
        }

        private bool IsConnectedToInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        private async Task SyncDataToFirebase(Drawing drawing)
        {
            if (!IsConnectedToInternet()) return;

            string firebaseUrl = $"https://notationapp-98854-default-rtdb.firebaseio.com/drawings/{drawing.Id}.json";

            using (HttpClient client = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(drawing);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    drawing.IsSynced = true;
                    await _database.UpdateDrawingAsync(drawing);
                    Console.WriteLine("Đồng bộ thành công hình vẽ với ID: " + drawing.Id);
                }
                else
                {
                    Console.WriteLine("Lỗi đồng bộ: " + response.ReasonPhrase);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteDrawingAsync()
        {
            if (_drawing != null)
            {
                _drawing.IsDeleted = true;
                await _database.UpdateDrawingAsync(_drawing);
                await SyncDataToFirebase(_drawing);
                Console.WriteLine("Đã chuyển hình vẽ vào thùng rác với ID: " + _drawing.Id);
            }
        }
        [RelayCommand]
        public async Task TogglePinAsync()
        {
            try
            {
                _drawing.IsPinned = !_drawing.IsPinned;
                _drawing.UpdateDate = DateTime.Now;
                await _database.SaveDrawingAsync(_drawing);
                await SyncDataToFirebase(_drawing);
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
                _drawing.TagName = selectedTag;
                _drawing.UpdateDate = DateTime.Now;
                await _database.SaveDrawingAsync(_drawing);
                await SyncDataToFirebase(_drawing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tag: {ex.Message}");
                throw;
            }
        }
    }
}
