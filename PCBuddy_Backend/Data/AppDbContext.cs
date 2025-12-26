using Microsoft.EntityFrameworkCore;
using PCBuddy.Models;

namespace PCBuddy.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cpu> Cpus { get; set; }
        public DbSet<Gpu> Gpus { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Memory> Memory { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<PowerSupply> PowerSupplies { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<PersonalPC> PersonalPCs { get; set; }
        public DbSet<PrebuiltPC> PrebuiltPCs { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<PersonalPC>()
            .Property(p => p.BuildStatus)
            .HasConversion<string>();
            modelBuilder.Entity<Game>()
                .HasIndex(g => g.Name)
                .IsUnique();
            modelBuilder.Entity<AdminLog>()
               .HasOne(al => al.Admin)
               .WithMany(u => u.AdminLogs)
               .HasForeignKey(al => al.AdminId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonalPC>()
                .HasOne(p => p.Storage)
                .WithMany()
                .HasForeignKey(p => p.StorageId);

            modelBuilder.Entity<PersonalPC>()
                .HasOne(p => p.Storage2)
                .WithMany()
                .HasForeignKey(p => p.StorageId2);

            modelBuilder.Entity<PrebuiltPC>()
                .HasOne(p => p.Engineer)
                .WithMany(u => u.EngineeredPCs)
                .HasForeignKey(p => p.EngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrebuiltPC>()
               .HasOne(p => p.Storage)
               .WithMany()
               .HasForeignKey(p => p.StorageId);

            modelBuilder.Entity<PrebuiltPC>()
                .HasOne(p => p.Storage2)
                .WithMany()
                .HasForeignKey(p => p.StorageId2);
        }
    }
}
