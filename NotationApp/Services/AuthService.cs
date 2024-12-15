using Firebase.Auth;
using Firebase.Auth.Providers;
using NotationApp.Models;

namespace NotationApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;

        public AuthService()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyATNq2OUsfWazU8b19kSFUPJ7liNr3S1Ns",
                AuthDomain = "notationapp-98854.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            _authClient = new FirebaseAuthClient(config);
        }

        public async Task<UserCredential> SignInWithEmailAndPassword(string email, string password)
        {
            try
            {
                Console.WriteLine("Attempting to sign in...");
                var result = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
                Console.WriteLine("Sign-in successful!");
                return result;
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"Firebase authentication error: {ex.Reason} - {ex.Message}");
                throw new Exception($"Authentication failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during sign-in: {ex.Message}");
                throw new Exception($"Authentication failed: {ex.Message}");
            }
        }


        public async Task<UserCredential> CreateUserWithEmailAndPassword(string email, string password)
        {
            try
            {
                Console.WriteLine($"Attempting to register user with email: {email}");

                // Call Firebase Authentication API
                var result = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);

                if (result == null || result.User == null)
                {
                    Console.WriteLine("Firebase returned null or user information is missing.");
                    throw new Exception("Failed to register user. No response from Firebase.");
                }

                Console.WriteLine($"User registered successfully: {result.User.Info.Email}");
                return result;
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"FirebaseAuthException: {ex.Reason} - {ex.Message}");
                throw new Exception($"Firebase error: {ex.Reason} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                throw;
            }
        }




        //public async Task<Models.UserInfo> GetCurrentUserInfo()
        //{
        //    var user = _authClient.User;
        //    if (user == null)
        //        return null;

        //    return new Models.UserInfo
        //    {
        //        DisplayName = user.Info.DisplayName,
        //        Email = user.Info.Email,
        //        PhotoUrl = user.Info.PhotoUrl,
        //        Uid = user.Uid
        //    };
        //}

        //public bool IsSignedIn()
        //{
        //    return _authClient.User != null;
        //}

        //public void SignOut()
        //{
        //    _authClient.SignOut();
        //}
    }
}