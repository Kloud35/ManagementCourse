﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ManagementCourse.Models;

#nullable disable

namespace ManagementCourse.Models.Context
{
    public partial class RTCContext : DbContext
    {
        public RTCContext()
        {
        }

        public RTCContext(DbContextOptions<RTCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseAnswer> CourseAnswers { get; set; }
        public virtual DbSet<CourseCatalog> CourseCatalogs { get; set; }
        public virtual DbSet<CourseExam> CourseExams { get; set; }
        public virtual DbSet<CourseExamResult> CourseExamResults { get; set; }
        public virtual DbSet<CourseExamResultDetail> CourseExamResultDetails { get; set; }
        public virtual DbSet<CourseFile> CourseFiles { get; set; }
        public virtual DbSet<CourseLesson> CourseLessons { get; set; }
        public virtual DbSet<CourseLessonHistory> CourseLessonHistories { get; set; }
        public virtual DbSet<CourseQuestion> CourseQuestions { get; set; }
        public virtual DbSet<CourseRightAnswer> CourseRightAnswers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DESKTOP-ABEN704\\SQLEXPRESS;database=RTC;User Id = sa; Password=1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CourseCatalogId).HasColumnName("CourseCatalogID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCourseId).HasColumnName("FileCourseID");

                entity.Property(e => e.Instructor).HasMaxLength(200);

                entity.Property(e => e.NameCourse).HasMaxLength(300);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseCatalog>(entity =>
            {
                entity.ToTable("CourseCatalog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseExam>(entity =>
            {
                entity.ToTable("CourseExam");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.CodeExam)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseExamResult>(entity =>
            {
                entity.ToTable("CourseExamResult");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PercentageCorrect).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseExamResultDetail>(entity =>
            {
                entity.ToTable("CourseExamResultDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseFile>(entity =>
            {
                entity.ToTable("CourseFile");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LessonId).HasColumnName("LessonID");

                entity.Property(e => e.NameFile).HasMaxLength(150);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseLesson>(entity =>
            {
                entity.ToTable("CourseLesson");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCourseId).HasColumnName("FileCourseID");

                entity.Property(e => e.LessonTitle).HasMaxLength(400);

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UrlPdf).HasColumnName("UrlPDF");

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("VideoURL");
            });

            modelBuilder.Entity<CourseLessonHistory>(entity =>
            {
                entity.ToTable("CourseLessonHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ViewDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseQuestion>(entity =>
            {
                entity.ToTable("CourseQuestion");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CheckInput).HasComment("1: có 1 đáp án đúng; 2: Có nhiều đáp án đúng");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourseRightAnswer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CourseAnswerId).HasColumnName("CourseAnswerID");

                entity.Property(e => e.CourseQuestionId).HasColumnName("CourseQuestionID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.IsShowHotline).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Pid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PId");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.LoginName, "Index_Users_LoginName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BankAccount).HasMaxLength(250);

                entity.Property(e => e.Bhxh)
                    .HasMaxLength(250)
                    .HasColumnName("BHXH");

                entity.Property(e => e.Bhyt)
                    .HasMaxLength(250)
                    .HasColumnName("BHYT");

                entity.Property(e => e.BirthOfDate).HasColumnType("datetime");

                entity.Property(e => e.Cmtnd)
                    .HasMaxLength(250)
                    .HasColumnName("CMTND");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Communication).HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.EmailCom).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.HandPhone).HasMaxLength(100);

                entity.Property(e => e.HomeAddress).HasMaxLength(100);

                entity.Property(e => e.ImagePath).HasColumnType("ntext");

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsAdminSale).HasColumnName("isAdminSale");

                entity.Property(e => e.IsSetupFunction).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobDescription).HasMaxLength(200);

                entity.Property(e => e.Leader).HasDefaultValueSql("((0))");

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.MainViewId).HasColumnName("MainViewID");

                entity.Property(e => e.Mst)
                    .HasMaxLength(250)
                    .HasColumnName("MST");

                entity.Property(e => e.PassExpireDate).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasMaxLength(250);

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.Qualifications).HasMaxLength(250);

                entity.Property(e => e.Resident).HasMaxLength(100);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.StartWorking).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Trạng thái hoạt động 0: hoạt động, 1: ngừng hoạt động");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.Telephone).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserGroupId).HasColumnName("UserGroupID");

                entity.Property(e => e.UserGroupSxid).HasColumnName("UserGroupSXID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
