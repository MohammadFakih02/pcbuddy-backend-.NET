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

            string? GetStringOrNull(SqlDataReader r, int ordinal) => r.IsDBNull(ordinal) ? null : r.GetString(ordinal);

            // 1. CPU
            var cpus = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Cpus {dateFilter}",
                lastSync, r => new CpuDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 2. GPU
            var gpus = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Gpus {dateFilter}",
                lastSync, r => new GpuDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 3. Memory
            var memories = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Memory {dateFilter}",
                lastSync, r => new MemoryDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 4. Storage
            var storages = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Storages {dateFilter}",
                lastSync, r => new StorageDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 5. Motherboards
            var motherboards = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Motherboards {dateFilter}",
                lastSync, r => new MotherboardDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 6. Power Supplies
            var powerSupplies = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM PowerSupplies {dateFilter}",
                lastSync, r => new PowerSupplyDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
                ));

            // 7. Cases
            var cases = await ReadParts(conn,
                $"SELECT Id, Name, Price, ImageUrl, IsDeleted FROM Cases {dateFilter}",
                lastSync, r => new CaseDto(
                    r.GetInt32(0),
                    r.GetString(1),
                    r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                    GetStringOrNull(r, 3),
                    r.GetBoolean(4)
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

            var prebuilts = await ReadParts(conn,
                            $"SELECT Id, Name, TotalPrice, Rating, ImageUrl, IsDeleted, CpuId, GpuId, MemoryId, StorageId, MotherboardId, PowerSupplyId, CaseId FROM PrebuiltPCs {dateFilter}",
                            lastSync,
                            r => new PrebuiltPcDto(
                                r.GetInt32(0),
                                r.GetString(1),
                                r.IsDBNull(2) ? 0 : Convert.ToDecimal(r.GetDouble(2)),
                                r.IsDBNull(3) ? 0 : r.GetDouble(3),
                                r.IsDBNull(4) ? null : r.GetString(4),
                                r.GetBoolean(5),
                                r.IsDBNull(6) ? null : r.GetInt32(6),
                                r.IsDBNull(7) ? null : r.GetInt32(7),
                                r.IsDBNull(8) ? null : r.GetInt32(8),
                                r.IsDBNull(9) ? null : r.GetInt32(9),
                                r.IsDBNull(10) ? null : r.GetInt32(10),
                                r.IsDBNull(11) ? null : r.GetInt32(11),
                                r.IsDBNull(12) ? null : r.GetInt32(12)
                            )
                        );

            return new SyncResponseDto(
                cpus, gpus, memories, storages, motherboards, powerSupplies, cases, games, prebuilts,
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