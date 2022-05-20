using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace doudizhuServer.Models
{
    public partial class gameContext : DbContext
    {
        public gameContext()
        {
        }

        public gameContext(DbContextOptions<gameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sign> Signs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=game;user=root;password=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Sign>(entity =>
            {
                entity.HasKey(e => new { e.SignTime, e.UserId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("sign");

                entity.Property(e => e.SignTime)
                    .HasColumnType("datetime")
                    .HasComment("签到时间");

                entity.Property(e => e.UserId).HasComment("用户ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasComment("用户ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasComment("邮箱");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasComment("密码");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''")
                    .HasComment("用户名");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
