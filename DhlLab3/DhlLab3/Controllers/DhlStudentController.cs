using DhlLab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DhlLab3.Controllers
{
    public class DhlStudentController : Controller
    {
        private List<DhlStudent> listStudents = new List<DhlStudent>();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DhlStudentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

            listStudents = new List<DhlStudent>()
            {
                new DhlStudent() { Id = 101, Name = "Hải Nam", Branch = Branch.IT,
                    Gender = Gender.Male, IsRegular=true,
                    Address = "A1-2018", Email = "nam@gmail.com" },
                new DhlStudent() { Id = 102, Name = "Minh Tú", Branch = Branch.BE,
                    Gender = Gender.Female, IsRegular=true,
                    Address = "A1-2019", Email = "tu@gmail.com" },
                new DhlStudent() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE,
                    Gender = Gender.Male, IsRegular=false,
                    Address = "A1-2020", Email = "phong@gmail.com" },
                new DhlStudent() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE,
                    Gender = Gender.Female, IsRegular = false,
                    Address = "A1-2021", Email = "mai@gmail.com" }
            };
        }

        [Route("Admin/Student/List")]
        public IActionResult Index()
        {
            return View(listStudents);
        }

        // GET: Chỉ hiển thị form trống, không nhận model để tránh ModelState báo lỗi sớm
        [HttpGet("Admin/Student/Add")]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };

            return View();
        }

        // POST: Xử lý dữ liệu form sau khi bấm Create
        [HttpPost("Admin/Student/Add")]
        public IActionResult Create(DhlStudent s, IFormFile? avatarFile)
        {
            // 1. Xử lý upload ảnh nếu người dùng chọn file
            if (avatarFile != null && avatarFile.Length > 0)
            {
                // Tạo đường dẫn đến thư mục wwwroot/images
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                // Tạo thư mục images nếu chưa có
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Đặt tên file duy nhất bằng Guid để tránh trùng tên
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vào thư mục wwwroot/images
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    avatarFile.CopyTo(fileStream);
                }

                // Gán tên file ảnh vào Model
                s.AvatarUrl = uniqueFileName;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
                ViewBag.AllBranches = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "BE", Value = "2" },
                    new SelectListItem { Text = "CE", Value = "3" },
                    new SelectListItem { Text = "EE", Value = "4" }
                };
                return View(s);
            }

            s.Id = listStudents.Last<DhlStudent>().Id + 1;
            listStudents.Add(s);
            return View("Index", listStudents);
        }
    }
}
