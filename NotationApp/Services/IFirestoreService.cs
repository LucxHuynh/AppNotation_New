using NotationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Services
{
    public interface IFirestoreService
    {
        Task<UserProfile> GetUserProfile(string userId);
        Task<bool> UpdateUserProfile(UserProfile profile);
        Task<string> UploadProfileImage(Stream imageStream, string userId);

        Task<bool> IsValidUser(string email);
        Task<UserProfile> GetUserProfileByEmail(string email);
        Task<bool> UpdateSharedItems(string userId, string itemId, string itemType);

        Task<bool> DeleteUserProfile(string userId);
    }
}
