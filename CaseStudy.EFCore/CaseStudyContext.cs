using CaseStudy.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CaseStudy.EFCore
{
    public class CaseStudyContext:DbContext
    {
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<ContactPerson> ContactPerson { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetupKey<BankAccount>(modelBuilder);
            SetupKey<ContactPerson>(modelBuilder);
            SetupKey<Vendor>(modelBuilder);
        }

        private void SetupKey<EFEntity>(ModelBuilder modelBuilder) where EFEntity: BaseEntity =>
            modelBuilder.Entity<EFEntity>().HasKey(o => o.Id);

        private string GetConnectionString()
        {
            if(!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<CaseStudyContext>().Build();
            var configuration = new EFCoreConfiguration();
            configurationRoot.GetSection(EFCoreConfiguration.SectionName).Bind(configuration);

            return configuration.ConnectionString;
        }

        private void LaunchDebugger()
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }
    }
}
