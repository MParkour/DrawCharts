using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Data.Sqlite;
using System;

namespace Models
{
    public class DrawChartsContext : DbContext
    {
        public DrawChartsContext(DbContextOptions<DrawChartsContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=DrawCharts.db;");
            // optionsBuilder.UseSqlite("Data Source=DrawCharts.db;", x => x.SuppressForeignKeyEnforcement());                                    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //User & User_Temp
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(p => p.UserName).IsUnique();
                entity.HasMany(p => p.User_Temps).WithOne(p => p.User).HasForeignKey(p => p.UserID);
            });

            //Template & User_Temp
            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasIndex(p => p.Title).IsUnique();
                entity.HasMany(p => p.User_Temps).WithOne(p => p.Template).HasForeignKey(p => p.TemplateID);
            });

            //User & Login_Log
            modelBuilder.Entity<Login_Log>(entity =>
            {
                entity.HasOne(p => p.User).WithMany(p => p.Login_Logs).HasForeignKey(p => p.UserID);
            });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<Login_Log> Login_Logs { get; set; }
        public virtual DbSet<Faild_Log> Faild_Logs { get; set; }
        public virtual DbSet<User_Temp> User_Temps { get; set; }
    }
}