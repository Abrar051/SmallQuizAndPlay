using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuizMaster.Data.QuizMasterLiveQuizLog
{
    public partial class QuizMasterLiveQuizLogContext : DbContext
    {
        public QuizMasterLiveQuizLogContext()
        {
        }

        public QuizMasterLiveQuizLogContext(DbContextOptions<QuizMasterLiveQuizLogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblQuizMasterLiveQuizLiveLog> TblQuizMasterLiveQuizLiveLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=103.134.68.67;database=QuizMasterLiveQuizLog;user=sa;pwd=adplayVu@1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblQuizMasterLiveQuizLiveLog>(entity =>
            {
                entity.ToTable("tbl_QuizMasterLiveQuizLiveLog");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Ckey).HasMaxLength(50);

                entity.Property(e => e.ExtraPram1).HasMaxLength(250);

                entity.Property(e => e.ExtraPram2).HasMaxLength(250);

                entity.Property(e => e.ExtraPram3).HasMaxLength(250);

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(50)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.SourceUrl).HasColumnName("SourceURL");

                entity.Property(e => e.ThemId)
                    .HasMaxLength(250)
                    .HasColumnName("Them_Id");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
