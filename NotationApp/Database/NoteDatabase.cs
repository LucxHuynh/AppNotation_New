using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotationApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NotationApp.Database
{
    public class NoteDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public NoteDatabase(string dbPath)
        {
            try
            {
                Debug.WriteLine($"Initializing database at: {dbPath}");

                // Đảm bảo thư mục tồn tại
                var folder = Path.GetDirectoryName(dbPath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Khởi tạo kết nối database
                _database = new SQLiteAsyncConnection(dbPath);

                // Tạo bảng Note_Realtime
                try
                {
                    Task.Run(async () => {
                        await _database.CreateTableAsync<Note_Realtime>();                        
                        Debug.WriteLine("Table Note_Realtime created successfully");
                    }).Wait();

                    Task.Run(async () => {
                        await _database.CreateTableAsync<Drawing>();
                        Debug.WriteLine("Table Drawing created successfully");
                    }).Wait();
                }
                catch (Exception tableEx)
                {
                    Debug.WriteLine($"Error creating table: {tableEx.Message}");
                    throw;
                }

                Debug.WriteLine("Database initialization completed");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database initialization error: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int> SaveNoteAsync(Note_Realtime note)
        {
            try
            {
                var existingNote = await GetNoteByIdAsync(note.Id);
                if (existingNote != null)
                {
                    // Giữ nguyên thông tin owner khi update
                    note.OwnerId = existingNote.OwnerId;
                    note.OwnerEmail = existingNote.OwnerEmail;

                    return await _database.ExecuteAsync(
                        @"UPDATE Note_Realtime SET 
                        Title = ?, 
                        Text = ?, 
                        CreateDate = ?,
                        UpdateDate = ?, 
                        IsDeleted = ?,
                        IsSynced = ?,
                        TagName = ?,
                        IsPinned = ?,
                        OwnerId = ?,
                        OwnerEmail = ?,
                        IsShared = ?,
                        Permission = ?,
                        ShareLink = ?,
                        SharedWithUsers = ?
                        WHERE Id = ?",
                        note.Title ?? string.Empty,
                        note.Text ?? string.Empty,
                        note.CreateDate,
                        note.UpdateDate,
                        note.IsDeleted,
                        note.IsSynced,
                        note.TagName ?? "Personal",
                        note.IsPinned,
                        note.OwnerId,
                        note.OwnerEmail,
                        note.IsShared,
                        (int)note.Permission,
                        note.ShareLink ?? string.Empty,
                        note.SharedWithUsers,
                        note.Id);
                }

                // Đảm bảo có OwnerId và OwnerEmail khi tạo mới
                if (string.IsNullOrEmpty(note.OwnerId))
                {
                    note.OwnerId = Preferences.Get("UserId", string.Empty);
                    note.OwnerEmail = Preferences.Get("UserEmail", string.Empty);
                }

                if (note.Id == 0)
                {
                    var notes = await _database.Table<Note_Realtime>().ToListAsync();
                    var maxId = notes.Any() ? notes.Max(d => d.Id) : 0;
                    note.Id = maxId + 1;
                }
                return await _database.InsertAsync(note);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error in SaveNoteAsync: {ex.Message}");
                throw;
            }
        }

        // ---------------- Notes ----------------
        public Task<Note_Realtime> GetNoteByIdAsync(int id)
        {
            return _database.Table<Note_Realtime>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Note_Realtime>> GetNotesAsync()
        {
            return _database.Table<Note_Realtime>().ToListAsync();
        }

        public Task<Note_Realtime> GetNoteAsync(int id)
        {
            return _database.Table<Note_Realtime>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Note_Realtime>> GetNotesToSyncAsync()
        {
            return _database.Table<Note_Realtime>().Where(note => note.IsSynced == false).ToListAsync();
        }


        public Task<int> UpdateNoteAsync(Note_Realtime note)
        {
            // Ensure SharedWithUsers is valid JSON before updating
            if (string.IsNullOrEmpty(note.SharedWithUsers))
            {
                note.SharedWithUsers = JsonConvert.SerializeObject(new Dictionary<string, string>());
            }

            return _database.ExecuteAsync(
                @"UPDATE Note_Realtime SET 
                Title = ?, 
                Text = ?, 
                CreateDate = ?,
                UpdateDate = ?, 
                IsDeleted = ?,
                IsSynced = ?,
                TagName = ?,
                IsPinned = ?,
                OwnerId = ?,
                OwnerEmail = ?,
                IsShared = ?,
                Permission = ?,
                ShareLink = ?,
                SharedWithUsers = ?
                WHERE Id = ?",
                note.Title ?? string.Empty,
                note.Text ?? string.Empty,
                note.CreateDate,
                note.UpdateDate,
                note.IsDeleted,
                note.IsSynced,
                note.TagName ?? "Personal",
                note.IsPinned,
                note.OwnerId,
                note.OwnerEmail,
                note.IsShared,
                (int)note.Permission,
                note.ShareLink ?? string.Empty,
                note.SharedWithUsers,
                note.Id);


        }

        public Task<int> DeleteNoteAsync(Note_Realtime note)
        {
            return _database.ExecuteAsync("DELETE FROM Note_Realtime WHERE Id = ?", note.Id);
        }

        public async Task<List<Note_Realtime>> GetSharedNotesAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return new List<Note_Realtime>();

            var notes = await _database.Table<Note_Realtime>()
                .Where(n => !n.IsDeleted && n.IsShared)
                .ToListAsync();

            return notes.Where(n => n.IsSharedWithUser(userEmail))
                .OrderByDescending(n => n.UpdateDate)
                .ToList();
        }

        public async Task<List<Note_Realtime>> GetOwnedNotesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<Note_Realtime>();

            return await _database.Table<Note_Realtime>()
                .Where(n => !n.IsDeleted && n.OwnerId == userId)
                .OrderByDescending(n => n.UpdateDate)
                .ToListAsync();
        }

        // ---------------- Drawings ----------------
        public Task<Drawing> GetDrawingByIdAsync(int id)
        {
            return _database.Table<Drawing>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Drawing>> GetDrawingsAsync()
        {
            return _database.Table<Drawing>().ToListAsync();
        }

        public Task<Drawing> GetDrawingAsync(int id)
        {
            return _database.Table<Drawing>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        public Task<List<Drawing>> GetDrawingsToSyncAsync()
        {
            return _database.Table<Drawing>().Where(drawing => drawing.IsSynced == false).ToListAsync();
        }

        public async Task<int> SaveDrawingAsync(Drawing drawing)
        {
            try
            {
                if (string.IsNullOrEmpty(drawing.SharedWithUsers))
                {
                    drawing.SharedWithUsers = JsonConvert.SerializeObject(new Dictionary<string, string>());
                }

                var existingDrawing = await GetDrawingByIdAsync(drawing.Id);
                if (existingDrawing != null)
                {

                    // Giữ nguyên thông tin owner khi update
                    drawing.OwnerId = existingDrawing.OwnerId;
                    drawing.OwnerEmail = existingDrawing.OwnerEmail;

                    return await _database.ExecuteAsync(
                        @"UPDATE Drawing SET 
                        Title = ?, 
                        ImagePath = ?, 
                        CreateDate = ?,
                        UpdateDate = ?, 
                        IsDeleted = ?,
                        IsSynced = ?,
                        TagName = ?,
                        IsPinned = ?,
                        OwnerId = ?,
                        OwnerEmail = ?,
                        IsShared = ?,
                        Permission = ?,
                        ShareLink = ?,
                        SharedWithUsers = ?
                        WHERE Id = ?",
                        drawing.Title ?? string.Empty,
                        drawing.ImagePath ?? string.Empty,
                        drawing.CreateDate,
                        drawing.UpdateDate,
                        drawing.IsDeleted,
                        drawing.IsSynced,
                        drawing.TagName ?? "Personal",
                        drawing.IsPinned,
                        drawing.OwnerId,
                        drawing.OwnerEmail,
                        drawing.IsShared,
                        (int)drawing.Permission,
                        drawing.ShareLink ?? string.Empty,
                        drawing.SharedWithUsers,
                        drawing.Id);
                }

                if (drawing.Id == 0)
                {
                    var drawings = await _database.Table<Drawing>().ToListAsync();
                    var maxId = drawings.Any() ? drawings.Max(d => d.Id) : 0;
                    drawing.Id = maxId + 1;
                }
                return await _database.InsertAsync(drawing);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
        }

        public Task<int> UpdateDrawingAsync(Drawing drawing)
        {
            if (string.IsNullOrEmpty(drawing.SharedWithUsers))
            {
                drawing.SharedWithUsers = JsonConvert.SerializeObject(new Dictionary<string, string>());
            }

            return _database.ExecuteAsync(
                @"UPDATE Drawing SET 
                Title = ?, 
                ImagePath = ?, 
                CreateDate = ?,
                UpdateDate = ?, 
                IsDeleted = ?,
                IsSynced = ?,
                TagName = ?,
                IsPinned = ?,
                OwnerId = ?,
                OwnerEmail = ?,
                IsShared = ?,
                Permission = ?,
                ShareLink = ?,
                SharedWithUsers = ?
                WHERE Id = ?",
                drawing.Title ?? string.Empty,
                drawing.ImagePath ?? string.Empty,
                drawing.CreateDate,
                drawing.UpdateDate,
                drawing.IsDeleted,
                drawing.IsSynced,
                drawing.TagName ?? "Personal",
                drawing.IsPinned,
                drawing.OwnerId,
                drawing.OwnerEmail,
                drawing.IsShared,
                (int)drawing.Permission,
                drawing.ShareLink ?? string.Empty,
                drawing.SharedWithUsers,
                drawing.Id);
        }

        public Task<int> DeleteDrawingAsync(Drawing drawing)
        {
            return _database.ExecuteAsync("DELETE FROM Drawing WHERE Id = ?", drawing.Id);
        }
        

        public async Task<List<Drawing>> GetOwnedDrawingsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<Drawing>();

            return await _database.Table<Drawing>()
                .Where(d => !d.IsDeleted && d.OwnerId == userId)
                .OrderByDescending(d => d.UpdateDate)
                .ToListAsync();
        }

        public async Task<List<Drawing>> GetSharedDrawingsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new List<Drawing>();

            var drawings = await _database.Table<Drawing>()
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            return drawings.Where(d =>
                d.IsShared &&
                (d.SharedWithUsers.Contains(userId) || d.Permission == Drawing.SharePermission.Full))
                .OrderByDescending(d => d.UpdateDate)
                .ToList();
        }
    }
}
