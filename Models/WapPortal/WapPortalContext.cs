using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QuizMaster.Models.StaticModels;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class WapPortalContext : DbContext
    {
        public WapPortalContext()
        {
        }

        public WapPortalContext(DbContextOptions<WapPortalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HappyHourMultipleTimeCheck> sp_BkashQuizMasterLiveQuizTimeSelection { get; set; }
        public virtual DbSet<GetLiveQuizVideosDataModel> GetLiveQuizVideosDataModel { get; set; }
        public virtual DbSet<JhotPot> JhotPots { get; set; }
        public virtual DbSet<JhotPotThemeQuestion> JhotPotThemeQuestions { get; set; }
        public virtual DbSet<TblAllTextNotification> TblAllTextNotifications { get; set; }
        public virtual DbSet<TblAppreferal> TblAppreferals { get; set; }
        public virtual DbSet<TblJhotPotAnswer> TblJhotPotAnswers { get; set; }
        //Ratul 
        public virtual DbSet<TblJhotPotAnswerWCup> TblJhotPotAnswersWcup { get; set; }
        //
        public virtual DbSet<TblJhotPotAnswerForSpecialQuiz> TblJhotPotAnswerForSpecialQuizzes { get; set; }
        public virtual DbSet<TblJhotpotPlayCount> TblJhotpotPlayCounts { get; set; }
        public virtual DbSet<TblJhotpotPlayCountForSpecialQuiz> TblJhotpotPlayCountForSpecialQuizzes { get; set; }
        public virtual DbSet<TblLiveQuizVideoQuestionsList> TblLiveQuizVideoQuestionsLists { get; set; }
        public virtual DbSet<TblQpFriendList> TblQpFriendLists { get; set; }
        public virtual DbSet<TblQuestionManagementChild> TblQuestionManagementChildren { get; set; }
        public virtual DbSet<TblQuestionManagementParent> TblQuestionManagementParents { get; set; }
        public virtual DbSet<TblQuizStarQuestionTimer> TblQuizStarQuestionTimers { get; set; }
        public virtual DbSet<TblQuizStarUiContent> TblQuizStarUiContents { get; set; }
        public virtual DbSet<TblQuizStarUiType> TblQuizStarUiTypes { get; set; }
        public virtual DbSet<TblRewardCoinActivity> TblRewardCoinActivities { get; set; }

        public virtual DbSet<QuizMaster.Models.Leaderboard> GameSpecificLeaderboards { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=103.134.68.67;database=WapPortal;user=sa;pwd=adplayVu@1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<JhotPot>(entity =>
            {
                entity.ToTable("JhotPot");

                entity.Property(e => e.Answer).HasMaxLength(255);

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.Option1).HasMaxLength(255);

                entity.Property(e => e.Option2).HasMaxLength(255);

                entity.Property(e => e.Option3).HasMaxLength(255);

                entity.Property(e => e.Question).HasMaxLength(255);
            });

            modelBuilder.Entity<JhotPotThemeQuestion>(entity =>
            {
                entity.ToTable("JhotPot_ThemeQuestionForGuessTheWordKids");

                entity.Property(e => e.Answer).HasMaxLength(255);

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.Option1).HasMaxLength(255);

                entity.Property(e => e.Option2).HasMaxLength(255);

                entity.Property(e => e.Option3).HasMaxLength(255);

                entity.Property(e => e.Question).HasMaxLength(255);

                entity.Property(e => e.QuestionCategory).HasMaxLength(255);

                entity.Property(e => e.Theme).HasMaxLength(255);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UploadBy).HasMaxLength(255);
            });

            modelBuilder.Entity<TblAllTextNotification>(entity =>
            {
                entity.ToTable("tbl_AllTextNOtification");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type).HasMaxLength(20);
            });

            modelBuilder.Entity<TblAppreferal>(entity =>
            {
                entity.ToTable("tbl_Appreferal");

                entity.Property(e => e.App).HasMaxLength(50);

                entity.Property(e => e.InviteFbid).HasMaxLength(50);

                entity.Property(e => e.IsSync).HasDefaultValueSql("((0))");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UserFbId).HasMaxLength(50);
            });

            modelBuilder.Entity<TblJhotPotAnswer>(entity =>
            {
                entity.ToTable("tbl_JhotPotAnswer");

                entity.Property(e => e.Answer).HasMaxLength(50);

                entity.Property(e => e.FbId).HasMaxLength(50);

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblJhotPotAnswerWCup>(entity =>
            {
                entity.ToTable("tbl_JhotPotAnswerWCup");

                entity.Property(e => e.Answer).HasMaxLength(50);

                entity.Property(e => e.FbId).HasMaxLength(50);

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblJhotPotAnswerForSpecialQuiz>(entity =>
            {
                entity.ToTable("tbl_JhotPotAnswerForSpecialQuiz");

                entity.Property(e => e.Answer).HasMaxLength(50);

                entity.Property(e => e.FbId).HasMaxLength(50);

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.ServiceType).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblJhotpotPlayCount>(entity =>
            {
                entity.ToTable("tbl_JhotpotPlayCount");

                entity.Property(e => e.FbId).IsRequired();

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblJhotpotPlayCountForSpecialQuiz>(entity =>
            {
                entity.ToTable("tbl_JhotpotPlayCountForSpecialQuiz");

                entity.Property(e => e.FbId).IsRequired();

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.ServiceType).HasMaxLength(50);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

    

            modelBuilder.Entity<TblLiveQuizVideoQuestionsList>(entity =>
            {
                entity.ToTable("tbl_LiveQuizVideoQuestionsList");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(30, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.GcloudVideoUrl).HasColumnName("GCloudVideoURL");

                entity.Property(e => e.UpdatedTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.VideoCategoryId)
                    .HasMaxLength(250)
                    .HasColumnName("VideoCategory_id");

                entity.Property(e => e.VideoUrl).HasColumnName("VideoURL");
            });

            modelBuilder.Entity<TblQpFriendList>(entity =>
            {
                entity.ToTable("tbl_QpFriendList");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblQuestionManagementChild>(entity =>
            {
                entity.ToTable("tbl_QuestionManagementChild");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(30, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Slno).HasColumnName("SLNo");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuestionManagementParent>(entity =>
            {
                entity.ToTable("tbl_QuestionManagementParent");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(30, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.ScheduledDateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdatedTimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblQuizStarQuestionTimer>(entity =>
            {
                entity.ToTable("tbl_QuizStarQuestionTimer");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Timer).HasMaxLength(50);

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblQuizStarUiContent>(entity =>
            {
                entity.ToTable("Tbl_QuizStar_UI_Content");

                entity.Property(e => e.TimeStamp).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.TitleEnglish).HasMaxLength(200);
            });

            modelBuilder.Entity<TblQuizStarUiType>(entity =>
            {
                entity.ToTable("Tbl_QuizStar_UI_Type");
            });

            modelBuilder.Entity<TblRewardCoinActivity>(entity =>
            {
                entity.ToTable("tbl_RewardCoinActivity");

                entity.Property(e => e.Fbid).HasMaxLength(50);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
