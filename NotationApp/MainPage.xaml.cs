namespace NotationApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                FloatingButton.IsVisible = false;
                // Add ToolbarItem for Windows
                if (!ToolbarItems.Contains(ToolbarAddButton))
                {
                    ToolbarItems.Add(ToolbarAddButton);
                }
            }
            else
            {
                FloatingButton.IsVisible = true;
                // Remove ToolbarItem for Android/iOS
                if (ToolbarItems.Contains(ToolbarAddButton))
                {
                    ToolbarItems.Remove(ToolbarAddButton);
                }
            }
        }
    }
}
