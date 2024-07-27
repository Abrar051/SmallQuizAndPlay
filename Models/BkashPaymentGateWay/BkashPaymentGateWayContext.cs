using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuizMaster.Models.BkashPaymentGateWay
{
    public partial class BkashPaymentGateWayContext : DbContext
    {
        public BkashPaymentGateWayContext()
        {
        }

        public BkashPaymentGateWayContext(DbContextOptions<BkashPaymentGateWayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BkashSecret> BkashSecrets { get; set; }
        public virtual DbSet<SessionDatum> SessionData { get; set; }
        public virtual DbSet<SubscriptionRequestData> SubscriptionRequestDatas { get; set; }
        public virtual DbSet<SubscriptionRequestDatasV3> SubscriptionRequestDatasV3s { get; set; }
        public virtual DbSet<WebHookSubscriptionDatasV3> WebHookSubscriptionDatasV3s { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=103.134.68.67;database=BkashPaymentGateWay;user=sa;pwd=adplayVu@1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BkashSecret>(entity =>
            {
                entity.ToTable("BkashSecret");

                entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.AppNappPassWord)
                    .HasMaxLength(500)
                    .HasColumnName("AppNAppPassWord");

                entity.Property(e => e.AppNappUserName)
                    .HasMaxLength(500)
                    .HasColumnName("AppNAppUserName");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstPaymentIncludedInCycle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaxCapRequired)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantShortCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PayerType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.SubscriptionType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SessionDatum>(entity =>
            {
                entity.Property(e => e.Msisdn)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubscriptionRequestData>(entity =>
            {
                entity.Property(e => e.Ckey).HasColumnName("CKEY");

                entity.Property(e => e.HsDim).HasColumnName("HS_DIM");

                entity.Property(e => e.HsManufac).HasColumnName("HS_MANUFAC");

                entity.Property(e => e.HsMod).HasColumnName("HS_MOD");

                entity.Property(e => e.HsOs).HasColumnName("HS_OS");

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Msisdn).HasColumnName("MSISDN");

                entity.Property(e => e.XappKey).HasColumnName("XAppKey");
            });

            modelBuilder.Entity<SubscriptionRequestDatasV3>(entity =>
            {
                entity.ToTable("SubscriptionRequestDatasV3");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(37, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ckey).HasColumnName("CKEY");

                entity.Property(e => e.HsDim).HasColumnName("HS_DIM");

                entity.Property(e => e.HsManufac).HasColumnName("HS_MANUFAC");

                entity.Property(e => e.HsMod).HasColumnName("HS_MOD");

                entity.Property(e => e.HsOs).HasColumnName("HS_OS");

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Msisdn).HasColumnName("MSISDN");

                entity.Property(e => e.XappKey).HasColumnName("XAppKey");
            });

            modelBuilder.Entity<WebHookSubscriptionDatasV3>(entity =>
            {
                entity.ToTable("WebHookSubscriptionDatasV3");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(37, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CancelledBy).HasColumnName("cancelledBy");

                entity.Property(e => e.DueDate).HasColumnName("dueDate");

                entity.Property(e => e.FirstPayment).HasColumnName("firstPayment");

                entity.Property(e => e.Message).HasColumnName("message");

                entity.Property(e => e.NextPaymentDate).HasColumnName("nextPaymentDate");

                entity.Property(e => e.PaymentId).HasColumnName("paymentId");

                entity.Property(e => e.PaymentStatus).HasColumnName("paymentStatus");

                entity.Property(e => e.SubscriptionId).HasColumnName("subscriptionId");

                entity.Property(e => e.SubscriptionRequestId).HasColumnName("subscriptionRequestId");

                entity.Property(e => e.SubscriptionStatus).HasColumnName("subscriptionStatus");

                entity.Property(e => e.TimeStamp).HasColumnName("timeStamp");

                entity.Property(e => e.TrxDate).HasColumnName("trxDate");

                entity.Property(e => e.TrxId).HasColumnName("trxId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
