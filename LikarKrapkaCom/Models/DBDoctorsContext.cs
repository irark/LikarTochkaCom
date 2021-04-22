using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LikarKrapkaCom.Models
{
    public class DBDoctorsContext: DbContext
    {
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public DBDoctorsContext(DbContextOptions<DBDoctorsContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
