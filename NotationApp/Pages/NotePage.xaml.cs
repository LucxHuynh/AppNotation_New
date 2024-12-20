using NotationApp.Models;
using NotationApp.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Maui.Audio;
using Newtonsoft.Json;
using System.Diagnostics;
using NotationApp.Database;
using NotationApp.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using System.Xml.Linq;
using System.Windows.Input;

namespace NotationApp.Pages
{
    [QueryProperty(nameof(SelectedNote), "SelectedNote")]
    public partial class NotePage : ContentPage
    {
        private Note_Realtime _selectedNote;
        private readonly NoteDatabase _database;
        private readonly IFirestoreService _firestoreService;
        private readonly string _currentUserId;

        public ObservableCollection<Drawing> Drawings { get; private set; } = new();
        public ObservableCollection<Drawing> SharedWithMeDrawings { get; private set; } = new();
        public Note_Realtime SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                BindingContext = new NotePageViewModel(_selectedNote); // Gán dữ liệu vào ViewModel
                LoadInitialContent(); // Load content into editor when note is set
            }
        }

        public NotePage()
        {
            InitializeComponent();
            SetEditorWebViewSource();
            //SetDrawingWebViewSource(); 
            audioManager = new AudioManager();
        }
        private IAudioManager audioManager;
        private IAudioRecorder audioRecorder;
        private bool isRecording = false;
        private string lastRecordedFilePath;

#if WINDOWS
        private NAudio.Wave.WaveInEvent waveSource;
        private NAudio.Wave.WaveFileWriter waveFile;
#endif

        private async void OnRecordButtonClicked(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                await StartRecording();
            }
            else
            {
                await StopRecording();
            }
        }

        private async Task StartRecording()
        {
            try
            {
                var permissions = await CheckAndRequestPermissions();
                if (!permissions) return;

#if WINDOWS
                var tempDir = Path.Combine(Path.GetTempPath(), "NotationAppTemp");
                Directory.CreateDirectory(tempDir);
                lastRecordedFilePath = Path.Combine(tempDir, $"recording_{DateTime.Now:yyyyMMddHHmmss}.wav");

                waveSource = new NAudio.Wave.WaveInEvent();
                waveSource.WaveFormat = new NAudio.Wave.WaveFormat(44100, 1);
                waveFile = new NAudio.Wave.WaveFileWriter(lastRecordedFilePath, waveSource.WaveFormat);

                waveSource.DataAvailable += (s, e) =>
                {
                    waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                };

                waveSource.StartRecording();
#else
                audioRecorder = audioManager.CreateRecorder();
                var cacheDir = FileSystem.CacheDirectory;
                lastRecordedFilePath = Path.Combine(cacheDir, $"recording_{DateTime.Now:yyyyMMddHHmmss}.wav");
                await audioRecorder.StartAsync(lastRecordedFilePath);
#endif
                isRecording = true;
                RecordButton.BackgroundColor = Colors.Red;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to start recording: {ex.Message}", "OK");
            }
        }

        private async Task StopRecording()
        {
            try
            {
#if WINDOWS
                if (waveSource != null)
                {
                    waveSource.StopRecording();
                    waveSource.Dispose();
                    waveSource = null;

                    if (waveFile != null)
                    {
                        waveFile.Close();
                        waveFile.Dispose();
                        waveFile = null;
                    }
                }
#else
                if (audioRecorder == null) return;
                await audioRecorder.StopAsync();
#endif

                if (File.Exists(lastRecordedFilePath))
                {
                    byte[] audioBytes = File.ReadAllBytes(lastRecordedFilePath);
                    string base64Audio = Convert.ToBase64String(audioBytes);
                    string audioUrl = $"data:audio/wav;base64,{base64Audio}";

                    // Thêm delay ngắn để đảm bảo file không bị lock
                    await Task.Delay(100);

                    var js = $"const range = quill.getSelection() || {{ index: quill.getLength() }};" +
             $"quill.insertEmbed(range.index, 'audio', '{audioUrl}');" +
             $"quill.setSelection(quill.getLength(), 0);";

                    await EditorWebView.EvaluateJavaScriptAsync(js);

                    // Thêm delay và kiểm tra file trước khi xóa
                    await Task.Delay(100);
                    if (File.Exists(lastRecordedFilePath))
                    {
                        File.Delete(lastRecordedFilePath);
                    }
                }

                isRecording = false;
                RecordButton.BackgroundColor = Colors.Black;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to stop recording: {ex.Message}", "OK");
            }
        }

        private async Task<bool> CheckAndRequestPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Microphone>();
            }
            return status == PermissionStatus.Granted;
        }
        private async Task<string> GetEditorContentFromWebViewAsync()
        {
            try
            {
                // Gọi hàm JavaScript để lấy nội dung HTML từ Quill Editor
                var editorContent = await EditorWebView.EvaluateJavaScriptAsync("getEditorContent()");
                if (!string.IsNullOrEmpty(editorContent))
                {
                    return editorContent; // Trả về nội dung HTML
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting editor content: {ex.Message}");
            }

            return string.Empty;
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
            switch (action)
            {
                case "Thêm vào sổ tay được chia sẻ":
                    // Thực hiện hành động thêm vào sổ tay
                    break;
                case "Tag":
                    var tags = new string[] { "Công việc", "Cá nhân", "Công việc phải làm", "Học tập", "Khác" };
                    var selectedTag = await DisplayActionSheet("Chọn thẻ", "Hủy", null, tags);

                    if (selectedTag != "Hủy" && selectedTag != null)
                    {
                        var viewModel2 = BindingContext as NotePageViewModel;
                        if (viewModel2 != null)
                        {
                            await viewModel2.UpdateTagAsync(selectedTag);
                            await DisplayAlert("Thành công", $"Đã gán thẻ '{selectedTag}'", "OK");
                        }
                    }
                    break;
                case "Ghim":
                    var viewModel = BindingContext as NotePageViewModel;
                    if (viewModel != null && !_selectedNote.IsPinned)
                    {
                        await viewModel.TogglePinAsync();
                        await DisplayAlert("Thành công", "Đã ghim ghi chú.", "OK");
                    }
                    break;

                case "Bỏ ghim":
                    viewModel = BindingContext as NotePageViewModel;
                    if (viewModel != null && _selectedNote.IsPinned)
                    {
                        await viewModel.TogglePinAsync();
                        await DisplayAlert("Thành công", "Đã bỏ ghim ghi chú.", "OK");
                    }
                    break;
                case "Chia sẻ":
                    // Thực hiện hành động chia sẻ
                    // Hiển thị ShareDialog
                    var firestoreService = IPlatformApplication.Current.Services.GetService<IFirestoreService>()?? new FirestoreService();
                    var shareDialog = new ShareDialog(_selectedNote, firestoreService, App.Database);
                    await Application.Current.MainPage.ShowPopupAsync(shareDialog);
                    break;
                case "Xóa":
                    // Thực hiện hành động xóa
                    // Xác nhận trước khi xóa
                    bool confirmDelete = await DisplayAlert("Xóa ghi chú", "Bạn có chắc chắn muốn chuyển ghi chú này vào thùng rác?", "Xóa", "Hủy");
                    if (confirmDelete)
                    {
                        var viewModel1 = BindingContext as NotePageViewModel;
                        if (viewModel1 != null)
                        {
                            await viewModel1.DeleteNoteAsync(); // Gọi hàm xóa ghi chú từ ViewModel
                            await DisplayAlert("Đã xóa", "Ghi chú đã được chuyển vào thùng rác.", "OK");

                            // Quay lại trang trước đó sau khi xóa
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    break;
            }
        }

        private void SetEditorWebViewSource()
        {
#if ANDROID
            // Trên Android, load file từ `android_asset`
            EditorWebView.Source = "file:///android_asset/editor.html";
#elif WINDOWS
            // Trên Windows, tạo đường dẫn đến file `editor.html` trong AppData
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "editor.html");
            if (!File.Exists(filePath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("editor.html").Result;
                using var reader = new StreamReader(stream);
                File.WriteAllText(filePath, reader.ReadToEnd());
            }
            EditorWebView.Source = filePath;
#endif
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000); // Đợi WebView tải xong

            //var userEmail = Preferences.Get("UserEmail", string.Empty);
            //if (!_selectedNote.CanUserEdit(userEmail))
            //{
            //    // Disable edit controls
            //    EditorWebView.IsEnabled = false;
            //    TitleEntry.IsEnabled = false;

            //    // Disable nút save bằng x:Name
            //    SaveButton.IsEnabled = false; // Thêm x:Name="SaveButton" vào XAML

            //    // Thông báo cho người dùng
            //    await DisplayAlert(
            //        "Read Only",
            //        "You have read-only access to this note",
            //        "OK"
            //    );
            //}

            LoadInitialContent();

        }

        // Hàm để nạp nội dung vào WebView editor

        private void LoadInitialContent()
        {
            var viewModel = BindingContext as NotePageViewModel;
            if (viewModel != null)
            {
                // Thay thế ký tự Unicode escape thành HTML thuần
                string content = viewModel.Text;
                // Gọi hàm JavaScript `setEditorContent` với nội dung đã chuyển đổi
                EditorWebView.Eval($"setEditorContent('{EscapeJavaScriptString(content)}');");
            }
        }

        // Hàm để escape chuỗi JavaScript an toàn khi truyền vào
        private string EscapeJavaScriptString(string content)
        {
            return content.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "");
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as NotePageViewModel;
            if (viewModel != null)
            {
                // Giả sử bạn có phương thức để lấy nội dung từ WebView qua JavaScript
                string htmlContent = await GetEditorContentFromWebViewAsync();
                string content = htmlContent
                    .Replace("\\u003C", "<")
                    .Replace("\\u003E", ">")
                    .Replace("\\u0027", "'")
                    .Replace("\\u0022", "\"");
                // Gán giá trị từ TitleEntry vào thuộc tính Title của ViewModel
                viewModel.Title = TitleEntry.Text;

                // Lưu ghi chú vào cơ sở dữ liệu
                await viewModel.SaveNoteAsync(content);

                // Thông báo lưu thành công cho người dùng (tùy chọn)
                await DisplayAlert("Thành công", "Ghi chú được lưu thành công", "OK");

                //// Quay lại trang trước đó (HomePage) sau khi lưu
                //await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnDrawButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as NotePageViewModel;
            if (viewModel != null)
            {
                try
                {
                    await viewModel.CreateDrawingAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Failed to create drawing", "OK");
                    Debug.WriteLine($"Error creating drawing: {ex.Message}");
                }
            }
        }
    }
}
