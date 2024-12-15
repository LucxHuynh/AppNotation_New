using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.ViewModels
{
    public class HomeItem : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdateDate { get; set; }
        public string TagName { get; set; }
        public bool IsPinned { get; set; }
        public bool IsShared { get; set; }
        public string ItemType { get; set; } // "Note" or "Drawing"
        public object OriginalItem { get; set; }
    }
}
