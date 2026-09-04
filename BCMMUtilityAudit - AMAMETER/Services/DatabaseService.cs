using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BCMMUtilityAudit___AMAMETER.Models;

namespace BCMMUtilityAudit___AMAMETER.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection? _database;

        private static async Task Init()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "bcmmaudit.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<AuditRecord>();
        }

        public static async Task<List<AuditRecord>> GetHistoryAsync()
        {
            await Init();
            return await _database!.Table<AuditRecord>().OrderByDescending(r => r.Id).ToListAsync();
        }

        public static async Task SaveRecordAsync(AuditRecord record)
        {
            await Init();
            await _database!.InsertAsync(record);
        }
    }
}