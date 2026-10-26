namespace DhlDBFirstApp.Models;

public partial class Major
{
    public int MajorID { get; set; }
    public string MajorName { get; set; } = null!;
    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
