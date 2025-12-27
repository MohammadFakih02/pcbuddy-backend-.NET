using Microsoft.Data.SqlClient;
using PCBuddy_Backend.DTOs;
using System.Data.SqlClient;

namespace PCBuddy_Backend.Services
{
    public class SyncService
    {
        private readonly string _connectionString;

        public SyncService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SyncResponseDto> GetReferenceDataAsync()
        {
            var cpus = new List<CpuDto>();
            var gpus = new List<GpuDto>();
            var memories = new List<MemoryDto>();
            var storages = new List<StorageDto>();
            var motherboards = new List<MotherboardDto>();
            var powerSupplies = new List<PowerSupplyDto>();
            var cases = new List<CaseDto>();
            var games = new List<GameSyncDto>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            async Task<List<T>> ReadParts<T>(string query, Func<SqlDataReader, T> map)
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

            cpus = await ReadParts("SELECT Id, Name, Price FROM Cpus", r =>
                new CpuDto(r.GetInt32(0), r.GetString(1),Convert.ToDecimal(r.GetDouble(2))));

            gpus = await ReadParts("SELECT Id, Name, Price FROM Gpus", r =>
                new GpuDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            memories = await ReadParts("SELECT Id, Name, Price FROM Memory", r =>
                new MemoryDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            storages = await ReadParts("SELECT Id, Name, Price FROM Storages", r =>
                new StorageDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            motherboards = await ReadParts("SELECT Id, Name, Price FROM Motherboards", r =>
                new MotherboardDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            powerSupplies = await ReadParts("SELECT Id, Name, Price FROM PowerSupplies", r =>
                new PowerSupplyDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            cases = await ReadParts("SELECT Id, Name, Price FROM Cases", r =>
                new CaseDto(r.GetInt32(0), r.GetString(1), Convert.ToDecimal(r.GetDouble(2))));

            games = await ReadParts(
                """
    SELECT Id, Name, Cpu, GraphicsCard, Memory, FileSize
    FROM Games
    """,
                r => new GameSyncDto(
                    r.GetInt32(0),                         // Id
                    r.GetString(1),                        // Name
                    r.IsDBNull(2) ? null : r.GetString(2), // Cpu
                    r.IsDBNull(3) ? null : r.GetString(3), // GPU
                    r.IsDBNull(4) ? null : Convert.ToDecimal(r.GetDouble(4)), // Memory (GB)
                    r.IsDBNull(5) ? null : Convert.ToDecimal(r.GetDouble(5))  // FileSize (GB)
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
                version: "2025.01"
            );
        }
    }
}
