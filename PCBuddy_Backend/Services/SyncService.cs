using Microsoft.Data.SqlClient; // Use this, remove System.Data.SqlClient
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

        public async Task<SyncResponseDto?> GetReferenceDataAsync(string? clientVersion)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string serverVersion = "1.0.0";
            string versionQuery = "SELECT SettingValue FROM SystemSettings WHERE SettingKey = 'DataVersion'";

            using (var cmd = new SqlCommand(versionQuery, conn))
            {
                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    serverVersion = result.ToString();
                }
            }

            if (!string.IsNullOrEmpty(clientVersion) &&
                string.Equals(clientVersion, serverVersion, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var cpus = await ReadParts(conn, "SELECT Id, Name, Price FROM Cpus", r =>
                new CpuDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var gpus = await ReadParts(conn, "SELECT Id, Name, Price FROM Gpus", r =>
                new GpuDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var memories = await ReadParts(conn, "SELECT Id, Name, Price FROM Memory", r =>
                new MemoryDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var storages = await ReadParts(conn, "SELECT Id, Name, Price FROM Storages", r =>
                new StorageDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var motherboards = await ReadParts(conn, "SELECT Id, Name, Price FROM Motherboards", r =>
                new MotherboardDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var powerSupplies = await ReadParts(conn, "SELECT Id, Name, Price FROM PowerSupplies", r =>
                new PowerSupplyDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var cases = await ReadParts(conn, "SELECT Id, Name, Price FROM Cases", r =>
                new CaseDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            var games = await ReadParts(conn,
                """
                SELECT Id, Name, Cpu, GraphicsCard, Memory, FileSize
                FROM Games
                """,
                r => new GameSyncDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? null : r.GetString(2),
                    r.IsDBNull(3) ? null : r.GetString(3),
                    r.IsDBNull(4) ? null : Convert.ToDecimal(r.GetDouble(4)),
                    r.IsDBNull(5) ? null : Convert.ToDecimal(r.GetDouble(5))
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
                serverVersion
            );
        }


        private async Task<List<T>> ReadParts<T>(SqlConnection conn, string query, Func<SqlDataReader, T> map)
        {
            var list = new List<T>();
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(map(reader));
            }
            return list;
        }
    }
}