using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuddy_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SidePanel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerSupplyShroud = table.Column<bool>(type: "bit", nullable: true),
                    FrontPanelUsb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherboardFormFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxVideoCardLength = table.Column<double>(type: "float", nullable: true),
                    DriveBays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpansionSlots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cpus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Microarchitecture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Socket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoreCount = table.Column<int>(type: "int", nullable: true),
                    PerformanceCoreClock = table.Column<double>(type: "float", nullable: true),
                    PerformanceCoreBoostClock = table.Column<double>(type: "float", nullable: true),
                    EfficiencyCoreClock = table.Column<double>(type: "float", nullable: true),
                    EfficiencyCoreBoostClock = table.Column<double>(type: "float", nullable: true),
                    L2Cache = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    L3Cache = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tdp = table.Column<int>(type: "int", nullable: true),
                    IntegratedGraphics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSupportedMemory = table.Column<double>(type: "float", nullable: true),
                    SimultaneousMultithreading = table.Column<bool>(type: "bit", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Memory = table.Column<double>(type: "float", nullable: true),
                    GraphicsCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cpu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chipset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memory = table.Column<double>(type: "float", nullable: true),
                    MemoryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoreClock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoostClock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveMemoryClock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrameSync = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<double>(type: "float", nullable: true),
                    Tdp = table.Column<int>(type: "int", nullable: true),
                    CaseExpansionSlotWidth = table.Column<int>(type: "int", nullable: true),
                    TotalSlotWidth = table.Column<int>(type: "int", nullable: true),
                    ExternalPower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerGb = table.Column<double>(type: "float", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstWordLatency = table.Column<double>(type: "float", nullable: true),
                    CasLatency = table.Column<double>(type: "float", nullable: true),
                    Voltage = table.Column<double>(type: "float", nullable: true),
                    Timing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeatSpreader = table.Column<bool>(type: "bit", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motherboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chipset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemoryMax = table.Column<int>(type: "int", nullable: true),
                    MemoryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemorySlots = table.Column<int>(type: "int", nullable: true),
                    MemorySpeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PcieX16Slots = table.Column<int>(type: "int", nullable: true),
                    M2Slots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sata6Gbps = table.Column<int>(type: "int", nullable: true),
                    OnboardEthernet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Usb20Headers = table.Column<int>(type: "int", nullable: true),
                    Usb32Gen1Headers = table.Column<int>(type: "int", nullable: true),
                    Usb32Gen2Headers = table.Column<int>(type: "int", nullable: true),
                    Usb32Gen2x2Headers = table.Column<int>(type: "int", nullable: true),
                    WirelessNetworking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Socket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motherboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Efficiency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wattage = table.Column<int>(type: "int", nullable: true),
                    Modular = table.Column<bool>(type: "bit", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<double>(type: "float", nullable: true),
                    Atx4PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Eps8PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Molex4PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Pcie12PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Pcie12Plus4Pin12VHPWRConnectors = table.Column<int>(type: "int", nullable: true),
                    Pcie6PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Pcie6Plus2PinConnectors = table.Column<int>(type: "int", nullable: true),
                    Pcie8PinConnectors = table.Column<int>(type: "int", nullable: true),
                    SataConnectors = table.Column<int>(type: "int", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSupplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<double>(type: "float", nullable: true),
                    Cache = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Banned = table.Column<bool>(type: "bit", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLogs_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonalPCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BuildName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddToProfile = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    CpuId = table.Column<int>(type: "int", nullable: true),
                    GpuId = table.Column<int>(type: "int", nullable: true),
                    MemoryId = table.Column<int>(type: "int", nullable: true),
                    MotherboardId = table.Column<int>(type: "int", nullable: true),
                    PowerSupplyId = table.Column<int>(type: "int", nullable: true),
                    StorageId = table.Column<int>(type: "int", nullable: true),
                    StorageId2 = table.Column<int>(type: "int", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalPCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Gpus_GpuId",
                        column: x => x.GpuId,
                        principalTable: "Gpus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Memory_MemoryId",
                        column: x => x.MemoryId,
                        principalTable: "Memory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_PowerSupplies_PowerSupplyId",
                        column: x => x.PowerSupplyId,
                        principalTable: "PowerSupplies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Storages_StorageId2",
                        column: x => x.StorageId2,
                        principalTable: "Storages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalPCs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrebuiltPCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EngineerId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    CpuId = table.Column<int>(type: "int", nullable: true),
                    GpuId = table.Column<int>(type: "int", nullable: true),
                    MemoryId = table.Column<int>(type: "int", nullable: true),
                    MotherboardId = table.Column<int>(type: "int", nullable: true),
                    PowerSupplyId = table.Column<int>(type: "int", nullable: true),
                    StorageId = table.Column<int>(type: "int", nullable: true),
                    StorageId2 = table.Column<int>(type: "int", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrebuiltPCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Gpus_GpuId",
                        column: x => x.GpuId,
                        principalTable: "Gpus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Memory_MemoryId",
                        column: x => x.MemoryId,
                        principalTable: "Memory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_PowerSupplies_PowerSupplyId",
                        column: x => x.PowerSupplyId,
                        principalTable: "PowerSupplies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Storages_StorageId2",
                        column: x => x.StorageId2,
                        principalTable: "Storages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrebuiltPCs_Users_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminLogs_AdminId",
                table: "AdminLogs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLogs_UserId",
                table: "AdminLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_CaseId",
                table: "PersonalPCs",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_CpuId",
                table: "PersonalPCs",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_GpuId",
                table: "PersonalPCs",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_MemoryId",
                table: "PersonalPCs",
                column: "MemoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_MotherboardId",
                table: "PersonalPCs",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_PowerSupplyId",
                table: "PersonalPCs",
                column: "PowerSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_StorageId",
                table: "PersonalPCs",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_StorageId2",
                table: "PersonalPCs",
                column: "StorageId2");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPCs_UserId",
                table: "PersonalPCs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_CaseId",
                table: "PrebuiltPCs",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_CpuId",
                table: "PrebuiltPCs",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_EngineerId",
                table: "PrebuiltPCs",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_GpuId",
                table: "PrebuiltPCs",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_MemoryId",
                table: "PrebuiltPCs",
                column: "MemoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_MotherboardId",
                table: "PrebuiltPCs",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_PowerSupplyId",
                table: "PrebuiltPCs",
                column: "PowerSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_StorageId",
                table: "PrebuiltPCs",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PrebuiltPCs_StorageId2",
                table: "PrebuiltPCs",
                column: "StorageId2");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PersonalPCs");

            migrationBuilder.DropTable(
                name: "PrebuiltPCs");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "Memory");

            migrationBuilder.DropTable(
                name: "Motherboards");

            migrationBuilder.DropTable(
                name: "PowerSupplies");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
