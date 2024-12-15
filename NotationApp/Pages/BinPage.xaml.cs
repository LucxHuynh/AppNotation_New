using Firebase.Database;
using NotationApp.Models;
using NotationApp.ViewModels;
using System.Collections.ObjectModel;

namespace NotationApp.Pages
{
    public partial class BinPage : ContentPage
    {
        private readonly BinPageViewModel _viewModel;

        public BinPage()
        {
            InitializeComponent();
            // Bỏ việc tạo BindingContext trong XAML
            _viewModel = new BinPageViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadDeletedItemsAsync();
        }

        private void OnSelectAllTapped(object sender, EventArgs e)
        {
            _viewModel.SelectAllItems();
        }

        private async void OnDeleteSelectedItems(object sender, EventArgs e)
        {
            if (!_viewModel.DeletedItems.Any(x => x.IsSelected))
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn ít nhất một mục để xóa.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Xác nhận xóa",
                "Bạn có chắc chắn muốn xóa vĩnh viễn các mục đã chọn không?", "Xóa", "Hủy");

            if (confirm)
            {
                await _viewModel.DeleteSelectedItemsAsync();
                await DisplayAlert("Xóa thành công", "Các mục đã được xóa vĩnh viễn.", "OK");
            }
        }

        private async void OnRestoreSelectedItems(object sender, EventArgs e)
        {
            if (!_viewModel.DeletedItems.Any(x => x.IsSelected))
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn ít nhất một mục để phục hồi.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Xác nhận phục hồi",
                "Bạn có chắc chắn muốn phục hồi các mục đã chọn không?", "Phục hồi", "Hủy");

            if (confirm)
            {
                await _viewModel.RestoreSelectedItemsAsync();
                await DisplayAlert("Phục hồi thành công", "Các mục đã được phục hồi.", "OK");
            }
        }

        private void OnItemCheckboxTapped(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is BinItem item)
            {
                item.IsSelected = e.Value;
            }
        }
    }
}