using Firebase.Database;
using Firebase.Database.Query;
using NotationApp.Models;
using NotationApp.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NotationApp.ViewModels
{
    public partial class BinPageViewModel : ObservableObject
    {
        private readonly NoteDatabase _database;
        private readonly FirebaseClient firebaseClient;

        public ObservableCollection<BinItem> DeletedItems { get; private set; } = new();

        public BinPageViewModel()
        {
            _database = App.Database;
            firebaseClient = new FirebaseClient("https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/");
        }

        public async Task LoadDeletedItemsAsync()
        {
            DeletedItems.Clear();
            await LoadDeletedNotes();
            await LoadDeletedDrawings();

            // Sắp xếp theo ngày
            var sortedItems = DeletedItems.OrderByDescending(x => x.UpdateDate).ToList();
            DeletedItems.Clear();
            foreach (var item in sortedItems)
            {
                DeletedItems.Add(item);
            }
        }

        private async Task LoadDeletedNotes()
        {

            var userId = Preferences.Get("UserId", string.Empty);
            var notes = await _database.GetNotesAsync();
            foreach (var note in notes.Where(n => n.IsDeleted && n.OwnerId == userId))
            {
                DeletedItems.Add(new BinItem
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.PlainText,
                    UpdateDate = note.UpdateDate,
                    IsSelected = false,
                    ItemType = "Note",
                    OriginalItem = note
                });
            }
        }

        private async Task LoadDeletedDrawings()
        {
            var userId = Preferences.Get("UserId", string.Empty);
            var drawings = await _database.GetDrawingsAsync();
            foreach (var drawing in drawings.Where(d => d.IsDeleted && d.OwnerId == userId))
            {
                DeletedItems.Add(new BinItem
                {
                    Id = drawing.Id,
                    Title = drawing.Title,
                    Content = "Drawing",
                    UpdateDate = drawing.UpdateDate,
                    IsSelected = false,
                    ItemType = "Drawing",
                    OriginalItem = drawing
                });
            }
        }

        [RelayCommand]
        public void SelectAllItems()
        {
            bool selectAll = DeletedItems.Any(item => !item.IsSelected);
            foreach (var item in DeletedItems)
            {
                item.IsSelected = selectAll;
            }
            OnPropertyChanged(nameof(DeletedItems));
        }

        [RelayCommand]
        public async Task RestoreSelectedItemsAsync()
        {
            var selectedItems = DeletedItems.Where(item => item.IsSelected).ToList();

            foreach (var item in selectedItems)
            {
                if (item.ItemType == "Note" && item.OriginalItem is Note_Realtime note)
                {
                    note.IsDeleted = false;
                    note.UpdateDate = DateTime.Now;
                    await _database.UpdateNoteAsync(note);
                }
                else if (item.ItemType == "Drawing" && item.OriginalItem is Drawing drawing)
                {
                    drawing.IsDeleted = false;
                    drawing.UpdateDate = DateTime.Now;
                    await _database.UpdateDrawingAsync(drawing);
                }
                DeletedItems.Remove(item);
            }
        }

        [RelayCommand]
        public async Task DeleteSelectedItemsAsync()
        {
            var selectedItems = DeletedItems.Where(item => item.IsSelected).ToList();

            foreach (var item in selectedItems)
            {
                try
                {
                    string firebaseUrl = $"https://appnotation-79a96-default-rtdb.asia-southeast1.firebasedatabase.app/{(item.ItemType.ToLower())}s/{item.Id}.json";
                    using (var client = new HttpClient())
                    {
                        var response = await client.DeleteAsync(firebaseUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            if (item.ItemType == "Note" && item.OriginalItem is Note_Realtime note)
                            {
                                await _database.DeleteNoteAsync(note);
                            }
                            else if (item.ItemType == "Drawing" && item.OriginalItem is Drawing drawing)
                            {
                                await _database.DeleteDrawingAsync(drawing);
                            }
                            DeletedItems.Remove(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting {item.ItemType} with ID {item.Id}: {ex.Message}");
                }
            }
        }
    }
}