using Microsoft.Maui.Controls;
using System;
using NotationApp.ViewModels;
using System.Diagnostics;


namespace NotationApp.Pages
{
    public partial class HomePage : ContentPage
    {
        private HomePageViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();
            _viewModel = new HomePageViewModel();
            BindingContext = _viewModel;

            //// Set navigation bar appearance
            Shell.SetNavBarIsVisible(this, true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.SyncDataFromFirebaseAsync(); // Đồng bộ từ firebase
            await _viewModel.LoadItemsAsync(); // Tải và đồng bộ các ghi chú chưa đồng bộ khi trang xuất hiện
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Button clicked");
            // Kiểm tra BindingContext
            if (BindingContext == null)
            {
                Debug.WriteLine("BindingContext is null");
            }
            else
            {
                Debug.WriteLine($"BindingContext type: {BindingContext.GetType().Name}");
            }
        }
    }
}
