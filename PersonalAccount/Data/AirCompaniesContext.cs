using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PersonalAccount.Data
{
    public partial class AirCompaniesContext : DbContext
    {
        public AirCompaniesContext()
        {
        }

        public AirCompaniesContext(DbContextOptions<AirCompaniesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirlineCompany> AirlineCompany { get; set; } = null!;
        public virtual DbSet<DataAll> DataAlls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AirCompanies;Username=postgres;Password=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataAll>(entity =>
            {
                entity.HasNoKey();
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
