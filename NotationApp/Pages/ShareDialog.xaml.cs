using CommunityToolkit.Maui.Views;
using NotationApp.Database;
using NotationApp.Services;
using NotationApp.ViewModels;
using System.Diagnostics;

namespace NotationApp.Pages;

public partial class ShareDialog : Popup
{
    public ShareDialog(object item, IFirestoreService firestoreService, NoteDatabase database)
    {
        InitializeComponent();
        Debug.WriteLine("Initializing ShareDialog");

        // Lấy FirestoreService từ DI container nếu được truyền vào null
        var service = firestoreService ?? IPlatformApplication.Current.Services.GetService<IFirestoreService>();
        if (service == null)
        {
            service = new FirestoreService(); // Fallback to create new instance
        }

        BindingContext = new ShareDialogViewModel(
            item,
            service,
            App.Database,
            () => Close()
        );
    }
}