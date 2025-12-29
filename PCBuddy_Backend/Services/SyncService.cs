using Microsoft.Data.SqlClient;
using PCBuddy_Backend.DTOs;
using Microsoft.Extensions.Configuration;

namespace PCBuddy_Backend.Services
{
    public class SyncService
    {
        private readonly string _connectionString;

        public SyncService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SyncResponseDto> GetReferenceDataAsync(DateTime? lastSync)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            DateTime syncTimestamp = DateTime.UtcNow;

            string dateFilter = lastSync.HasValue ? "WHERE UpdatedAt > @LastSync" : "";


            var cpus = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Cpus {dateFilter}", lastSync, r =>
                new CpuDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var gpus = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Gpus {dateFilter}", lastSync, r =>
                new GpuDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var memories = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Memory {dateFilter}", lastSync, r =>
                new MemoryDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var storages = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Storages {dateFilter}", lastSync, r =>
                new StorageDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var motherboards = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Motherboards {dateFilter}", lastSync, r =>
                new MotherboardDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var powerSupplies = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM PowerSupplies {dateFilter}", lastSync, r =>
                new PowerSupplyDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var cases = await ReadParts(conn, $"SELECT Id, Name, Price, IsDeleted FROM Cases {dateFilter}", lastSync, r =>
                new CaseDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    r.GetBoolean(3)
                ));

            var games = await ReadParts(conn,
                $"""
                SELECT Id, Name, Cpu, GraphicsCard, Memory, FileSize, IsDeleted
                FROM Games
                {dateFilter}
                """,
                lastSync,
                r => new GameSyncDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? null : r.GetString(2),
                    r.IsDBNull(3) ? null : r.GetString(3),
                    r.IsDBNull(4) ? null : Convert.ToDecimal(r.GetDouble(4)),
                    r.IsDBNull(5) ? null : Convert.ToDecimal(r.GetDouble(5)),
                    r.GetBoolean(6)
                )
            );

            return new SyncResponseDto(
                cpus,
                gpus,
                memories,
                storages,
                motherboards,
                powerSupplies,
                cases,
                games,
                syncTimestamp.ToString("o")
            );
        }

        private async Task<List<T>> ReadParts<T>(SqlConnection conn, string query, DateTime? lastSync, Func<SqlDataReader, T> map)
        {
            var list = new List<T>();
            using var cmd = new SqlCommand(query, conn);

            if (lastSync.HasValue)
            {
                cmd.Parameters.Add(new SqlParameter("@LastSync", System.Data.SqlDbType.DateTime2) { Value = lastSync.Value });
            }

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(map(reader));
            }
            return list;
        }
    }
}