using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public class Drawing
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsSynced { get; set; } = false;
        // Thuộc tính tag
        public string TagName { get; set; } = "Cá nhân";
        // Thuộc tính ghim
        public bool IsPinned { get; set; } = false;
        // New sharing properties
        public string OwnerId { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public bool IsShared { get; set; } = false;
        public string SharedWithUsers { get; set; } = JsonConvert.SerializeObject(new Dictionary<string, string>());
        public string ShareLink { get; set; } = string.Empty;
        public SharePermission Permission { get; set; } = SharePermission.ReadOnly;

        public enum SharePermission
        {
            ReadOnly,
            ReadWrite,
            Full
        }

        public bool IsSharedWithUser(string userEmail)
        {
            if (!IsShared || IsDeleted || string.IsNullOrEmpty(userEmail))
                return false;

            try
            {
                var sharedUsers = JsonConvert.DeserializeObject<Dictionary<string, string>>(SharedWithUsers ?? "{}");
                return sharedUsers.ContainsKey(userEmail);
            }
            catch
            {
                return false;
            }
        }


        public bool CanUserEdit(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return false;

            // Owner can always edit
            if (OwnerEmail == userEmail)
                return true;

            try
            {
                var sharedUsers = JsonConvert.DeserializeObject<Dictionary<string, string>>(SharedWithUsers ?? "{}");
                if (sharedUsers.TryGetValue(userEmail, out string permission))
                {
                    return permission == "ReadWrite" || permission == "Full";
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

}
