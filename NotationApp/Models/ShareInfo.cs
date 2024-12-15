using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public class ShareInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ItemId { get; set; }  // ID of the shared note or drawing
        public string ItemType { get; set; }  // "Note" or "Drawing"
        public string OwnerId { get; set; }
        public DateTime SharedDate { get; set; } = DateTime.Now;
        public string[] SharedWithUsers { get; set; } = Array.Empty<string>();
        public string ShareLink { get; set; } = string.Empty;
        public SharePermission Permission { get; set; } = SharePermission.ReadOnly;
        public bool IsActive { get; set; } = true;

        public enum SharePermission
        {
            ReadOnly,
            ReadWrite,
            Full
        }
    }
}
