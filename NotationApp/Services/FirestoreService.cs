using Google.Cloud.Firestore;
using NotationApp.Models;
using System.Diagnostics;
using System.Text;

namespace NotationApp.Services
{
    public class FirestoreService : IFirestoreService
    {
        private FirestoreDb _db;
        private readonly string _projectId = "notationapp-98854";
        private readonly string _credentialsJson = @"{
          ""type"": ""service_account"",
          ""project_id"": ""notationapp-98854"",
          ""private_key_id"": ""95634ddbf2c9f3ab4afc0d77c706ed681caf3644"",
          ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDW8oSOKs58iy8L\nxt4eP4KeU4JwvgFIr4wk/9S8aLAJtAh2jNL+YBBQfPDOFR9ppj4VJ1FKQZycQV/L\njo4ZUXLYthoP/ZSB2ifNHitN8I5aB/63s3aGWqhxScMqfSGbIXF2kmLB8axq20Xr\nXt1gNFoQbZiJm1QR10FLbQasMvnvZ8sHSEcKr9FT7xZTQZtEkfm33OVklD2MaJuc\ngWbiL+iQRAQejfpfnsGtJwY1yG1EJciz5GNue2z5iTQKzwUAHo4H61RFydHOVBwx\ncM/3XugLLH2U5DYNtsyG1fry93esnziAEigR0lLExcOBKpTczSmWW2TIpqiLh8d4\nYcjgC5GrAgMBAAECggEAFe4DmiGXPtulvsouVTWzlNr+SZReZdULM6cqRC1bcHyp\nXwxPu9t/Lpxgpms/RjR2IbWzic5B39XyTmlKlB4G3UH5ohIEDxomdLs+6HfCKOtR\n5doYc0KtQXRTcMf4IBn8YoAy39378kxRc4m9L5iHwSSoAGS+4gcXxqM79WD0tk5X\nvq3yuk0kWJT2it0DMz07P3t59SZtM/qbzRtbI92F7JznYydQikkgdb1EcppQ0xGs\nzkfVn+384Cug8ZROZW0zaFaG80ZiaZBcOp8Ejba2Q3UD/9YssB0lJwXGj8+XyBRZ\noaRwFTRUhZYoUonycKKri1rpcYjdkuMWAmKljCPrqQKBgQDssxyN7dHQKeg3oycQ\ndgYUod3S9R961d7Nw6qQsAeww/WhG6lzyLhLxdgwHq3FFJT+Bz1CFroyX9sjWda+\nbgE2JmEfEiG03wJKALoaDwxwlPB/x3OZMIXfK0TbQjjbFrO5oYfbcSVYKEQ9XR1V\nwIm+Qu5SgcJZXPOLhUaPw0mwvQKBgQDoeVi1IlWgdi5DMrSBObVKQFOurHIHWKCb\nJzGrbv7P64rRBUPfSlN8fnnGQdQqutw8GFr32UGmsP1+Y3RaKI9Uh4eOEtg57tFc\n+AOPCy3AD3YRcwzeHqFNYAyaSeMJyvfL+s5iXORNLJeoSq+SWYS3oYoqHNc/0BL3\n4frL6b22hwKBgQCF0DRiYIJvBmILoibhfXeujlvTeVRUozgUZ3uz1HBklEP20qaX\nmG/oFm9DtPRUKHCatAkDpcmZ1OsULyPiAFqa/FOHtz5q/HBU2dhcBwnnU6wq8Nz/\nS5lDMzj4I5X09f6hARrHCk4saoc5kbyV0AWPFFObPVRcJNpik/PyLlsomQKBgFwl\nZTT+9APTdEjse0HRlvtkfgb5kYU56wc0xOpX56aQjYiGVr3WHzt0gl5ET43UXWFw\nofYl5nDlLMWBNGhcNVvjEKcO7ygfclswb3CulKbROhJ2kP4mE/ewf65UZSrHiesP\ncejpOxEgAMkTOX5//ywuWq6Qmf1QQ4R/zUIwVyNXAoGBAI7eGFYDFz9gi6F01zpW\ndRfmJjoDJqVMUH9+WMVAlxmjO6vcUxOj08UUIXy/8eSjrwebZKfXLJ9oj+Lqv2SA\nZA/kxMFvyFqYBxe892DKkDSF5jtG0+NvhT9OWaJG2nCDZdKwYcTz/m96er4E9hKQ\niRhTbbyx1PSZX4smfy7ehY0f\n-----END PRIVATE KEY-----\n"",
          ""client_email"": ""firebase-adminsdk-fjj74@notationapp-98854.iam.gserviceaccount.com"",
          ""client_id"": ""109023486775148635970"",
          ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
          ""token_uri"": ""https://oauth2.googleapis.com/token"",
          ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
          ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-fjj74%40notationapp-98854.iam.gserviceaccount.com"",
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
    }
}
