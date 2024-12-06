using EMS.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Data
{
    public class EMSDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EmpAuth> EmpAuths { get; set; }
        public DbSet<EmpBank> EmpBanks { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Banks> Banks { get; set; }
        public DbSet<LoansandAdvance> LoansandAdvances { get; set; }
        public DbSet<LnABalance> LnABalances { get; set; }
        public EMSDbContext(DbContextOptions<EMSDbContext> options) : base(options) 
        {

        }
 	protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LnABalance>()
                .HasIndex(l => new { l.EmpID, l.MonthYear, l.TotalDeduction })
                .IsUnique();

        }
        public DbSet<EMS.Models.Uploads> Uploads { get; set; } = default!;

    }
}
