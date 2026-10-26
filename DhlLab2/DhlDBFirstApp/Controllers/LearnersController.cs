using DhlDBFirstApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DhlDBFirstApp.Controllers;

public class LearnersController(DhlSchoolContext context) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await context.Learners.AsNoTracking().Include(l => l.Major).OrderBy(l => l.LearnerID).ToListAsync(cancellationToken));

    public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
    {
        if (id is null) return NotFound();
        var learner = await context.Learners.AsNoTracking().Include(l => l.Major)
            .FirstOrDefaultAsync(l => l.LearnerID == id, cancellationToken);
        return learner is null ? NotFound() : View(learner);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateMajorsAsync(null, cancellationToken);
        return View(new Learner { EnrollmentDate = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            context.Add(learner);
            await context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        await PopulateMajorsAsync(learner.MajorID, cancellationToken);
        return View(learner);
    }

    public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
    {
        if (id is null) return NotFound();
        var learner = await context.Learners.FindAsync([id.Value], cancellationToken);
        if (learner is null) return NotFound();
        await PopulateMajorsAsync(learner.MajorID, cancellationToken);
        return View(learner);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner, CancellationToken cancellationToken)
    {
        if (id != learner.LearnerID) return NotFound();
        if (ModelState.IsValid)
        {
            context.Update(learner);
            try { await context.SaveChangesAsync(cancellationToken); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Learners.AnyAsync(e => e.LearnerID == id, cancellationToken)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await PopulateMajorsAsync(learner.MajorID, cancellationToken);
        return View(learner);
    }

    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id is null) return NotFound();
        var learner = await context.Learners.AsNoTracking().Include(l => l.Major).Include(l => l.Enrollments)
            .FirstOrDefaultAsync(l => l.LearnerID == id, cancellationToken);
        if (learner is null) return NotFound();
        if (learner.Enrollments.Count > 0)
        {
            TempData["DeleteError"] = "Không thể xóa người học này vì vẫn còn đăng ký học.";
            return RedirectToAction(nameof(Index));
        }
        return View(learner);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var learner = await context.Learners.Include(l => l.Enrollments).FirstOrDefaultAsync(l => l.LearnerID == id, cancellationToken);
        if (learner is null) return NotFound();
        if (learner.Enrollments.Count > 0)
        {
            TempData["DeleteError"] = "Không thể xóa người học này vì vẫn còn đăng ký học.";
            return RedirectToAction(nameof(Index));
        }
        context.Learners.Remove(learner);
        await context.SaveChangesAsync(cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMajorsAsync(int? selectedMajorId, CancellationToken cancellationToken) =>
        ViewData["MajorID"] = new SelectList(await context.Majors.AsNoTracking().OrderBy(m => m.MajorName).ToListAsync(cancellationToken), "MajorID", "MajorName", selectedMajorId);
}
