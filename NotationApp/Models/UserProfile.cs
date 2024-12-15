using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public class UserProfile : ObservableObject
    {
        private string id;
        private string displayName;
        private string email;
        private string photoUrl;
        private string bio;
        private DateTime? dateJoined;
        private string phoneNumber;
        private List<string> sharedWithMe = new();

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string DisplayName
        {
            get => displayName;
            set => SetProperty(ref displayName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string PhotoUrl
        {
            get => photoUrl;
            set => SetProperty(ref photoUrl, value);
        }

        public string Bio
        {
            get => bio;
            set => SetProperty(ref bio, value);
        }

        public DateTime? DateJoined
        {
            get => dateJoined;
            set => SetProperty(ref dateJoined, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        
        public List<string> SharedWithMe
        {
            get => sharedWithMe;
            set => SetProperty(ref sharedWithMe, value);
        }

        private List<string> mySharedItems = new();
        public List<string> MySharedItems
        {
            get => mySharedItems;
            set => SetProperty(ref mySharedItems, value);
        }
    }
}
