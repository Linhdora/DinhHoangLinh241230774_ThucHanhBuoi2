using Microsoft.EntityFrameworkCore;

namespace DhlDBFirstApp.Models;

public partial class DhlSchoolContext : DbContext
{
    public DhlSchoolContext(DbContextOptions<DhlSchoolContext> options) : base(options) { }

    public virtual DbSet<Course> Courses => Set<Course>();
    public virtual DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public virtual DbSet<Learner> Learners => Set<Learner>();
    public virtual DbSet<Major> Majors => Set<Major>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");
            entity.HasKey(e => e.CourseID);
            entity.Property(e => e.CourseID).ValueGeneratedNever();
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.ToTable("Major");
            entity.HasKey(e => e.MajorID);
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.ToTable("Learner");
            entity.HasKey(e => e.LearnerID);
            entity.HasOne(e => e.Major)
                .WithMany(e => e.Learners)
                .HasForeignKey(e => e.MajorID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollment");
            entity.HasKey(e => e.EnrollmentID);
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Learner)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.LearnerID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
