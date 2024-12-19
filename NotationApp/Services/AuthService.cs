using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Maui.Authentication;
using Newtonsoft.Json;
using NotationApp.Models;

namespace NotationApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly IWebAuthenticator _webAuthenticator;
        private readonly IFirestoreService _firestoreService;

        private readonly string _clientId = "388493852290-qbf7b6q76g6ufpod7m72o08rrtr7ikom.apps.googleusercontent.com";

        public AuthService(IWebAuthenticator webAuthenticator, IFirestoreService firestoreService)
        {
            _webAuthenticator = webAuthenticator;
            _firestoreService = firestoreService;
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyD9oQxB8qWhG02NkOSnePhGILuFJiE8olM",
                AuthDomain = "my-maui.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                    new GoogleProvider()
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




        public async Task<UserCredential> SignInWithGoogle()
        {
            try
            {
                var scopes = new[]
                {
                    "openid",
                    "email",
                    "profile"
                };

                var authUrl = new Uri(
                    $"https://accounts.google.com/o/oauth2/v2/auth" +
                    $"?client_id={_clientId}" +
                    $"&redirect_uri=com.cscompany.appnotations:/oauth2redirect" +
                    $"&response_type=code id_token" +
                    $"&scope={Uri.EscapeDataString(string.Join(" ", scopes))}" +
                    $"&nonce={Guid.NewGuid()}" +
                    $"&prompt=select_account");

                var callbackUrl = new Uri("com.cscompany.appnotations:/oauth2redirect");

                var result = await _webAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
                {
                    Url = authUrl,
                    CallbackUrl = callbackUrl,
                    PrefersEphemeralWebBrowserSession = true
                });

                if (result?.Properties != null)
                {
                    var idToken = result.Properties["id_token"];
                    if (string.IsNullOrEmpty(idToken))
                    {
                        throw new Exception("No id_token received from Google");
                    }

                    // Sign in with Firebase
                    var credential = GoogleProvider.GetCredential(idToken, OAuthCredentialTokenType.IdToken);
                    var userCredential = await _authClient.SignInWithCredentialAsync(credential);

                    if (userCredential?.User != null)
                    {
                        // Get or create user profile in Firestore
                        var existingProfile = await _firestoreService.GetUserProfile(userCredential.User.Uid);
                        if (existingProfile == null)
                        {
                            // Create new profile
                            var newProfile = new UserProfile
                            {
                                Id = userCredential.User.Uid,
                                Email = userCredential.User.Info.Email,
                                DisplayName = userCredential.User.Info.DisplayName,
                                DateJoined = DateTime.UtcNow
                            };

                            await _firestoreService.UpdateUserProfile(newProfile);
                        }
                    }

                    return userCredential;
                }

                throw new Exception("Authentication failed: No result returned");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Google sign-in error: {ex}");
                throw new Exception($"Authentication failed: {ex.Message}");
            }
        }

        public async Task SendPasswordResetEmail(string email)
        {
            try
            {
                Console.WriteLine($"Attempting to send password reset email to: {email}");

                // Validate email format first
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email address cannot be empty");
                }

                // Use the FirebaseAuthClient to send the reset email
                await _authClient.ResetEmailPasswordAsync(email);

                Console.WriteLine("Password reset email sent successfully");
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"Firebase auth error during password reset: {ex.Reason} - {ex.Message}");

                // Convert Firebase-specific errors to more user-friendly messages
                string errorMessage = ex.Reason switch
                {
                    AuthErrorReason.InvalidEmailAddress => "The email address is not valid",
                    AuthErrorReason.UserNotFound => "No account exists with this email address",
                    _ => "Failed to send password reset email"
                };

                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during password reset: {ex.Message}");
                throw new Exception("An unexpected error occurred while sending the password reset email");
            }
        }
    }
}


