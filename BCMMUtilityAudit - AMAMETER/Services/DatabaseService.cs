using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCMMUtilityAudit___AMAMETER.Models;

namespace BCMMUtilityAudit___AMAMETER.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            // Define local file path for the SQLite database
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "amameter_audits.db3");
            _database = new SQLiteAsyncConnection(databasePath);

            // Create the table if it doesn't already exist
            await _database.CreateTableAsync<AuditRecord>();
        }

        public async Task<int> SaveAuditRecordAsync(AuditRecord record)
        {
            await InitAsync();
            return await _database!.InsertAsync(record);
        }

        public async Task<List<AuditRecord>> GetAuditRecordsAsync()
        {
            await InitAsync();
            return await _database!.Table<AuditRecord>().ToListAsync();
        }
    }
}