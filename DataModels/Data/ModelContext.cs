using System;
using System.Collections.Generic;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataModels.Data;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AqSchedule> AqSchedules { get; set; }

    public virtual DbSet<Help> Helps { get; set; }

    public virtual DbSet<MviewAdvAjg> MviewAdvAjgs { get; set; }

    public virtual DbSet<MviewAdvBasetable> MviewAdvBasetables { get; set; }

    public virtual DbSet<MviewAdvClique> MviewAdvCliques { get; set; }

    public virtual DbSet<MviewAdvEligible> MviewAdvEligibles { get; set; }

    public virtual DbSet<MviewAdvException> MviewAdvExceptions { get; set; }

    public virtual DbSet<MviewAdvFilter> MviewAdvFilters { get; set; }

    public virtual DbSet<MviewAdvFilterinstance> MviewAdvFilterinstances { get; set; }

    public virtual DbSet<MviewAdvFjg> MviewAdvFjgs { get; set; }

    public virtual DbSet<MviewAdvGc> MviewAdvGcs { get; set; }

    public virtual DbSet<MviewAdvInfo> MviewAdvInfos { get; set; }

    public virtual DbSet<MviewAdvJournal> MviewAdvJournals { get; set; }

    public virtual DbSet<MviewAdvLevel> MviewAdvLevels { get; set; }

    public virtual DbSet<MviewAdvLog> MviewAdvLogs { get; set; }

    public virtual DbSet<MviewAdvOutput> MviewAdvOutputs { get; set; }

    public virtual DbSet<MviewAdvParameter> MviewAdvParameters { get; set; }

    public virtual DbSet<MviewAdvPlan> MviewAdvPlans { get; set; }

    public virtual DbSet<MviewAdvPretty> MviewAdvPretties { get; set; }

    public virtual DbSet<MviewAdvRollup> MviewAdvRollups { get; set; }

    public virtual DbSet<MviewAdvSqldepend> MviewAdvSqldepends { get; set; }

    public virtual DbSet<MviewAdvTemp> MviewAdvTemps { get; set; }

    public virtual DbSet<MviewAdvWorkload> MviewAdvWorkloads { get; set; }

    public virtual DbSet<MviewEvaluation> MviewEvaluations { get; set; }

    public virtual DbSet<MviewException> MviewExceptions { get; set; }

    public virtual DbSet<MviewFilter> MviewFilters { get; set; }

    public virtual DbSet<MviewFilterinstance> MviewFilterinstances { get; set; }

    public virtual DbSet<MviewLog> MviewLogs { get; set; }

    public virtual DbSet<MviewRecommendation> MviewRecommendations { get; set; }

    public virtual DbSet<MviewWorkload> MviewWorkloads { get; set; }

    public virtual DbSet<Ol> Ols { get; set; }

    public virtual DbSet<OlHint> OlHints { get; set; }

    public virtual DbSet<OlNode> OlNodes { get; set; }

    public virtual DbSet<ProductPriv> ProductPrivs { get; set; }

    public virtual DbSet<RedoDb> RedoDbs { get; set; }

    public virtual DbSet<RedoLog> RedoLogs { get; set; }

    public virtual DbSet<SchedulerJobArg> SchedulerJobArgs { get; set; }

    public virtual DbSet<SchedulerJobArgsTbl> SchedulerJobArgsTbls { get; set; }

    public virtual DbSet<SchedulerProgramArg> SchedulerProgramArgs { get; set; }

    public virtual DbSet<SchedulerProgramArgsTbl> SchedulerProgramArgsTbls { get; set; }

    public virtual DbSet<SqlplusProductProfile> SqlplusProductProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=system;Password=oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.200)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=freepdb1)))");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<AqSchedule>(entity =>
        {
            entity.HasKey(e => new { e.Oid, e.Destination }).HasName("AQ$_SCHEDULES_PRIMARY");
        });

        modelBuilder.Entity<Help>(entity =>
        {
            entity.HasKey(e => new { e.Topic, e.Seq }).HasName("HELP_TOPIC_SEQ");
        });

        modelBuilder.Entity<MviewAdvAjg>(entity =>
        {
            entity.HasKey(e => e.Ajgid).HasName("MVIEW$_ADV_AJG_PK");

            entity.ToTable("MVIEW$_ADV_AJG", tb => tb.HasComment("Anchor-join graph representation"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvAjgs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_AJG_FK");
        });

        modelBuilder.Entity<MviewAdvBasetable>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_BASETABLE", tb => tb.HasComment("Base tables refered by a query"));

            entity.HasOne(d => d.Query).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_BASETABLE_FK");
        });

        modelBuilder.Entity<MviewAdvClique>(entity =>
        {
            entity.HasKey(e => e.Cliqueid).HasName("MVIEW$_ADV_CLIQUE_PK");

            entity.ToTable("MVIEW$_ADV_CLIQUE", tb => tb.HasComment("Table for storing canonical form of Clique queries"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvCliques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_CLIQUE_FK");
        });

        modelBuilder.Entity<MviewAdvEligible>(entity =>
        {
            entity.HasKey(e => new { e.Sumobjn, e.Runid }).HasName("MVIEW$_ADV_ELIGIBLE_PK");

            entity.ToTable("MVIEW$_ADV_ELIGIBLE", tb => tb.HasComment("Summary management rewrite eligibility information"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvEligibles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_ELIGIBLE_FK");
        });

        modelBuilder.Entity<MviewAdvException>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_EXCEPTIONS", tb => tb.HasComment("Output table for dimension validations"));

            entity.HasOne(d => d.Run).WithMany().HasConstraintName("MVIEW$_ADV_EXCEPTION_FK");
        });

        modelBuilder.Entity<MviewAdvFilter>(entity =>
        {
            entity.HasKey(e => new { e.Filterid, e.Subfilternum }).HasName("MVIEW$_ADV_FILTER_PK");

            entity.ToTable("MVIEW$_ADV_FILTER", tb => tb.HasComment("Table for workload filter definition"));
        });

        modelBuilder.Entity<MviewAdvFilterinstance>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_FILTERINSTANCE", tb => tb.HasComment("Table for workload filter instance definition"));

            entity.HasOne(d => d.Run).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_FILTERINSTANCE_FK");
        });

        modelBuilder.Entity<MviewAdvFjg>(entity =>
        {
            entity.HasKey(e => e.Fjgid).HasName("MVIEW$_ADV_FJG_PK");

            entity.ToTable("MVIEW$_ADV_FJG", tb => tb.HasComment("Representation for query join sub-graph not in AJG "));

            entity.HasOne(d => d.Ajg).WithMany(p => p.MviewAdvFjgs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_FJG_FK");
        });

        modelBuilder.Entity<MviewAdvGc>(entity =>
        {
            entity.HasKey(e => e.Gcid).HasName("MVIEW$_ADV_GC_PK");

            entity.ToTable("MVIEW$_ADV_GC", tb => tb.HasComment("Group-by columns of a query"));

            entity.HasOne(d => d.Fjg).WithMany(p => p.MviewAdvGcs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_GC_FK");
        });

        modelBuilder.Entity<MviewAdvInfo>(entity =>
        {
            entity.HasKey(e => new { e.Runid, e.Seq }).HasName("MVIEW$_ADV_INFO_PK");

            entity.ToTable("MVIEW$_ADV_INFO", tb => tb.HasComment("Internal table for passing information from the SQL analyzer"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_INFO_FK");
        });

        modelBuilder.Entity<MviewAdvJournal>(entity =>
        {
            entity.HasKey(e => new { e.Runid, e.Seq }).HasName("MVIEW$_ADV_JOURNAL_PK");

            entity.ToTable("MVIEW$_ADV_JOURNAL", tb => tb.HasComment("Summary advisor journal table for debugging and information"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvJournals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_JOURNAL_FK");
        });

        modelBuilder.Entity<MviewAdvLevel>(entity =>
        {
            entity.HasKey(e => new { e.Runid, e.Levelid }).HasName("MVIEW$_ADV_LEVEL_PK");

            entity.ToTable("MVIEW$_ADV_LEVEL", tb => tb.HasComment("Level definition"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_LEVEL_FK");
        });

        modelBuilder.Entity<MviewAdvLog>(entity =>
        {
            entity.HasKey(e => e.Runid).HasName("MVIEW$_ADV_LOG_PK");

            entity.ToTable("MVIEW$_ADV_LOG", tb => tb.HasComment("Log all calls to summary advisory functions"));
        });

        modelBuilder.Entity<MviewAdvOutput>(entity =>
        {
            entity.HasKey(e => new { e.Runid, e.Rank }).HasName("MVIEW$_ADV_OUTPUT_PK");

            entity.ToTable("MVIEW$_ADV_OUTPUT", tb => tb.HasComment("Output table for summary recommendations and evaluations"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvOutputs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_OUTPUT_FK");
        });

        modelBuilder.Entity<MviewAdvParameter>(entity =>
        {
            entity.HasKey(e => e.ParameterName).HasName("MVIEW$_ADV_PARAMETERS_PK");

            entity.ToTable("MVIEW$_ADV_PARAMETERS", tb => tb.HasComment("Summary advisor tuning parameters"));
        });

        modelBuilder.Entity<MviewAdvPlan>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_PLAN", tb => tb.HasComment("Private plan table for estimate_mview_size operations"));
        });

        modelBuilder.Entity<MviewAdvPretty>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_PRETTY", tb => tb.HasComment("Table for sql parsing"));
        });

        modelBuilder.Entity<MviewAdvRollup>(entity =>
        {
            entity.HasKey(e => new { e.Runid, e.Clevelid, e.Plevelid }).HasName("MVIEW$_ADV_ROLLUP_PK");

            entity.ToTable("MVIEW$_ADV_ROLLUP", tb => tb.HasComment("Each row repesents either a functional dependency or join-key relationship"));

            entity.HasOne(d => d.Run).WithMany(p => p.MviewAdvRollups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_ROLLUP_FK");

            entity.HasOne(d => d.MviewAdvLevel).WithMany(p => p.MviewAdvRollupMviewAdvLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_ROLLUP_CFK");

            entity.HasOne(d => d.MviewAdvLevelNavigation).WithMany(p => p.MviewAdvRollupMviewAdvLevelNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MVIEW$_ADV_ROLLUP_PFK");
        });

        modelBuilder.Entity<MviewAdvSqldepend>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_SQLDEPEND", tb => tb.HasComment("Temporary table for workload collections"));
        });

        modelBuilder.Entity<MviewAdvTemp>(entity =>
        {
            entity.ToTable("MVIEW$_ADV_TEMP", tb => tb.HasComment("Table for temporary data"));
        });

        modelBuilder.Entity<MviewAdvWorkload>(entity =>
        {
            entity.HasKey(e => e.Queryid).HasName("MVIEW$_ADV_WORKLOAD_PK");

            entity.ToTable("MVIEW$_ADV_WORKLOAD", tb => tb.HasComment("Shared workload repository for DBA users of summary advisor"));
        });

        modelBuilder.Entity<MviewEvaluation>(entity =>
        {
            entity.ToView("MVIEW_EVALUATIONS");
        });

        modelBuilder.Entity<MviewException>(entity =>
        {
            entity.ToView("MVIEW_EXCEPTIONS");
        });

        modelBuilder.Entity<MviewFilter>(entity =>
        {
            entity.ToView("MVIEW_FILTER");
        });

        modelBuilder.Entity<MviewFilterinstance>(entity =>
        {
            entity.ToView("MVIEW_FILTERINSTANCE");
        });

        modelBuilder.Entity<MviewLog>(entity =>
        {
            entity.ToView("MVIEW_LOG");
        });

        modelBuilder.Entity<MviewRecommendation>(entity =>
        {
            entity.ToView("MVIEW_RECOMMENDATIONS");
        });

        modelBuilder.Entity<MviewWorkload>(entity =>
        {
            entity.ToView("MVIEW_WORKLOAD");
        });

        modelBuilder.Entity<ProductPriv>(entity =>
        {
            entity.ToView("PRODUCT_PRIVS");
        });

        modelBuilder.Entity<SchedulerJobArg>(entity =>
        {
            entity.ToView("SCHEDULER_JOB_ARGS");
        });

        modelBuilder.Entity<SchedulerProgramArg>(entity =>
        {
            entity.ToView("SCHEDULER_PROGRAM_ARGS");
        });
        modelBuilder.HasSequence("MVIEW$_ADVSEQ_GENERIC");
        modelBuilder.HasSequence("MVIEW$_ADVSEQ_ID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
