namespace DhlDBFirstApp.Models;

public partial class Learner
{
    public int LearnerID { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstMidName { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
    public int MajorID { get; set; }
    public virtual Major Major { get; set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
