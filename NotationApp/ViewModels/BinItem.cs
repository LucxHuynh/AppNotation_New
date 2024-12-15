using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.ViewModels
{
    public class BinItem : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsSelected { get; set; }
        public string ItemType { get; set; } // "Note" hoặc "Drawing"
        public object OriginalItem { get; set; } // Lưu trữ đối tượng gốc
    }
}
