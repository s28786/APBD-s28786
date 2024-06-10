using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task9.Models;

namespace Task9.Context
{
    public partial class s28786DbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public s28786DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public s28786DbContext(DbContextOptions<s28786DbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prescription_Medicament>().HasKey(e => new { e.IdMedicament, e.IdPrescription });

            modelBuilder.Entity<Prescription>().HasKey(e => e.IdPrescription);

            modelBuilder.Entity<Prescription>().HasMany(e => e.Prescription_Medicaments)
                   .WithOne(e => e.Prescription)
                   .HasForeignKey(e => e.IdPrescription);

            modelBuilder.Entity<Medicament>().HasKey(e => e.IdMedicament);
            modelBuilder.Entity<Medicament>().HasMany(e => e.Prescription_Medicaments)
                    .WithOne(e => e.Medicament)
                    .HasForeignKey(e => e.IdMedicament);

            modelBuilder.Entity<Doctor>().HasKey(e => e.IdDoctor);
            modelBuilder.Entity<Doctor>().HasMany(e => e.Prescriptions)
                    .WithOne(e => e.Doctor)
                    .HasForeignKey(e => e.IdDoctor);

            modelBuilder.Entity<Patient>().HasKey(e => e.IdPatient);
            modelBuilder.Entity<Patient>().HasMany(e => e.Prescriptions)
                    .WithOne(e => e.Patient)
                    .HasForeignKey(e => e.IdPatient);

            modelBuilder.Entity<AppUser>().HasKey(e => e.IdUser);
        }
    }
}