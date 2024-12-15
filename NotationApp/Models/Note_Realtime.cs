using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public class Note_Realtime
    {
        //[PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Id tự động tăng
        public string Title { get; set; } = string.Empty; // Không nullable và đảm bảo có giá trị mặc định
        public string Text { get; set; } = string.Empty;  // Không nullable và đảm bảo có giá trị mặc định

        // Thuộc tính ngày tạo (CreateDate)
        public DateTime CreateDate { get; set; } = DateTime.Now; // Lấy ngày giờ hiện tại làm giá trị mặc định

        // Thuộc tính ngày cập nhật (UpdateDate)
        public DateTime UpdateDate { get; set; } = DateTime.Now; // Cập nhật tự động khi ghi chú được chỉnh sửa

        // Thuộc tính đánh dấu ghi chú bị xóa (IsDeleted)
        public bool IsDeleted { get; set; } = false; // Đánh dấu mặc định là ghi chú chưa bị xóa

        // Thuộc tính đánh dấu ghi chú đã được chọn để thao tác
        public bool IsSelected { get; set; } = false; // Mặc định chưa được chọn
        // Đánh dấu trạng thái đồng bộ của ghi chú
        public bool IsSynced { get; set; } = false;
        // Thuộc tính tag
        public string TagName { get; set; } = "Personal";
        // Thuộc tính ghim
        public bool IsPinned { get; set; } = false;

        // New properties for user and sharing
        public string OwnerId { get; set; } = string.Empty; // ID of the note creator
        public bool IsShared { get; set; } = false; // Indicates if note is shared
        public string SharedWithUsers { get; set; } = JsonConvert.SerializeObject(new Dictionary<string, string>());
        public string ShareLink { get; set; } = string.Empty; // Optional public share link
        public SharePermission Permission { get; set; } = SharePermission.ReadOnly; // Permission level for shared users

        


        // Thuộc tính chỉ đọc để hiển thị văn bản thuần túy không có định dạng HTML
        public string PlainText
        {
            get
            {
                return Text == null ? string.Empty : StripHtmlTags(Text);
            }
        }

        // Hàm loại bỏ các thẻ HTML
        private string StripHtmlTags(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // Giải mã các ký tự HTML và loại bỏ các thẻ HTML
            string decodedText = System.Net.WebUtility.HtmlDecode(input);
            return System.Text.RegularExpressions.Regex.Replace(decodedText, "<.*?>", string.Empty);
        }

        public enum SharePermission
        {
            ReadOnly,
            ReadWrite,
            Full // Includes ability to reshare
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
            if (OwnerId == userEmail)
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
