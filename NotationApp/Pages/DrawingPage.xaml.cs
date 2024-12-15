using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls;
using NotationApp.Models;
using NotationApp.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NotationApp.Pages
{
    [QueryProperty(nameof(SelectedDrawing), "SelectedDrawing")]
    public partial class DrawingPage : ContentPage
    {
        private Drawing _selectedDrawing;

        public Drawing SelectedDrawing
        {
            get => _selectedDrawing;
            set
            {
                _selectedDrawing = value;
                BindingContext = new DrawingPageViewModel(_selectedDrawing);
                LoadInitialContent();
            }
        }

        public DrawingPage()
        {
            InitializeComponent();
            SetDrawingWebViewSource();
        }

        private async void OnDotsButtonClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Chọn tùy chọn", "Hủy", null,
                "Thêm vào sổ tay được chia sẻ",
                "Tag",
                "Ghim",
                "Bỏ ghim",
                "Chia sẻ",
                "Xóa");

            // Thực hiện hành động dựa trên tùy chọn người dùng chọn
            var viewModel = BindingContext as DrawingPageViewModel;
            if (viewModel == null) return;
            switch (action)
            {
                case "Thêm vào sổ tay được chia sẻ":
                    // Thực hiện hành động thêm vào sổ tay
                    break;
                case "Tag":
                    var tags = new string[] { "Work", "Personal", "Todo", "Study", "Other" };
                    var selectedTag = await DisplayActionSheet("Chọn thẻ", "Hủy", null, tags);
                    if (selectedTag != "Hủy" && selectedTag != null)
                    {
                        await viewModel.UpdateTagAsync(selectedTag);
                        await DisplayAlert("Thành công", $"Đã gán thẻ '{selectedTag}'", "OK");
                    }
                    break;

                case "Ghim":
                    if (!_selectedDrawing.IsPinned)
                    {
                        await viewModel.TogglePinAsync();
                        await DisplayAlert("Thành công", "Đã ghim hình vẽ.", "OK");
                    }
                    break;
                case "Bỏ ghim":
                    if (_selectedDrawing.IsPinned)
                    {
                        await viewModel.TogglePinAsync();
                        await DisplayAlert("Thành công", "Đã bỏ ghim hình vẽ.", "OK");
                    }
                    break;
                case "Chia sẻ":
                    // Thực hiện hành động chia sẻ
                    break;
                case "Xóa":
                    // Thực hiện hành động xóa
                    bool confirmDelete = await DisplayAlert("Xóa hình vẽ", "Bạn có chắc chắn muốn chuyển hình vẽ này vào thùng rác?", "Xóa", "Hủy");
                    if (confirmDelete)
                    {
                        var viewModel1 = BindingContext as DrawingPageViewModel;
                        if (viewModel1 != null)
                        {
                            await viewModel1.DeleteDrawingAsync(); // Gọi hàm xóa từ ViewModel
                            await DisplayAlert("Đã xóa", "Hình vẽ đã được chuyển vào thùng rác.", "OK");

                            // Quay lại trang trước đó sau khi xóa
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    break;
            }
        }

        private void SetDrawingWebViewSource()
        {
#if ANDROID
            DrawingWebView.Source = "file:///android_asset/drawing_editor.html";
#elif WINDOWS
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "drawing_editor.html");
            if (!File.Exists(filePath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("drawing_editor.html").Result;
                using var reader = new StreamReader(stream);
                File.WriteAllText(filePath, reader.ReadToEnd());
            }
            DrawingWebView.Source = filePath;
#endif
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000); // Đợi WebView tải xong
            LoadInitialContent();
        }

        private void LoadInitialContent()
        {
            var viewModel = BindingContext as DrawingPageViewModel;
            if (viewModel != null && !string.IsNullOrEmpty(viewModel.ImagePath))
            {
                string content = viewModel.ImagePath;
                // Escape JSON và gọi hàm JavaScript
                DrawingWebView.Eval($"setEditorContent('{EscapeJavaScriptString(content)}');");

            }
        }


        private string EscapeJavaScriptString(string content)
        {
            return content
                .Replace("\\", "\\\\") // Escape ký tự backslash
                .Replace("'", "\\'")   // Escape ký tự đơn
                .Replace("\n", "\\n")  // Escape ký tự xuống dòng
                .Replace("\r", "\\r")  // Escape ký tự xuống dòng kiểu cũ
                .Replace("\"", "\\\""); // Escape dấu ngoặc kép
        }



        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as DrawingPageViewModel;
            if (viewModel != null)
            {
                string drawingContent = await GetDrawingContentFromWebViewAsync();
                viewModel.ImagePath = drawingContent;
                await viewModel.SaveDrawingAsync(drawingContent);
                await DisplayAlert("Thành công", "Hình vẽ đã được lưu.", "OK");
            }
        }


        private async Task<string> GetDrawingContentFromWebViewAsync()
        {
            try
            {
                var content = await DrawingWebView.EvaluateJavaScriptAsync("getEditorContent()");
                return content ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy nội dung canvas: {ex.Message}");
                return string.Empty;
            }
        }
        //public async Task SaveImageToFile(string base64Data)
        //{
        //    try
        //    {
        //        var fileName = $"drawing_{DateTime.Now:yyyyMMddHHmmss}.png";
        //        var bytes = Convert.FromBase64String(base64Data.Split(',')[1]);
        //        await FileSaver.Default.SaveAsync(fileName, new MemoryStream(bytes), CancellationToken.None);
        //        await DisplayAlert("Thành công", "Đã lưu hình vẽ", "OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Lỗi", $"Không thể lưu: {ex.Message}", "OK");
        //    }
        //}

    }
}
