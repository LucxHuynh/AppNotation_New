using Google.Cloud.Firestore;
using NotationApp.Models;
using System.Diagnostics;
using System.Text;

namespace NotationApp.Services
{
    public class FirestoreService : IFirestoreService
    {
        private FirestoreDb _db;
        private readonly string _projectId = "my-maui";
        private readonly string _credentialsJson = @"{
          ""type"": ""service_account"",
          ""project_id"": ""my-maui"",
          ""private_key_id"": ""313ab24b2dd615b74421bdbca900a52ed737eed3"",
          ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDJYsU95U94mLuD\nit0XSzSoQvQ1B46p/3/CdWUXC/EgsfTKKBrVs+ccb8qjAC2e9UImkYWxx2FnxpQ7\nDNB3HPQHxEdfgc8eXGpfxNxUoDlZU0jXJDXqEDfheC5YLoEo++EPuEzr+gSl1rXo\n6YhLmQ/TrM0mNh0ObmC1FfBPNnLFtvf9kTdfHg3mvqi9Hb4hK+JOUoN9jorW9CZ5\nAB6e8kWGdGXwo58P1C8CBt6GwthdIiFaekDsmnXRX2orp8bdZz6haWotWPIMH+YT\n4cwTnDzHnVhrhMwTN3nPN57ZaDNvYqyK5t2e0U8QtNbK2Zc8m6WZf3MRWsZn/nMU\nKDgibtDDAgMBAAECggEADj3/erH3GFgeLskz0JwUzxozwMpWMxHq3FBGxBtSmWrX\nHTLlOZwoVJECu4YIM+00GUaCYNZxE5CY1sJAaKQcuAM6vvePCR56YWfVOIuU8rXF\nTtMfQvyUTyGbjsifLCGqwtytsqSduTLBmQVnCl2lSmWL+U9WEmQI+oA2MgvvkFQ7\nqQNW4G40kk4hFu3fv5pjkWyPeHGMcWqfscyZq7h7Gca8o8fkJA1765aVGL5Z7RiQ\n4nXN2lXmnEU+M5a+go/nz7Rg6nXu5IVJ60OS4GUccqWR8PEe1le6Q2e7AAU8qURf\nH2hU7x7hYnHJ3Ns7V87NX6cJkx6aIE6LFXMwlPOLlQKBgQDo1yI+2Sg9jXTOxdqf\n37F9YvcdlCnXL+QFKZJJzL0CARNvzrBN3rxfMV+LBBznL7wglMjPeqPcgEsEdHTU\nGcAaJDaIj5+D2F0dEOkhNw5RE64kVamQFtVtdXpv2ZzjRvpeRRlulWedGmL38rP1\n3aYPkwJcyXJKLsrMn/jpxVj/NQKBgQDdarPfOsqhSfWNR5CqfvbauahmZj1HvJUG\np6rWvuxM5+EGoysipq3bMHKO859zsrACIhuDBS850SglHqClh8P9p03bPtc/GXNH\ngOoPO4VEUhYl0OJcy/6X/jLFytbiQFcf+KJOByY6Ba8GIESfvUW9JIW6USQPU1rY\nP2JsYB23FwKBgBcamcxQsfyBl9CYs1vfz3XYpxqpRAmVN/QHLvvhs/OJ9crHYJkp\ne9maRZe+vbt28hzthouH6NCNbmQvxhPxxi0R4NNmJPbpNjwXHHruQp5q7IGwelXQ\n3jaxTIfiuP2YYmmOQ7rEWnatUpq124OPDdZHyRk55/xSbk8lpIZVzcHtAoGARQT2\n7vX3phFU7uZJDSyorIPFH65tGciKN9naIji/ljWN6rDASHvmo/XVMAR3RuyDexbA\ntrXOA9rUBcYnU5IfoRke5uIO3MeFLNvdmkP2hdaEuuMDPOQGB8EgeJGDLGHcBsZN\nab9Gxj1xUpqKabTpYU1aNjsG35QfNFuFxbysZp8CgYEAzcymVu2pB64FtALExp6W\n7T0jtfyDTnGXts72sfGE7qpC9s5VVl37lURLxqcJQahgwZHoWK3mRnSdpSPZ+u1P\nmrfT+uue7o/YXIHxgq7GJacC2Fb4cdFQic1ulw+q9nYitp/42jhQAka9ZUdNY3v4\nNYfSASUiyrf3A4+Mhu2cjgI=\n-----END PRIVATE KEY-----\n"",
          ""client_email"": ""firebase-adminsdk-oao1f@my-maui.iam.gserviceaccount.com"",
          ""client_id"": ""100273268017663856715"",
          ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
          ""token_uri"": ""https://oauth2.googleapis.com/token"",
          ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
          ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-oao1f%40my-maui.iam.gserviceaccount.com"",
          ""universe_domain"": ""googleapis.com""
        }";

        public FirestoreService()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Initializing FirestoreService...");
                InitializeFirestore();
                System.Diagnostics.Debug.WriteLine("FirestoreService initialized successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing FirestoreService: {ex}");
                throw;
            }
        }

        private void InitializeFirestore()
        {
            try
            {
                // Save credentials to a temporary file
                string tempPath = Path.Combine(FileSystem.CacheDirectory, "firebase-credentials.json");
                File.WriteAllText(tempPath, _credentialsJson);

                // Set the environment variable
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);

                System.Diagnostics.Debug.WriteLine("Firestore credentials set successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InitializeFirestore: {ex}");
                throw;
            }
        }

        private async Task EnsureFirestoreInitialized()
        {
            if (_db == null)
            {
                try
                {
                    var builder = new FirestoreDbBuilder
                    {
                        ProjectId = _projectId,
                        JsonCredentials = _credentialsJson
                    };
                    _db = await builder.BuildAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error initializing Firestore: {ex}");
                    throw;
                }
            }
        }

        public async Task<UserProfile> GetUserProfile(string userId)
        {
            try
            {
                await EnsureFirestoreInitialized();
                System.Diagnostics.Debug.WriteLine($"Getting profile for userId: {userId}");

                if (string.IsNullOrEmpty(userId))
                {
                    System.Diagnostics.Debug.WriteLine("UserId is empty");
                    return null;
                }

                // Get document reference
                DocumentReference docRef = _db.Collection("users").Document(userId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                System.Diagnostics.Debug.WriteLine($"Document exists: {snapshot.Exists}");

                if (snapshot.Exists)
                {
                    // Log raw data
                    var data = snapshot.ToDictionary();
                    foreach (var item in data)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field: {item.Key}, Value: {item.Value}");
                    }

                    // Convert to UserProfile
                    var profile = new UserProfile
                    {
                        Id = userId,
                        DisplayName = snapshot.GetValue<string>("DisplayName"),
                        Email = snapshot.GetValue<string>("Email"),
                        PhotoUrl = snapshot.GetValue<string>("PhotoUrl"),
                        Bio = snapshot.GetValue<string>("Bio"),
                        PhoneNumber = snapshot.GetValue<string>("PhoneNumber"),
                        DateJoined = snapshot.GetValue<DateTime?>("DateJoined")
                    };

                    System.Diagnostics.Debug.WriteLine($"Converted profile - Name: {profile.DisplayName}, Email: {profile.Email}");
                    return profile;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No document found for this userId");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetUserProfile: {ex}");
                throw;
            }
        }

        public async Task<bool> UpdateUserProfile(UserProfile profile)
        {
            try
            {
                await EnsureFirestoreInitialized();

                if (profile == null || string.IsNullOrEmpty(profile.Id))
                {
                    return false;
                }

                // Convert to dictionary to handle potential serialization issues
                var profileData = new Dictionary<string, object>
                {
                    { "Id", profile.Id },
                    { "DisplayName", profile.DisplayName ?? "" },
                    { "Email", profile.Email ?? "" },
                    { "PhotoUrl", profile.PhotoUrl ?? "" },
                    { "Bio", profile.Bio ?? "" },
                    { "DateJoined", profile.DateJoined ?? DateTime.UtcNow },
                    { "PhoneNumber", profile.PhoneNumber ?? "" }
                };

                await _db.Collection("users").Document(profile.Id).SetAsync(profileData);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateUserProfile: {ex}");
                return false;
            }
        }

        public async Task<string> UploadProfileImage(Stream imageStream, string userId)
        {
            try
            {
                // Mock implementation - in reality you would upload to Firebase Storage
                return "https://via.placeholder.com/150";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UploadProfileImage: {ex}");
                return null;
            }
        }

        // Kiểm tra xem email có phải là user hợp lệ trong hệ thống
        public async Task<bool> IsValidUser(string email)
        {
            try
            {
                await EnsureFirestoreInitialized();
                var query = _db.Collection("users").WhereEqualTo("Email", email);
                var snapshot = await query.GetSnapshotAsync();
                return snapshot.Count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking user validity: {ex.Message}");
                return false;
            }
        }

        // Lấy profile bằng email
        public async Task<UserProfile> GetUserProfileByEmail(string email)
        {
            try
            {
                await EnsureFirestoreInitialized();
                var query = _db.Collection("users").WhereEqualTo("Email", email);
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count == 0)
                    return null;

                var userDoc = snapshot.FirstOrDefault();
                return new UserProfile
                {
                    Id = userDoc.Id,
                    Email = email,
                    DisplayName = userDoc.GetValue<string>("DisplayName"),
                    // ... other properties
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting user profile: {ex.Message}");
                return null;
            }
        }

        // Cập nhật danh sách items được chia sẻ cho user
        public async Task<bool> UpdateSharedItems(string userId, string itemId, string itemType)
        {
            try
            {
                await EnsureFirestoreInitialized();
                var docRef = _db.Collection("users").Document(userId);
                var sharedItemKey = $"{itemType}_{itemId}";

                await docRef.UpdateAsync("SharedWithMe", FieldValue.ArrayUnion(sharedItemKey));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating shared items: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserProfile(string userId)
        {
            try
            {
                await EnsureFirestoreInitialized();
                var docRef = _db.Collection("users").Document(userId);
                await docRef.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting user profile: {ex.Message}");
                return false;
            }
        }
    }
}
