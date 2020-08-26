using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GovtechDBLib.Models
{
    public partial class GovtechHackathonContext : DbContext
    {

        public GovtechHackathonContext(DbContextOptions<GovtechHackathonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Applicant> Applicant { get; set; }
        public virtual DbSet<Auditor> Auditor { get; set; }
        public virtual DbSet<AuditorNote> AuditorNote { get; set; }
        public virtual DbSet<BusinessIdea> BusinessIdea { get; set; }
        public virtual DbSet<CardDescriptions> CardDescriptions { get; set; }
        public virtual DbSet<CaseAction> CaseAction { get; set; }
        public virtual DbSet<CaseAdjudicatorReferalReason> CaseAdjudicatorReferalReason { get; set; }
        public virtual DbSet<CaseAdminApproved> CaseAdminApproved { get; set; }
        public virtual DbSet<CaseAssignments> CaseAssignments { get; set; }
        public virtual DbSet<CaseCategory> CaseCategory { get; set; }
        public virtual DbSet<CaseCategoryScore> CaseCategoryScore { get; set; }
        public virtual DbSet<CaseComment> CaseComment { get; set; }
        public virtual DbSet<CaseInformation> CaseInformation { get; set; }
        public virtual DbSet<CaseInvestorInterested> CaseInvestorInterested { get; set; }
        public virtual DbSet<CaseOutcomes> CaseOutcomes { get; set; }
        public virtual DbSet<CaseStatus> CaseStatus { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<ConfigSettings> ConfigSettings { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Countries20140629> Countries20140629 { get; set; }
        public virtual DbSet<Dates> Dates { get; set; }
        public virtual DbSet<Genders> Genders { get; set; }
        public virtual DbSet<General> General { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Investor> Investor { get; set; }
        public virtual DbSet<InvestorIndustry> InvestorIndustry { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Outcomes> Outcomes { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<Purpose> Purpose { get; set; }
        public virtual DbSet<Race> Race { get; set; }
        public virtual DbSet<ScoringCategories> ScoringCategories { get; set; }
        public virtual DbSet<ScoringCategoryType> ScoringCategoryType { get; set; }
        public virtual DbSet<SituationOptions> SituationOptions { get; set; }
        public virtual DbSet<SituationStatements> SituationStatements { get; set; }
        public virtual DbSet<StudentDetails> StudentDetails { get; set; }
        public virtual DbSet<TeamMember> TeamMember { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserComments> UserComments { get; set; }
        public virtual DbSet<UserLevel> UserLevel { get; set; }
        public virtual DbSet<UsersInGroup> UsersInGroup { get; set; }
        public virtual DbSet<VwCaseGroups> VwCaseGroups { get; set; }
        public virtual DbSet<VwUserGroups> VwUserGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_User");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.EmailAddress).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.FkGenderId).HasColumnName("fkGenderID");

                entity.Property(e => e.FkHomeLanguageId).HasColumnName("fkHomeLanguageID");

                entity.Property(e => e.FkProvinceId).HasColumnName("fkProvinceID");

                entity.Property(e => e.FkRaceId).HasColumnName("fkRaceID");

                entity.Property(e => e.Idnumber)
                    .IsRequired()
                    .HasColumnName("IDNumber")
                    .HasMaxLength(13)
                    .IsFixedLength();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.LinkedIn).IsUnicode(false);

                entity.Property(e => e.Twitter).IsUnicode(false);

                entity.Property(e => e.ZipCode).HasColumnName("Zip Code");

                entity.HasOne(d => d.FkHomeLanguage)
                    .WithMany(p => p.Applicant)
                    .HasForeignKey(d => d.FkHomeLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRegistration_Languages");

                entity.HasOne(d => d.FkRace)
                    .WithMany(p => p.Applicant)
                    .HasForeignKey(d => d.FkRaceId)
                    .HasConstraintName("FK_UserRegistration_Race");
            });

            modelBuilder.Entity<Auditor>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.ContactNo).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name");

                entity.Property(e => e.Surname).IsRequired();
            });

            modelBuilder.Entity<AuditorNote>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkAuditorId).HasColumnName("fkAuditorID");

                entity.Property(e => e.Note).IsRequired();

                entity.Property(e => e.NoteDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAuditor)
                    .WithMany(p => p.AuditorNote)
                    .HasForeignKey(d => d.FkAuditorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditorNote_User");
            });

            modelBuilder.Entity<BusinessIdea>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.How).IsRequired();

                entity.Property(e => e.Impact).IsRequired();

                entity.Property(e => e.WhatProblem)
                    .IsRequired()
                    .HasColumnName("What Problem");

                entity.Property(e => e.Who).IsRequired();
            });

            modelBuilder.Entity<CardDescriptions>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_Table_1");

                entity.Property(e => e.PkId).HasColumnName("pkID");
            });

            modelBuilder.Entity<CaseAction>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_CaseActions");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FkAdjudicatorId).HasColumnName("fkAdjudicatorID");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.HasOne(d => d.FkAdjudicator)
                    .WithMany(p => p.CaseAction)
                    .HasForeignKey(d => d.FkAdjudicatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseActions_User");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseAction)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseActions_CaseInformation");
            });

            modelBuilder.Entity<CaseAdjudicatorReferalReason>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.Reason).IsRequired();

                entity.Property(e => e.ResubmissionDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseAdjudicatorReferalReason)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseAdjudicatorReferalReason_CaseInformation");
            });

            modelBuilder.Entity<CaseAdminApproved>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_CaseRound1ApproveRejected");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseAdminApproved)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseAdminApproved_CaseInformation");
            });

            modelBuilder.Entity<CaseAssignments>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkGroupId).HasColumnName("fkGroupID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseAssignments)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseAssignments_CaseInformation");

                entity.HasOne(d => d.FkGroup)
                    .WithMany(p => p.CaseAssignments)
                    .HasForeignKey(d => d.FkGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseAssignments_Group");
            });

            modelBuilder.Entity<CaseCategory>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<CaseCategoryScore>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_CaseCatagoryScore");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkCategoryId).HasColumnName("fkCategoryID");

                entity.Property(e => e.FkUserId).HasColumnName("fkUserID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseCategoryScore)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseCategoryScore_CaseInformation");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.CaseCategoryScore)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseCategoryScore_ScoringCategories");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.CaseCategoryScore)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseCategoryScore_User");
            });

            modelBuilder.Entity<CaseComment>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DateStamp).HasColumnType("datetime");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkUserId).HasColumnName("fkUserID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseComment)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseComment_CaseInformation");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.CaseComment)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseComment_User");
            });

            modelBuilder.Entity<CaseInformation>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkBusinessIdeaId).HasColumnName("fkBusinessIdeaID");

                entity.Property(e => e.FkCategoryId).HasColumnName("fkCategoryID");

                entity.Property(e => e.FkCompanyId).HasColumnName("fkCompanyID");

                entity.Property(e => e.FkPurposeId).HasColumnName("fkPurposeID");

                entity.Property(e => e.FkSituationOptionId).HasColumnName("fkSituationOptionID");

                entity.Property(e => e.FkSituationStatementId).HasColumnName("fkSituationStatementID");

                entity.Property(e => e.FkStatusId).HasColumnName("fkStatusID");

                entity.Property(e => e.FkStudentInfoId).HasColumnName("fkStudentInfoID");

                entity.Property(e => e.FkUserId).HasColumnName("fkUserID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.SubmittedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkBusinessIdea)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkBusinessIdeaId)
                    .HasConstraintName("FK_CaseInformation_BusinessIdea");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkCategoryId)
                    .HasConstraintName("FK_CaseInformation_CaseCategory");

                entity.HasOne(d => d.FkCompany)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkCompanyId)
                    .HasConstraintName("FK_CaseInformation_Company");

                entity.HasOne(d => d.FkPurpose)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkPurposeId)
                    .HasConstraintName("FK_CaseInformation_Purpose");

                entity.HasOne(d => d.FkSituationOption)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkSituationOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInformation_SituationOptions");

                entity.HasOne(d => d.FkSituationStatement)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkSituationStatementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInformation_SituationStatements");

                entity.HasOne(d => d.FkStatus)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInformation_CaseStatus");

                entity.HasOne(d => d.FkStudentInfo)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkStudentInfoId)
                    .HasConstraintName("FK_CaseInformation_StudentDetails");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.CaseInformation)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInformation_UserRegistration");
            });

            modelBuilder.Entity<CaseInvestorInterested>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkInvestorId).HasColumnName("fkInvestorID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseInvestorInterested)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInvestorInterested_CaseInformation");

                entity.HasOne(d => d.FkInvestor)
                    .WithMany(p => p.CaseInvestorInterested)
                    .HasForeignKey(d => d.FkInvestorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseInvestorInterested_Investor");
            });

            modelBuilder.Entity<CaseOutcomes>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_CaseOutComes");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkOutcomeId).HasColumnName("fkOutcomeID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.CaseOutcomes)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseOutcomes_CaseInformation");

                entity.HasOne(d => d.FkOutcome)
                    .WithMany(p => p.CaseOutcomes)
                    .HasForeignKey(d => d.FkOutcomeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseOutcomes_Outcomes");
            });

            modelBuilder.Entity<CaseStatus>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_Company_Info");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.City).IsRequired();

                entity.Property(e => e.FkProvinceId).HasColumnName("fkProvinceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.FkProvince)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.FkProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_Provinces");
            });

            modelBuilder.Entity<ConfigSettings>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Countries20140629>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("countries-20140629");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishName)
                    .IsRequired()
                    .HasColumnName("English_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasColumnName("French_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Dates>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DateName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PkId)
                    .HasColumnName("pkID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Genders>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId)
                    .HasColumnName("pkID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<General>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("PkID");

                entity.Property(e => e.PrivacyPolicy)
                    .IsRequired()
                    .HasColumnName("Privacy_Policy");

                entity.Property(e => e.TermsConditions)
                    .IsRequired()
                    .HasColumnName("Terms_Conditions");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Investor>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FkIndustryId).HasColumnName("fkIndustryID");

                entity.Property(e => e.FkProvinceId).HasColumnName("fkProvinceID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.FkIndustry)
                    .WithMany(p => p.Investor)
                    .HasForeignKey(d => d.FkIndustryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Investor_InvestorIndustry");

                entity.HasOne(d => d.FkProvince)
                    .WithMany(p => p.Investor)
                    .HasForeignKey(d => d.FkProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Investor_Provinces");
            });

            modelBuilder.Entity<InvestorIndustry>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Language).IsRequired();
            });

            modelBuilder.Entity<Outcomes>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");
            });

            modelBuilder.Entity<Provinces>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Purpose>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.CoreValues)
                    .IsRequired()
                    .HasColumnName("Core Values");

                entity.Property(e => e.Motivation).IsRequired();
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ScoringCategories>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FkTypeId).HasColumnName("fkTypeID");

                entity.Property(e => e.MaxScore).HasColumnName("Max Score");

                entity.HasOne(d => d.FkType)
                    .WithMany(p => p.ScoringCategories)
                    .HasForeignKey(d => d.FkTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScoringCategories_ScoringCategoryType");
            });

            modelBuilder.Entity<ScoringCategoryType>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<SituationOptions>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.DisplayOrder).HasColumnName("Display Order");

                entity.Property(e => e.DisplayString)
                    .IsRequired()
                    .HasColumnName("Display String");

                entity.Property(e => e.VisualId)
                    .IsRequired()
                    .HasColumnName("[Visual ID");
            });

            modelBuilder.Entity<SituationStatements>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_Situation");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.DisplayOrder).HasColumnName("Display Order");

                entity.Property(e => e.DisplayString)
                    .IsRequired()
                    .HasColumnName("Display String");

                entity.Property(e => e.RequiresCompany).HasColumnName("Requires Company");

                entity.Property(e => e.RequiresStudentInfo).HasColumnName("Requires Student Info");

                entity.Property(e => e.VisualId)
                    .IsRequired()
                    .HasColumnName("Visual ID");
            });

            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Course).IsRequired();

                entity.Property(e => e.Instituion).IsRequired();
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_User_Teams");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.AccessLevel).HasMaxLength(50);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkCountryId).HasColumnName("fkCountryID");

                entity.Property(e => e.FkGenderId).HasColumnName("fkGenderID");

                entity.Property(e => e.FkProvinceId).HasColumnName("fkProvinceID");

                entity.Property(e => e.FkRaceId).HasColumnName("fkRaceID");

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.ZipCode).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMember_CaseInformation1");

                entity.HasOne(d => d.FkCountry)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMember_Countries");

                entity.HasOne(d => d.FkGender)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkGenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMember_Genders");

                entity.HasOne(d => d.FkProvince)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkProvinceId)
                    .HasConstraintName("FK_TeamMember_Provinces");

                entity.HasOne(d => d.FkRace)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkRaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamMember_Race");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_User_1");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ContactNo).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.FkProvinceId).HasColumnName("fkProvinceID");

                entity.Property(e => e.FkUserLevelId).HasColumnName("fkUserLevelID");

                entity.Property(e => e.Surname).IsRequired();

                entity.HasOne(d => d.FkProvince)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FkProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Provinces");

                entity.HasOne(d => d.FkUserLevel)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FkUserLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserLevel");
            });

            modelBuilder.Entity<UserComments>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DateStamp).HasColumnType("datetime");

                entity.Property(e => e.FkCaseId).HasColumnName("fkCaseID");

                entity.Property(e => e.FkUserId).HasColumnName("fkUserID");

                entity.HasOne(d => d.FkCase)
                    .WithMany(p => p.UserComments)
                    .HasForeignKey(d => d.FkCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserComments_CaseInformation");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UserComments)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserComments_User");
            });

            modelBuilder.Entity<UserLevel>(entity =>
            {
                entity.HasKey(e => e.PkId)
                    .HasName("PK_UserType");

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<UsersInGroup>(entity =>
            {
                entity.HasKey(e => e.PkId);

                entity.Property(e => e.PkId).HasColumnName("pkID");

                entity.Property(e => e.FkGroupId).HasColumnName("fkGroupID");

                entity.Property(e => e.FkUserId).HasColumnName("fkUserID");

                entity.HasOne(d => d.FkGroup)
                    .WithMany(p => p.UsersInGroup)
                    .HasForeignKey(d => d.FkGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersInGroup_Group");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.UsersInGroup)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersInGroup_User");
            });

            modelBuilder.Entity<VwCaseGroups>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCaseGroups");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName).IsUnicode(false);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.PkId).HasColumnName("pkID");
            });

            modelBuilder.Entity<VwUserGroups>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserGroups");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Surname).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
