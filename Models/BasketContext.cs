using System;
using System.Collections.Generic;
using Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuizMaster.Models
{
    public partial class BasketContext : DbContext
    {
        public BasketContext()
        {
        }

        public BasketContext(DbContextOptions<BasketContext> options)
            : base(options)
        {
        }
        public virtual DbSet<LiveVideoQuizInformationModel> LiveVideoQuizInformationModels { get; set; }
        public virtual DbSet<GeneralProcedureReturnType> GeneralProcedureReturnTypes { get; set; }
        public virtual DbSet<sp_HappyHourBannerSelection> sp_HappyHourBannerSelections { get; set; }
        public virtual DbSet<QuizQuestionCategorySelection> QuizQuestionCategorySelections { get; set; }
        public virtual DbSet<CheckoutUserData> CheckoutUserDatas { get; set; }
        public virtual DbSet<sp_Bkash_Result_Jhotpot_Result> sp_Bkash_Result_Jhotpot_Results { get; set; }
        public virtual DbSet<Sp_PushNotificationForQuizMasterWinner> Sp_PushNotificationForQuizMasterWinners { get; set; }
        public virtual DbSet<TblAccessBdtubeAll> TblAccessBdtubeAlls { get; set; }
        public virtual DbSet<TblAllLogPushNotificationMessageSending> TblAllLogPushNotificationMessageSendings { get; set; }
        public virtual DbSet<TblAllLogPushNotificationTopicWise> TblAllLogPushNotificationTopicWises { get; set; }
        public virtual DbSet<TblAllLogPushNotificationV2> TblAllLogPushNotificationV2s { get; set; }
        public virtual DbSet<TblAllLogQuizBkash> TblAllLogQuizBkashes { get; set; }
        public virtual DbSet<TblDailyAppResponse> TblDailyAppResponses { get; set; }
        public virtual DbSet<TblDailyAppResponseWeb> TblDailyAppResponseWebs { get; set; }
        public virtual DbSet<TblDailyLogin> TblDailyLogins { get; set; }
        public virtual DbSet<TblDailyPlayStatus> TblDailyPlayStatuses { get; set; }
        public virtual DbSet<TblDailyQuizStartTime> TblDailyQuizStartTimes { get; set; }
        public virtual DbSet<TblFreePlayStatus> TblFreePlayStatuses { get; set; }
        public virtual DbSet<TblLikeViewPortal> TblLikeViewPortals { get; set; }
        public virtual DbSet<TblMsisdnQuizBkash> TblMsisdnQuizBkashes { get; set; }
        public virtual DbSet<TblPushNotificationUserBase> TblPushNotificationUserBases { get; set; }
        public virtual DbSet<TblPuzzleWordLib> TblPuzzleWordLibs { get; set; }

        public virtual DbSet<WordMixUpQuestionAnswerCombo> WordMixUpQuestionAnswerCombos { get; set; }
        public virtual DbSet<TblQpTerm> TblQpTerms { get; set; }
        //public virtual DbSet<TblQpTermAppInApp> TblQpTermAppInApps { get; set; }
        public virtual DbSet<TblQsdlquizEnroll> TblQsdlquizEnrolls { get; set; }
        public virtual DbSet<TblQuizMasterUnsubscriptionNew> TblQuizMasterUnsubscriptionNews { get; set; }
        public virtual DbSet<TblQuizMasterUserInfo> TblQuizMasterUserInfos { get; set; }
        public virtual DbSet<TblQuizMasterWordPuzzleGame> TblQuizMasterWordPuzzleGames { get; set; }
        public virtual DbSet<TblQuizMasterWordPuzzleGamePlayerNotPlayed> TblQuizMasterWordPuzzleGamePlayerNotPlayed { get; set; }
        public virtual DbSet<TblQuizStarLiveVideoQuizSchedule> TblQuizStarLiveVideoQuizSchedules { get; set; }
        public virtual DbSet<TblQuizStarLiveVideoQuizScheduleDetail> TblQuizStarLiveVideoQuizScheduleDetails { get; set; }
        public virtual DbSet<TblQuizStarMoneyClaim> TblQuizStarMoneyClaims { get; set; }
        public virtual DbSet<TblQuizStarSubscription> TblQuizStarSubscriptions { get; set; }
        public virtual DbSet<TblQuizStarSubscriptionAppInApp> TblQuizStarSubscriptionAppInApps { get; set; }
        public virtual DbSet<TblQuizStarSubscriptionTournament1> TblQuizStarSubscriptionTournament1s { get; set; }
        public virtual DbSet<TblQuizStarSubscriptionTournament2> TblQuizStarSubscriptionTournament2s { get; set; }
        public virtual DbSet<Breaktimequiz> Breaktimequizzes { get; set; }
       

        public virtual DbSet<BreaktimequizAnswer> BreaktimequizAnswers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=103.134.68.67;database=Basket;user=sa;pwd=adplayVu@1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAccessBdtubeAll>(entity =>
            {
                entity.ToTable("tbl_Access_BDTUBE_ALL");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APN");

                entity.Property(e => e.DeviceIp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Device_IP");

                entity.Property(e => e.HsDim)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HS_DIM");

                entity.Property(e => e.HsManufacturer)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MANUFACTURER");

                entity.Property(e => e.HsModel)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MODEL");

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.Os)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OS");

                entity.Property(e => e.PortalFullnShort)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PORTAL_FULLnSHORT");

                entity.Property(e => e.ServiceRequest)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_REQUEST");

                entity.Property(e => e.SourceUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SOURCE_URL");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UaprofileUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UAPROFILE_URL");
            });

            modelBuilder.Entity<TblAllLogPushNotificationMessageSending>(entity =>
            {
                entity.ToTable("tbl_all_log_PushNotificationMessageSending");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(35, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ReScheduleTime).HasColumnType("datetime");

                entity.Property(e => e.ServiceName).HasMaxLength(100);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP");
            });

            modelBuilder.Entity<TblAllLogPushNotificationTopicWise>(entity =>
            {
                entity.ToTable("tbl_all_log_PushNotificationTopicWise");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(35, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ServiceName).HasMaxLength(100);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP");
            });

            modelBuilder.Entity<TblAllLogPushNotificationV2>(entity =>
            {
                entity.ToTable("tbl_all_log_PushNotificationV2");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(35, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ServiceName).HasMaxLength(100);

                entity.Property(e => e.SourceUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SOURCE_URL");

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP");
            });

            modelBuilder.Entity<TblAllLogQuizBkash>(entity =>
            {
                entity.ToTable("tbl_all_log_Quiz_Bkash");

                entity.HasIndex(e => new { e.SourceUrl, e.ThemeId, e.TimeStamp }, "index_tbl_all_log_Quiz_Bkash");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APN");

                entity.Property(e => e.CamName)
                    .HasMaxLength(50)
                    .HasColumnName("camName");

                entity.Property(e => e.Ckey)
                    .HasMaxLength(50)
                    .HasColumnName("ckey");

                entity.Property(e => e.Confirmbutton)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("confirmbutton");

                entity.Property(e => e.DeviceIp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Device_IP");

                entity.Property(e => e.FirstPageSession)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first page session");

                entity.Property(e => e.HsDim)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HS_DIM");

                entity.Property(e => e.HsManufacturer)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MANUFACTURER");

                entity.Property(e => e.HsModel)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MODEL");

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.Numbersubit)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("numbersubit");

                entity.Property(e => e.Os)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OS");

                entity.Property(e => e.PortalFullnShort)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PORTAL_FULLnSHORT");

                entity.Property(e => e.SecondPageSession)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("second page session");

                entity.Property(e => e.ServiceRequest)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_REQUEST");

                entity.Property(e => e.SourceUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SOURCE_URL");

                entity.Property(e => e.ThemeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Theme_id");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP");

                entity.Property(e => e.UaprofileUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UAPROFILE_URL");
            });

            modelBuilder.Entity<TblDailyAppResponse>(entity =>
            {
                entity.ToTable("tbl_DailyAppResponse");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Android')");
            });

            modelBuilder.Entity<TblDailyAppResponseWeb>(entity =>
            {
                entity.ToTable("tbl_DailyAppResponseWeb");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblDailyLogin>(entity =>
            {
                entity.ToTable("Tbl_DailyLogin");

                entity.Property(e => e.IsLogin).HasColumnName("isLogin");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblDailyPlayStatus>(entity =>
            {
                entity.ToTable("Tbl_DailyPlayStatus");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Type).HasMaxLength(20);
            });

            modelBuilder.Entity<TblDailyQuizStartTime>(entity =>
            {
                entity.ToTable("tbl_DailyQuizStartTime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblFreePlayStatus>(entity =>
            {
                entity.ToTable("Tbl_FreePlayStatus");

                entity.Property(e => e.Fbid).HasColumnName("fbid");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblLikeViewPortal>(entity =>
            {
                entity.ToTable("tbl_like_view_portal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contentcode).HasColumnName("contentcode");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblMsisdnQuizBkash>(entity =>
            {
                entity.ToTable("tbl_msisdn_Quiz_Bkash");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APN");

                entity.Property(e => e.DeviceIp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Device_IP");

                entity.Property(e => e.HsDim)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HS_DIM");

                entity.Property(e => e.HsManufacturer)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MANUFACTURER");

                entity.Property(e => e.HsModel)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HS_MODEL");

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.Os)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OS");

                entity.Property(e => e.PortalFullnShort)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PORTAL_FULLnSHORT");

                entity.Property(e => e.ServiceRequest)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_REQUEST");

                entity.Property(e => e.SourceUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SOURCE_URL");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("TIME_STAMP")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UaprofileUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UAPROFILE_URL");
            });

            modelBuilder.Entity<TblPushNotificationUserBase>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_PushNotificationUserBase");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TblPuzzleWordLib>(entity =>
            {
                entity.ToTable("tbl_PuzzleWordLib");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<WordMixUpQuestionAnswerCombo>(entity =>
            {
                entity.ToTable("tbl_WordMixUpQuestionAnswerCombo");

                entity.Property(e => e.Question)
                .HasMaxLength(255)
                .HasColumnName("Question");

                entity.Property(e => e.Words)
                .HasMaxLength(50)
                .HasColumnName("Words");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });


            modelBuilder.Entity<TblQpTerm>(entity =>
            {
                entity.ToTable("tbl_QpTermsAppInApp");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQsdlquizEnroll>(entity =>
            {
                entity.ToTable("Tbl_QSDLQuizEnroll");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizMasterUnsubscriptionNew>(entity =>
            {
                entity.ToTable("tbl_QuizMasterUnsubscriptionNew");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.UnsubscriptionNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<TblQuizMasterUserInfo>(entity =>
            {
                entity.ToTable("Tbl_QuizMaster_UserInfo");

                entity.Property(e => e.AppInAppTokenTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.Ckey).HasColumnName("CKey");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(500)
                    .HasColumnName("imageName");

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(15)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizMasterWordPuzzleGame>(entity =>
            {
                entity.ToTable("Tbl_QuizMaster_WordPuzzleGames");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(24, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.GameName).HasMaxLength(150);

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(15)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.TimeCount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizMasterWordPuzzleGamePlayerNotPlayed>(entity =>
            {
                entity.ToTable("Tbl_QuizMaster_WordPuzzleGamesPlayerNotPlayed");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(24, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.GameName).HasMaxLength(150);

                entity.Property(e => e.Msisdn)
                    .HasMaxLength(15)
                    .HasColumnName("MSISDN");

                entity.Property(e => e.TimeCount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizStarLiveVideoQuizSchedule>(entity =>
            {
                entity.ToTable("tbl_QuizStarLiveVideoQuizSchedule");

                entity.Property(e => e.Duration).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.ScheduledDateTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizStarLiveVideoQuizScheduleDetail>(entity =>
            {
                entity.ToTable("tbl_QuizStarLiveVideoQuizScheduleDetails");

                entity.Property(e => e.LiveQuizUid).HasColumnName("LiveQuizUId");

                entity.Property(e => e.QuestionId).HasColumnType("numeric(25, 0)");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.LiveQuizU)
                    .WithMany(p => p.TblQuizStarLiveVideoQuizScheduleDetails)
                    .HasForeignKey(d => d.LiveQuizUid)
                    .HasConstraintName("FK__tbl_QuizS__LiveQ__483BA0F8");
            });

            modelBuilder.Entity<TblQuizStarMoneyClaim>(entity =>
            {
                entity.ToTable("Tbl_QuizStar_MoneyClaim");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });
            modelBuilder.Entity<Breaktimequiz>(entity =>
            {
                entity.ToTable("tbl_Breaktimequiz");



                entity.Property(e => e.Date).HasColumnName("Date");



                entity.Property(e => e.Question).HasColumnName("Question");
            });
            modelBuilder.Entity<BreaktimequizAnswer>(entity =>
            {
                entity.ToTable("tbl_BreaktimequizAnswer");
                entity.Property(e => e.FbId).HasColumnName("FbId");
                entity.Property(e => e.Answer).HasColumnName("Answer");
                entity.Property(e => e.Round).HasColumnName("Round");
          

            });

            modelBuilder.Entity<TblQuizStarSubscription>(entity =>
            {
                entity.ToTable("tbl_QuizStarSubscription");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");

                entity.Property(e => e.LastChargeDate).HasColumnType("datetime");

                entity.Property(e => e.LastLimitDate).HasColumnType("datetime");

                entity.Property(e => e.ReactivationDate).HasColumnType("datetime");

                entity.Property(e => e.RegDate).HasColumnType("datetime");

                entity.Property(e => e.SubscriptionRequestId).HasMaxLength(500);
              

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.CKEY).HasMaxLength(50);

            });

            modelBuilder.Entity<TblQuizStarSubscriptionAppInApp>(entity =>
            {
                entity.ToTable("tbl_QuizStarSubscriptionAppInApp");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");

                entity.Property(e => e.LastChargeDate).HasColumnType("datetime");

                entity.Property(e => e.LastLimitDate).HasColumnType("datetime");

                entity.Property(e => e.ReactivationDate).HasColumnType("datetime");

                entity.Property(e => e.RegDate).HasColumnType("datetime");

                entity.Property(e => e.SubscriptionRequestId).HasMaxLength(500);


                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.CKEY).HasMaxLength(50);

            });

            modelBuilder.Entity<TblQuizStarSubscriptionTournament1>(entity =>
            {
                entity.ToTable("tbl_QuizStarSubscriptionTournament1");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");

                entity.Property(e => e.LastChargeDate).HasColumnType("datetime");

                entity.Property(e => e.LastLimitDate).HasColumnType("datetime");

                entity.Property(e => e.ReactivationDate).HasColumnType("datetime");

                entity.Property(e => e.RegDate).HasColumnType("datetime");

                entity.Property(e => e.SubscriptionRequestId).HasMaxLength(500);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
                entity.Property(e => e.CKEY).HasMaxLength(50);
            });

            modelBuilder.Entity<TblQuizStarSubscriptionTournament2>(entity =>
            {
                entity.ToTable("tbl_QuizStarSubscriptionTournament2");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");

                entity.Property(e => e.LastChargeDate).HasColumnType("datetime");

                entity.Property(e => e.LastLimitDate).HasColumnType("datetime");

                entity.Property(e => e.ReactivationDate).HasColumnType("datetime");

                entity.Property(e => e.RegDate).HasColumnType("datetime");

                entity.Property(e => e.SubscriptionRequestId).HasMaxLength(500);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
                entity.Property(e => e.CKEY).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
