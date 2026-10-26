namespace DhlDBFirstApp.Models;

public partial class Enrollment
{
    public int EnrollmentID { get; set; }
    public int CourseID { get; set; }
    public int LearnerID { get; set; }
    public float Grade { get; set; }
    public virtual Course Course { get; set; } = null!;
    public virtual Learner Learner { get; set; } = null!;
}
