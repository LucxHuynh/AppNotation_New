
using Firebase.Auth.Providers;
using Firebase.Auth;
using NotationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NotationApp.Services
{
    public interface IAuthService
    {
        Task<UserCredential> SignInWithEmailAndPassword(string email, string password);
        Task<UserCredential> CreateUserWithEmailAndPassword(string email, string password);

        Task<UserCredential> SignInWithGoogle();
        Task SendPasswordResetEmail(string email);
    }
}
